using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Steamworks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

public abstract class Mod {
    public delegate void ModLoadedAction(IResourceLocator locator);
    public event ModLoadedAction finishedLoading;
    private const string uniqueFileName = "11afa19f-4c98-4679-9404-fbff3c145fae.json";
    
    private static string GetRunningPlatform() {
        switch (Application.platform) {
            case RuntimePlatform.LinuxPlayer:
            case RuntimePlatform.LinuxServer:
            case RuntimePlatform.LinuxEditor:
                return "StandaloneLinux64";
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsServer:
                return "StandaloneWindows64";
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.OSXServer:
                return "StandaloneOSX";
            default: return "Unknown";
        } 
    }
    
    public void Load() {
        var catalogDirectory = new DirectoryInfo(Path.Combine(GetModPath().FullName, GetRunningPlatform()));
        if (!catalogDirectory.Exists) {
            throw new FileNotFoundException($"Missing catalogue data for platform {GetRunningPlatform()} for mod {GetModPath().FullName}...");
        }
        var catalogFile = catalogDirectory.GetFiles().FirstOrDefault(file => file.Name.EndsWith(".json") && file.Name != uniqueFileName);
        if (catalogFile == null) {
            throw new FileNotFoundException($"Couldn't find catalogue file for mod {GetModPath().FullName}...");
        }

        var catalogContents = catalogFile.OpenText().ReadToEnd();
        catalogContents = catalogContents.Replace(Modding.uniquePathReplacementID, GetModPath().FullName);

        var catalogOverrideFilePath = Path.Combine(catalogFile.Directory.FullName, uniqueFileName);
        StreamWriter writer = new StreamWriter(catalogOverrideFilePath);
        writer.Write(catalogContents);
        writer.Close();

        var handle = Addressables.LoadContentCatalogAsync(catalogOverrideFilePath, true);
        handle.Completed += OnLoadComplete;
    }
    
    private void OnLoadComplete(AsyncOperationHandle<IResourceLocator> obj) {
        if (obj.Status != AsyncOperationStatus.Succeeded) {
            throw new UnityException($"Failed to load mod {GetModPath().FullName}");
        }
        var resourceLocator = obj.Result;
        finishedLoading?.Invoke(resourceLocator);
    }
    
    protected abstract DirectoryInfo GetModPath();
    public abstract ModDescription GetDescription();
}
