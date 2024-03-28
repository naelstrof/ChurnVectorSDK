using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SimpleJSON;
using Steamworks;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Build.DataBuilders;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.Localization;
using UnityEngine.Rendering;

[CustomEditor(typeof(ModProfile))]
public class ModProfileEditor : Editor {
    private static bool SupportsBuildPlatform(BuildTarget target) {
	    var moduleManager = System.Type.GetType("UnityEditor.Modules.ModuleManager,UnityEditor.dll");
	    var isPlatformSupportLoaded = moduleManager.GetMethod("IsPlatformSupportLoaded", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
	    var getTargetStringFromBuildTarget = moduleManager.GetMethod("GetTargetStringFromBuildTarget", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
	    return (bool)isPlatformSupportLoaded.Invoke(null,new object[] {(string)getTargetStringFromBuildTarget.Invoke(null, new object[] {target})});
    }
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		if (!SupportsBuildPlatform(BuildTarget.StandaloneWindows64)) {
			EditorGUILayout.HelpBox("You must install the Windows Mono support for this verison of unity and restart the editor.", MessageType.Error);
			GUI.enabled = false;
		}
		if (!SupportsBuildPlatform(BuildTarget.StandaloneLinux64)) {
			EditorGUILayout.HelpBox("You must install the Linux Mono support for this verison of unity and restart the editor.", MessageType.Error);
			GUI.enabled = false;
		}
		if (!SupportsBuildPlatform(BuildTarget.StandaloneOSX)) {
			EditorGUILayout.HelpBox("You must install the Linux Mono support for this verison of unity and restart the editor.", MessageType.Error);
			GUI.enabled = false;
		}
		if (GUILayout.Button("Build Locally")) {
			((ModProfile)target).Build();
		}
		if (GUILayout.Button("Upload to Steam")) {
			var wizard = ScriptableWizard.DisplayWizard<ModUploadWizard>("Steam workshop uploader","Close");
			wizard.StartUpload((ModProfile)target);
		}
		EditorGUILayout.HelpBox("Only need to download metadata from steam if you customized the item on the website.", MessageType.Info);
		if (GUILayout.Button("Download metadata from Steam")) {
			var wizard = ScriptableWizard.DisplayWizard<ModUploadWizard>("Steam workshop downloader","Close");
			wizard.StartDownload((ModProfile)target);
		}
	}
}

public class ModUploadWizard : ScriptableWizard {
	private static ModUploadWizard instance;
	private SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;
	[AOT.MonoPInvokeCallback(typeof(SteamAPIWarningMessageHook_t))]
	private void SteamAPIDebugTextHook(int nSeverity, System.Text.StringBuilder pchDebugText) {
		Debug.LogWarning(pchDebugText);
	}

	private ModProfile target;

	private void OnWizardCreate() {
		// Do nothing!
	}

	public void StartUpload(ModProfile target) {
		if (!isValid) {
			return;
		}
		this.target = target;
		target.Upload();
	}
	
	public void StartDownload(ModProfile target) {
		if (!isValid) {
			return;
		}
		this.target = target;
		target.Download();
	}
	
	protected override bool DrawWizardGUI() {
		target.OnInspectorGUI();
		return true;
	}

