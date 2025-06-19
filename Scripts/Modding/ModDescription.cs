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

    public IReadOnlyCollection<ReplacementCharacter> GetReplacementCharacters() {
        return replacementCharacters.AsReadOnly();
    }

    public Sprite GetPreviewImage()
    {
        Rect rec = new Rect(0, 0, preview.width, preview.height);
        return Sprite.Create(preview, rec, Vector2.zero);
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

    public void SetModActive(bool active)
    {
        this.active = active;
    }

    public void LoadPreferences(JSONNode rootNode)
    {
        JSONNode node = rootNode[publishedFileId?.ToString()].Or(JSONNode.Parse("{}"));
        active = node[nameof(active)];
    }

    public void SavePreferences(JSONNode rootNode)
    {
        JSONNode node = rootNode[publishedFileId?.ToString()].Or(JSONNode.Parse("{}"));
        node[nameof(active)] = active;

        rootNode[publishedFileId?.ToString()] = node;
    }

    public void Destroy() {
        if (preview != null) {
            Object.Destroy(preview);
        }
    }
}
