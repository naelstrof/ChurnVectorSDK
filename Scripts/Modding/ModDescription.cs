using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;
using Steamworks;
using UnityEngine;

public class ModDescription {
    private string title;
    private string description;
    private bool active;
    private PublishedFileId_t? publishedFileId;
    private Texture2D preview;
    private List<ReplacementCharacter> replacementCharacters;

    public string GetTitle() => title;
    public string GetDescription() => description;
    public bool IsActive() => active;
    public PublishedFileId_t? GetPublishedFileID() => publishedFileId;
    public Texture2D GetPreview() => preview;
    public struct ReplacementCharacter {
        public string existingGUID;
        public string replacementGUID;
    }

    Sprite cachedPreview = null;

    private Dictionary<(string,string), bool> activeReplacements = new Dictionary<(string,string), bool>();

    public IReadOnlyCollection<ReplacementCharacter> GetReplacementCharacters() {
        return replacementCharacters.AsReadOnly();
    }

    public Sprite GetPreviewImage()
    {
        if (cachedPreview == null)
        {
            Rect rec = new Rect(0, 0, preview.width, preview.height);
            cachedPreview = Sprite.Create(preview, rec, Vector2.zero);
        }
        return cachedPreview;
    }
    
    public ModDescription(FileInfo jsonFileInfo) {
        if (jsonFileInfo.Directory == null) {
            throw new FileNotFoundException("JSON file was in a null directory, this shouldn't happen!");
        }
        using FileStream file = jsonFileInfo.OpenRead();
        using StreamReader reader = new StreamReader(file);
        var rootNode = JSONNode.Parse(reader.ReadToEnd());
        if (rootNode.HasKey("publishedFileId")) {
            if (ulong.TryParse(rootNode["publishedFileId"], out ulong output)) {
                publishedFileId = (PublishedFileId_t)output;
            }
        }
        if (rootNode.HasKey("description")) {
            description= rootNode["description"];
        }
        if (rootNode.HasKey("title")) {
            title = rootNode["title"];
        }

        replacementCharacters = new List<ReplacementCharacter>();
        if (rootNode.HasKey("replacementCharacters")) {
            JSONArray characters = rootNode["replacementCharacters"].AsArray;
            foreach (var pair in characters) {
                replacementCharacters.Add(new ReplacementCharacter {
                    existingGUID = pair.Value["existingCharacter"],
                    replacementGUID = pair.Value["replacementCharacter"]
                });
            }
        }

        var previewPng = new FileInfo(Path.Join(jsonFileInfo.Directory.FullName, "preview.png"));
        if (previewPng.Exists) {
            preview = new Texture2D(16, 16);
            preview.LoadImage(File.ReadAllBytes(previewPng.FullName));
        }

        active = false;
    }

    public bool IsReplacementActive(string baseGUID, string replacementGUID, bool ignoreModStatus = false)
    {
        if (!active && !ignoreModStatus)
            return false;

        var key = (baseGUID, replacementGUID);
        if(activeReplacements.ContainsKey(key))
            return activeReplacements[key];

        return true;
    }

    public void SetReplacementActive(string baseGUID, string replacementGUID, bool active)
    {
        var key = (baseGUID, replacementGUID);
        activeReplacements[key] = active;
    }

    public void SetModActive(bool active)
    {
        this.active = active;
    }

    private const bool DefaultReplacementActive = true;

    public void LoadPreferences(JSONNode rootNode)
    {
        JSONNode node = rootNode[publishedFileId?.ToString()].Or(JSON.Parse("{}"));
        active = node[nameof(active)];

        JSONNode replacementNode = node["replacements"].Or(JSON.Parse("{}"));
        foreach (var replacement in replacementCharacters)
        {
            var key = (replacement.existingGUID, replacement.replacementGUID);
            activeReplacements[key] = replacementNode[$"{replacement.existingGUID}>{replacement.replacementGUID}"].Or(DefaultReplacementActive);
        }
    }

    public void SavePreferences(JSONNode rootNode)
    {
        JSONNode node = rootNode[publishedFileId?.ToString()].Or(JSON.Parse("{}"));
        node[nameof(active)] = active;

        JSONNode replacementNode = node["replacements"].Or(JSON.Parse("{}"));
        foreach (var replacement in replacementCharacters)
        {
            var key = (replacement.existingGUID, replacement.replacementGUID);
            bool status = activeReplacements.ContainsKey(key) ? activeReplacements[key] : DefaultReplacementActive;
            replacementNode[$"{replacement.existingGUID}>{replacement.replacementGUID}"] = status;
        }
        node["replacements"] = replacementNode;

        rootNode[publishedFileId?.ToString()] = node;
    }

    public void Destroy() {
        if (preview != null) {
            Object.Destroy(preview);
            preview = null;
        }
    }
}
