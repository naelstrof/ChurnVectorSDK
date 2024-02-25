using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

public class Modding : MonoBehaviour {
    private static List<Mod> mods = new List<Mod>();
    private static List<TagActionPair> modActions = new List<TagActionPair>();
    private static List<IResourceLocator> locators = new List<IResourceLocator>();
    private static Dictionary<string, List<Object>> recordedLoads = new();
    public const string uniquePathReplacementID = "{fe63e628-ea59-46e9-8411-850a7865c194}";
    
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
    private void Start() {
        LoadMods();
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
        mods = new List<Mod>();
        foreach (var directory in Directory.EnumerateDirectories(Path.Combine(Application.persistentDataPath,"mods"))) {
            try {
                mods.Add(new LocalMod(new DirectoryInfo(directory)));
            } catch (Exception e) {
                Debug.LogException(e);
            }
        }
        foreach (var mod in mods) {
            mod.finishedLoading += OnModFinishedLoading;
            mod.Load();
        }
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