	private void OnEnable() {
		EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
		if (instance != null) {
			instance.Close();
		}
		instance = this;
		isValid = SteamAPI.Init();
		if (!isValid) {
			Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.");
			Close();
		}
		if (m_SteamAPIWarningMessageHook == null) {
			m_SteamAPIWarningMessageHook = SteamAPIDebugTextHook;
			SteamClient.SetWarningMessageHook(m_SteamAPIWarningMessageHook);
		}
	}
	private void OnInspectorUpdate() {
		if (isValid) {
			SteamAPI.RunCallbacks();
		}
	}
	private void OnPlayModeStateChanged(PlayModeStateChange change) {
		Close();
	}
	private void OnDisable() {
		EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
		target.Interrupt();
		if (isValid) {
			SteamAPI.Shutdown();
		}
	}
}

[CreateAssetMenu(fileName = "New ModProfile", menuName = "Data/Mod Profile", order = 36)]
public class ModProfile : ScriptableObject {
	private static readonly AppId_t AppID = (AppId_t)2686900; // Churn Vector ID
	[SerializeField] private ulong steamWorkshopID = (ulong)PublishedFileId_t.Invalid;
	[SerializeField] private ERemoteStoragePublishedFileVisibility visibility;
    [SerializeField] private string title;
    [SerializeField,Multiline] private string description;
    [SerializeField,Multiline] private string changeNotes;
    [SerializeField] private Sprite previewIcon;
	[SerializeField] private SteamWorkshopItemTag tags;
	private enum SteamWorkshopLanguage {
		english, arabic, bulgarian, schinese, tchinese, czech, danish, dutch, finnish, french, german, greek, hungarian,
		italian, japanese, koreana, norwegian, polish, portuguese, brazilian, romanian, russian, spanish, latam, swedish,
		thai, turkish, ukrainian, vietnamese,
	}
	[SerializeField] private SteamWorkshopLanguage language;
	
	[SerializeField] private List<StringTableCollection> customLocalization;
    
    [System.Serializable]
    private class LevelReference : AssetReferenceT<Level> {
        public LevelReference(string guid) : base(guid) { }
    }
    
    [System.Serializable]
    private class CharacterReplacement {
        [SerializeField] private CivilianReference existingCharacter;
        [SerializeField] private CivilianReference replacementCharacter;
        public CivilianReference GetReplacementCharacter() => replacementCharacter;
        public CivilianReference GetExistingCharacter() => existingCharacter;
    }
    
    [SerializeField] private List<LevelReference> levels;
    [SerializeField] private List<CharacterReplacement> replacementCharacters;
    
    [Flags]
    private enum SteamWorkshopItemTag {
        None = 0,
        Characters = 1,
        Maps = 2,
    }

    private struct ModProfileStatus {
		public MessageType statusType;
		public float progress;
		public string message;
    }

    private ModProfileStatus status;

    private void SetStatus(string statusMessage, float progress, MessageType messageType) {
	    status = new ModProfileStatus() {
		    statusType = messageType,
		    progress = progress,
		    message = statusMessage,
	    };
    }

    private string GetSteamDownloadPreviewPath() {
        AssetDatabase.TryGetGUIDAndLocalFileIdentifier(this, out string guid, out long localId);
        var profilePath = AssetDatabase.GUIDToAssetPath(guid);
        FileInfo profileInfo = new FileInfo(profilePath);
        var parentFolder = profileInfo.Directory;
        if (parentFolder == null) {
            throw new UnityException( $"Couldn't figure out where the file {profileInfo.FullName} is located... No permissions?");
        }
        return Path.Combine(parentFolder.FullName, $"{name}_preview.png");
    }

    private string GetBuiltPreviewPath() {
	    return Path.Combine(Application.persistentDataPath, "mods", name, "preview.png");
    }

    private string GetBuiltPath() {
        return Path.Combine(Application.persistentDataPath, "mods", name);
    }

    private CallResult<RemoteStorageDownloadUGCResult_t> onFinishedDownload;
    private CallResult<SteamUGCQueryCompleted_t> onFinishedQuery;
    public void Download() {
		PublishedFileId_t[] ids = { (PublishedFileId_t)steamWorkshopID };
		var request = SteamUGC.CreateQueryUGCDetailsRequest(ids, 1);

		onFinishedQuery = new CallResult<SteamUGCQueryCompleted_t>(OnFinishedQuery);
		var handle = SteamUGC.SendQueryUGCRequest(request);
		onFinishedQuery.Set(handle);
    }

