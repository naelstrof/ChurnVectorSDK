using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;
using System.Linq;

public class Modding : MonoBehaviour {
    private static List<Mod> mods = new List<Mod>();
    private static List<TagActionPair> modActions = new List<TagActionPair>();
    private static List<IResourceLocator> locators = new List<IResourceLocator>();
    private static Dictionary<string, List<Object>> recordedLoads = new();
    public const string uniquePathReplacementID = "{fe63e628-ea59-46e9-8411-850a7865c194}";
    private static bool loading = true;
    private static int remainingLoads = 0;

    public static bool IsLoading() => loading || remainingLoads != 0;

    public static Task GetLoadingTask() {
        if (!IsLoading()) {
            return Task.CompletedTask;
        }

        return new Task(() => {
            while (IsLoading()) {
                Thread.Sleep(10);
            }
        });
    }
    public static IReadOnlyCollection<Mod> GetMods(bool includeInactive = false) {
        if (IsLoading()) {
            throw new UnityException( "Tried to get a character before modding was done... Please use the InitializationManager to ensure things are ready...");
        }
        LoadPreferences();

        if(!includeInactive)
            return mods.Where(mod => mod.GetDescription().IsActive()).ToList().AsReadOnly();

        return mods.AsReadOnly();
    }
    
    private struct TagActionPair {
        public string tag;
        public Action<Object> loadedAction;
        public Action<Object> unloadedAction;
    }

    public static void SubscribeToTag(string tag, Action<Object> loadedAction, Action<Object> unloadedAction) {
        var pair = new TagActionPair {
            tag = tag,
            loadedAction = loadedAction,
            unloadedAction = unloadedAction,
        };
        if (modActions.Contains(pair)) {
            return;
        }
        modActions.Add(pair);
    }
    IEnumerator Start() {
        yield return new WaitUntil(() => SteamWorkshopDownloader.GetStatus().statusType == SteamWorkshopDownloader.StatusType.Done);
        LoadMods();
    }

    public static void AddMod(LocalMod newMod) {
        var newModID = newMod.GetDescription().GetPublishedFileID();
        if (!newModID.HasValue) {
            mods.Add(newMod);
            return;
        }

        foreach (var mod in mods) {
            var existingModID = mod.GetDescription().GetPublishedFileID();
            if (existingModID != newModID) continue;
            Debug.LogError($"Tried to install a mod twice! Ignoring one mod named {newMod.GetDescription().GetTitle()}.");
            return;
        }
        mods.Add(newMod);
    }

    private void RememberLoad(string tag, Object obj) {
        if (!recordedLoads.ContainsKey(tag)) {
            recordedLoads.Add(tag, new List<Object>());
        }
        if (recordedLoads[tag].Contains(obj)) {
            return;
        }
        recordedLoads[tag].Add(obj);
    }

    private void LoadMods() {
        var dir = Path.Combine(Application.persistentDataPath, "mods");
        if (Directory.Exists(dir)) {
            foreach (var directory in Directory.EnumerateDirectories(dir)) {
                try {
                    AddMod(new LocalMod(new DirectoryInfo(directory)));
                } catch (Exception e) {
                    Debug.LogException(e);
                }
            }
        }
        if(mods.Count == 0) {
            loading = false;
            return;
        }
        foreach (var mod in mods) {
            Debug.Log("Loaded mod " + mod.GetDescription().GetTitle());
            remainingLoads++;
            mod.finishedLoading += OnModFinishedLoading;
            mod.Load();
        }
    }

    public static void LoadPreferences()
    {
        if (SaveManager.GetSaveSlot() == null)
        {
            SaveManager.SelectSaveSlot(0);
        }

        JSONNode data = SaveManager.GetData();
        JSONNode preferences = data["mods"].Or(JSONNode.Parse("{}"));

        CharacterLibrary.Load(preferences);

        foreach(Mod mod in mods)
        {
            mod.GetDescription().LoadPreferences(preferences);
        }
    }

    public static void SavePreferences()
    {
        JSONNode data = SaveManager.GetData();
        JSONNode preferences = data["mods"].Or(JSONNode.Parse("{}"));

        CharacterLibrary.Save(preferences);

        foreach (Mod mod in mods)
        {
            mod.GetDescription().SavePreferences(preferences);
        }

        data["mods"] = preferences;
        SaveManager.Save();
    }

    private void OnModFinishedLoading(IResourceLocator locator) {
        locators.Add(locator);
        foreach (var pair in modActions) {
            if (locator.Locate(pair.tag, typeof(Object), out IList<IResourceLocation> locations)) {
                Addressables.LoadAssetsAsync(locations, (Object obj) => {
                    RememberLoad(pair.tag, obj);
                    pair.loadedAction?.Invoke(obj);
                });
            }
        }
        remainingLoads--;
        if (remainingLoads == 0) {
            loading = false;
        }
    }

    private void UnloadMods() {
        foreach (var record in recordedLoads) {
            foreach (var pair in modActions) {
                if (pair.tag != record.Key) {
                    continue;
                }
                foreach (var obj in record.Value) {
                    pair.unloadedAction?.Invoke(obj);
                }
            }
        }
        recordedLoads = new Dictionary<string, List<Object>>();
        
        foreach (var locator in locators) {
            Addressables.RemoveResourceLocator(locator);
        }
        Addressables.ClearResourceLocators();
        locators.Clear();
    }

    private void ReloadMods() {
        UnloadMods();
        LoadMods();
    }
}