    private void OnFinishedQuery(SteamUGCQueryCompleted_t param, bool biofailure) {
		if (!SteamUGC.GetQueryUGCResult(param.m_handle, 0, out var details)) {
			SetStatus($"Failed to download information for mod with ID {steamWorkshopID}. Does it exist and do you have permissions to view it?", 0f, MessageType.Error);
			throw new UnityException(status.message);
		}
		Undo.RecordObject(this, "Updated mod profile from SteamWorkshop");
		description = details.m_rgchDescription;
		title = details.m_rgchTitle;
		tags = SteamWorkshopItemTag.None;
		foreach (SteamWorkshopItemTag tag in (SteamWorkshopItemTag[])Enum.GetValues(typeof(SteamWorkshopItemTag))) {
			if (details.m_rgchTags.Contains(tag.ToString())) {
				tags |= tag;
			}
		}

		visibility = details.m_eVisibility;
		onFinishedDownload = new CallResult<RemoteStorageDownloadUGCResult_t>(OnFinishDownloadPreview);
		var handle = SteamRemoteStorage.UGCDownloadToLocation(details.m_hPreviewFile, GetSteamDownloadPreviewPath(), 0);
		onFinishedDownload.Set(handle);
		SetStatus("Downloading workshop preview.", 0f, MessageType.Info);
        EditorUtility.SetDirty(this);
    }

    private void OnFinishDownloadPreview(RemoteStorageDownloadUGCResult_t param, bool biofailure) {
        if (biofailure) {
			SetStatus("Failed to download preview image. Whoops!", 1f, MessageType.Error);
            throw new UnityException(status.message);
        }

        AssetDatabase.Refresh();
        var assetPath = GetSteamDownloadPreviewPath();
        assetPath = assetPath.Replace(@"\", "/");
        assetPath = assetPath.Substring(assetPath.IndexOf("Assets", StringComparison.Ordinal));
        Texture2D image = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
        if (image == null) {
			SetStatus($"Failed to download preview image at {assetPath}. Whoops!", 1f, MessageType.Error);
            throw new UnityException(status.message);
        }
        TextureImporter ti = (TextureImporter)AssetImporter.GetAtPath(assetPath);
        ti.textureType = TextureImporterType.Sprite;
        ti.SaveAndReimport();
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
        Undo.RecordObject(this, "Changed icon");
        previewIcon = sprite;
        EditorUtility.SetDirty(this);
		SetStatus("Successfully downloaded fresh metadata!", 1f, MessageType.Info);
    }

    public void Interrupt() {
	    createHandleCallback?.Dispose();
	    createHandleCallback = null;
	    onFinishedDownload?.Dispose();
	    onFinishedQuery?.Dispose();
	    onFinishedQuery = null;
	    onFinishedDownload = null;
	    onSubmitItemUpdateCallback?.Dispose();
	    onSubmitItemUpdateCallback = null;
    }

    private CallResult<CreateItemResult_t> createHandleCallback;
    public void Upload() {
		if (SteamUtils.GetAppID() != AppID) {
			SetStatus($"Wrong AppID for steamworks, please put {AppID} inside the steam_appid.txt found inside the root of your project. Then restart Unity.", 1f, MessageType.Error);
			throw new UnityException(status.message);
		}
		if (!Directory.Exists(GetBuiltPath())) {
			SetStatus("Please build your mod before trying to upload it!", 1f, MessageType.Error);
			throw new UnityException(status.message);
		}
		
		if (previewIcon == null) {
			SetStatus( "You MUST supply a preview icon! This is required, sorry! Use win+shift+s to take a quick crop image in the editor.", 1f, MessageType.Error);
			throw new UnityException(status.message);
		}
		
		if (steamWorkshopID == (ulong)PublishedFileId_t.Invalid) {
			SetStatus( "Creating new workshop item...", 0f, MessageType.Info);
			var handle = SteamUGC.CreateItem(AppID, EWorkshopFileType.k_EWorkshopFileTypeCommunity);
			createHandleCallback = new CallResult<CreateItemResult_t>(OnCreateItem);
			createHandleCallback.Set(handle);
		} else {
			ItemUpdate();
		}
    }

    private void OnCreateItem(CreateItemResult_t param, bool bIOFailure) {
		if (bIOFailure || param.m_eResult != EResult.k_EResultOK) {
			SetStatus($"Failed to upload workshop item, error code: {param.m_eResult}, Check https://partner.steamgames.com/doc/api/ISteamUGC#CreateItemResult_t for more information.", 1f, MessageType.Error);
			throw new UnityException(status.message);
		}

		if (param.m_bUserNeedsToAcceptWorkshopLegalAgreement) {
			SetStatus("Apparently you need to accept the workshop legal agreement. Go to your workshop page (or try to create a workshop item) in a browser and it should prompt you for an agreement.", 1f, MessageType.Error);
			throw new UnityException(status.message);
		}

		if (param.m_nPublishedFileId == PublishedFileId_t.Invalid) {
			SetStatus("Failed to upload, generated an invalid ID. Not sure what the problem is sorry!", 1f, MessageType.Error);
			throw new UnityException(status.message);
		}

		Undo.RecordObject(this, "Updated steamworkshopID");
		steamWorkshopID = (ulong)param.m_nPublishedFileId;
		EditorUtility.SetDirty(this);
		ItemUpdate();
    }

    private bool uploading;
    private UGCUpdateHandle_t ugcUpdateHandle;
	CallResult<SubmitItemUpdateResult_t> onSubmitItemUpdateCallback;
    private void ItemUpdate() {
	    Save(); // Make sure we serialize out our item ID, if it has changed.
	    
	    SetStatus("Setting item description...", 0f, MessageType.Info);
		ugcUpdateHandle = SteamUGC.StartItemUpdate(AppID, (PublishedFileId_t)steamWorkshopID);
		if (!SteamUGC.SetItemDescription(ugcUpdateHandle, description)) {
			SetStatus("Failed to set item description.", 1f, MessageType.Error);
			throw new UnityException(status.message);
		}

	    SetStatus("Setting item language...", 0f, MessageType.Info);
		if (!SteamUGC.SetItemUpdateLanguage(ugcUpdateHandle, language.ToString())) {
			SetStatus("Failed to set item update language.", 1f, MessageType.Error);
			throw new UnityException(status.message);
		}

	    SetStatus("Setting item title..", 0f, MessageType.Info);
		if (!SteamUGC.SetItemTitle(ugcUpdateHandle, title)) {
			SetStatus("Failed to set item title", 1f, MessageType.Error);
			throw new UnityException(status.message);
		}

		List<string> stringTags = new List<string>();
		foreach (SteamWorkshopItemTag tag in (SteamWorkshopItemTag[])Enum.GetValues(typeof(SteamWorkshopItemTag))) {
			if ((tag & tags) != 0) {
				stringTags.Add(tag.ToString());
			}
		}
	    SetStatus("Setting item tags...", 0f, MessageType.Info);
		if (!SteamUGC.SetItemTags(ugcUpdateHandle, stringTags)) {
			SetStatus("Failed to set item tags.", 1f, MessageType.Error);
			throw new UnityException(status.message);
		}

	    SetStatus("Setting item visibility...", 0f, MessageType.Info);
		if (!SteamUGC.SetItemVisibility(ugcUpdateHandle, visibility)) {
			SetStatus("Failed to set item visibility.", 1f, MessageType.Error);
			throw new UnityException(status.message);
		}

	    SetStatus("Setting item preview...", 0f, MessageType.Info);
		if (!SteamUGC.SetItemPreview(ugcUpdateHandle, GetBuiltPreviewPath())) {
			SetStatus("Failed to set item preview.", 1f, MessageType.Error);
			throw new UnityException(status.message);
		}

	    SetStatus("Setting item content...", 0f, MessageType.Info);
		if (!SteamUGC.SetItemContent(ugcUpdateHandle, GetBuiltPath())) {
			SetStatus("Failed to set item content.", 1f, MessageType.Error);
			throw new UnityException(status.message);
		}

		uploading = true;
		onSubmitItemUpdateCallback = new CallResult<SubmitItemUpdateResult_t>(OnSubmitItem);
		var handle = SteamUGC.SubmitItemUpdate(ugcUpdateHandle, string.IsNullOrEmpty(changeNotes) ? null : changeNotes);
		onSubmitItemUpdateCallback.Set(handle);
	}

	private void OnSubmitItem(SubmitItemUpdateResult_t param, bool biofailure) {
		uploading = false;
		if (param.m_eResult == EResult.k_EResultOK) {
			SetStatus($"Upload success! Check https://steamcommunity.com/sharedfiles/filedetails/?id={steamWorkshopID} to see your item.", 1f, MessageType.Info);
			Debug.Log(status.message);
		} else {
			SetStatus($"Upload failed with error {param.m_eResult}. Check https://partner.steamgames.com/doc/api/ISteamUGC#SubmitItemUpdateResult_t for more information.", 1f, MessageType.Error);
			Debug.LogError(status.message);
		}
	}

    public void Build() {
        var settings = AddressableAssetSettingsDefaultObject.Settings;

        var group = settings.FindGroup(name);
        if (group == null) {
	        group = settings.CreateGroup(name, false, false, true, settings.DefaultGroup.Schemas);
        }

		var schema = group.GetSchema<BundledAssetGroupSchema>();
		Undo.RecordObject(schema, "Schema change.");
		schema.IncludeInBuild = true;
        try {
	        
	        Undo.RecordObject(group, "Filled custom group.");

	        foreach (var level in levels) {
		        var entry = settings.CreateOrMoveEntry(level.AssetGUID, group, false, false);
		        entry.SetLabel("ChurnVectorLevel", true, true);
				settings.CreateOrMoveEntry(level.editorAsset.GetSceneReference().AssetGUID, group, false, false);
	        }

	        foreach (var stringTableCollection in customLocalization) {
		        if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(stringTableCollection.SharedData, out string shareDataGuid, out long shareDataLocalId)) {
			        settings.CreateOrMoveEntry(shareDataGuid, group, false, false);
		        }

		        foreach (var stringTable in stringTableCollection.StringTables) {
			        if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(stringTable, out string guid, out long localId)) {
						settings.CreateOrMoveEntry(guid, group, false, false);
			        }
		        }
	        }

	        // Don't allow things in default group
	        foreach (var entry in settings.DefaultGroup.entries) {
		        settings.CreateOrMoveEntry(entry.guid, group, false, false);
	        }

	        PlayerSettings.companyName = "ArchivalEugeneNaelstrof";
	        PlayerSettings.productName = "ChurnVector";

	        var churnVectorCharacters = settings.FindGroup("ChurnVectorCharacters");
	        var characterEntries = churnVectorCharacters.entries;
	        foreach (var replacement in replacementCharacters) {
		        var replacementCharacterID = replacement.GetReplacementCharacter().AssetGUID;
		        var found = characterEntries.FirstOrDefault((entry) => entry.guid == replacementCharacterID);
		        // Double check that we aren't replacing assets with existing assets before adding it to our bundle.
		        if (found != null) {
			        throw new UnityException(
				        "You cannot replace existing characters with other existing characters. Please duplicate them into an original asset (shift+d)! (This is to prevent a zero size bundle, which isn't supported.)");
		        }

		        var entry = settings.CreateOrMoveEntry(replacementCharacterID, group, false, false);
		        entry.SetLabel("ChurnVectorCharacter", true, true);
	        }

	        var defaultBuildPath = "[UnityEngine.AddressableAssets.Addressables.BuildPath]/[BuildTarget]";
	        var defaultLoadPath = "{UnityEngine.AddressableAssets.Addressables.RuntimePath}/[BuildTarget]";

	        settings.profileSettings.SetValue(settings.activeProfileId, "Local.BuildPath", defaultBuildPath);
	        settings.profileSettings.SetValue(settings.activeProfileId, "Local.LoadPath", defaultLoadPath);

	        var modBuildPath = $"[UnityEngine.Application.persistentDataPath]/mods/{name}/[BuildTarget]";
	        var modLoadPath = $"{Modding.uniquePathReplacementID}/[BuildTarget]";

	        var buildPathInfo = settings.profileSettings.GetProfileDataByName("ChurnVectorMod.BuildPath");
	        if (buildPathInfo == null) {
		        settings.profileSettings.CreateValue("ChurnVectorMod.BuildPath", modBuildPath);
		        settings.profileSettings.CreateValue("ChurnVectorMod.LoadPath", modLoadPath);
		        settings.profileSettings.SetValue(settings.activeProfileId, "ChurnVectorMod.BuildPath", modBuildPath);
		        settings.profileSettings.SetValue(settings.activeProfileId, "ChurnVectorMod.LoadPath", modLoadPath);
	        } else {
		        settings.profileSettings.SetValue(settings.activeProfileId, "ChurnVectorMod.BuildPath", modBuildPath);
		        settings.profileSettings.SetValue(settings.activeProfileId, "ChurnVectorMod.LoadPath", modLoadPath);
	        }

	        EditorUtility.SetDirty(group);

	        ExternalCatalogSetup externalCatalog = ScriptableObject.CreateInstance<ExternalCatalogSetup>();
	        externalCatalog.AssetGroups = new List<AddressableAssetGroup> { group };
	        buildPathInfo = settings.profileSettings.GetProfileDataByName("ChurnVectorMod.BuildPath");
	        externalCatalog.BuildPath.SetVariableById(settings, buildPathInfo.Id);
	        var runtimeLoadPathInfo = settings.profileSettings.GetProfileDataByName("ChurnVectorMod.LoadPath");
	        externalCatalog.RuntimeLoadPath.SetVariableById(settings, runtimeLoadPathInfo.Id);
	        externalCatalog.CatalogName = name;
	        AssetDatabase.CreateAsset(externalCatalog,
		        Path.Combine(settings.DataBuilderFolder, "ExternalCatalog.asset"));

	        schema.BuildPath.SetVariableById(settings, buildPathInfo.Id);
	        schema.LoadPath.SetVariableById(settings, runtimeLoadPathInfo.Id);

	        BuildScriptPackedMultiCatalogMode multicatalog =
		        (BuildScriptPackedMultiCatalogMode)settings.DataBuilders.FirstOrDefault((builder) =>
			        builder is BuildScriptPackedMultiCatalogMode);
	        if (multicatalog == null) {
		        multicatalog = CreateInstance<BuildScriptPackedMultiCatalogMode>();
		        multicatalog.ExternalCatalogs = new List<ExternalCatalogSetup> { externalCatalog };
		        AssetDatabase.CreateAsset(multicatalog,
			        Path.Combine(settings.DataBuilderFolder, "BuildScriptPackedMultiCatalog.asset"));
		        Undo.RecordObject(settings, "Added builder to settings.");
		        settings.DataBuilders.Add(multicatalog);
	        }

	        Undo.RecordObject(multicatalog, "Set externalCatalogs");
	        multicatalog.ExternalCatalogs = new List<ExternalCatalogSetup> { externalCatalog };
	        Undo.RecordObject(settings, "Set active build index");
	        settings.ActivePlayerDataBuilderIndex = settings.DataBuilders.IndexOf(multicatalog);

	        if (Directory.Exists(GetBuiltPath())) {
		        Directory.Delete(GetBuiltPath(), true);
	        }

	        var currentBuildTarget = EditorUserBuildSettings.activeBuildTarget;
	        try {
		        BuildForPlatform(BuildTarget.StandaloneWindows64);
		        BuildForPlatform(BuildTarget.StandaloneLinux64);
		        BuildForPlatform(BuildTarget.StandaloneOSX);
	        } finally {
		        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, currentBuildTarget);
	        }

	        Save();
	        EditorUtility.RevealInFinder(GetBuiltPath());
        } finally {
	        schema.IncludeInBuild = false;
        }
    }

    private void Save() {
        JSONNode infoJson = JSONNode.Parse("{}");
        infoJson["title"] = title;
        infoJson["description"] = description;
        infoJson["publishedFileId"] = steamWorkshopID;
        AssetDatabase.TryGetGUIDAndLocalFileIdentifier(previewIcon, out string iconGUID, out long localid);
        infoJson["spriteGUID"] = iconGUID;
        var arrayNode = new JSONArray();
        foreach (SteamWorkshopItemTag tag in (SteamWorkshopItemTag[])Enum.GetValues(typeof(SteamWorkshopItemTag))) {
            if ((tag & tags) != 0) {
                arrayNode.Add(tag.ToString());
            }
        }
        infoJson["tags"] = arrayNode;
        var replacementJSON = new JSONArray();
        foreach (var replacement in replacementCharacters) {
            JSONNode characterReplacement = JSONNode.Parse("{}");
            characterReplacement["existingCharacter"] = replacement.GetExistingCharacter().AssetGUID;
            characterReplacement["replacementCharacter"] = replacement.GetReplacementCharacter().AssetGUID;
            replacementJSON.Add(characterReplacement);
        }
        infoJson["replacementCharacters"] = replacementJSON;
        
        StreamWriter infoWriter = new StreamWriter(Path.Combine(Application.persistentDataPath, "mods", name, "info.json"));
        infoWriter.Write(infoJson.ToString(2));
        infoWriter.Close();
        if (previewIcon != null) {
	        WriteOutSpriteToPath(GetBuiltPreviewPath(), previewIcon);
        }
    }

    private void WriteOutSpriteToPath(string path, Sprite sprite) {
        // Write out the sprite to a render texture, so that we can ignore its "readOnly" state.
        var rect = sprite.textureRect;
        RenderTexture targetTexture = new RenderTexture((int)rect.width, (int)rect.height, 0);
        CommandBuffer buffer = new CommandBuffer();
        buffer.Blit(sprite.texture, targetTexture, Vector2.one, sprite.textureRectOffset);
        Graphics.ExecuteCommandBuffer(buffer);

        // Then read-back the render texture to a regular texture2D.
        Texture2D previewTextureWrite = new Texture2D((int)rect.width, (int)rect.height);
        RenderTexture oldTex = RenderTexture.active;
        RenderTexture.active = targetTexture;
        previewTextureWrite.ReadPixels(new Rect(0, 0, rect.width, rect.height), 0, 0);
        File.WriteAllBytes(path, previewTextureWrite.EncodeToPNG());
        RenderTexture.active = oldTex;
    }

    private void BuildForPlatform(BuildTarget target) {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, target);
        AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);
        if (result == null) {
            throw new Exception("Something went very wrong!");
        }

        if (!string.IsNullOrEmpty(result.Error)) {
            throw new UnityException(result.Error);
        }
    }

    public void OnInspectorGUI() {
	    if (uploading) {
			var uploadStatus = SteamUGC.GetItemUpdateProgress(ugcUpdateHandle, out ulong punBytesProcessed, out ulong punBytesTotal);
			switch (uploadStatus) {
				case EItemUpdateStatus.k_EItemUpdateStatusPreparingConfig:
				case EItemUpdateStatus.k_EItemUpdateStatusCommittingChanges:
				case EItemUpdateStatus.k_EItemUpdateStatusPreparingContent:
				case EItemUpdateStatus.k_EItemUpdateStatusUploadingPreviewFile:
				case EItemUpdateStatus.k_EItemUpdateStatusUploadingContent:
					SetStatus($"Uploading... {uploadStatus.ToString()}", punBytesProcessed / (float)punBytesTotal, MessageType.Info);
					EditorGUILayout.HelpBox($"Uploading... {uploadStatus.ToString()}", MessageType.Info);
					var rect = EditorGUILayout.BeginVertical();
					EditorGUI.ProgressBar(rect, punBytesProcessed / (float)punBytesTotal, $"{punBytesProcessed}/{punBytesTotal}");
					GUILayout.Space(20);
					EditorGUILayout.EndVertical();
					break;
			}
		} else {
		    EditorGUILayout.HelpBox(status.message, status.statusType);
		    var rect = EditorGUILayout.BeginVertical();
		    EditorGUI.ProgressBar(rect, status.progress, status.message);
		    GUILayout.Space(20);
		    EditorGUILayout.EndVertical();
	    }
    }
}
