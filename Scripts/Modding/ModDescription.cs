using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;
using Steamworks;
using UnityEngine;

public class ModDescription {
    private string title;
    private string description;
    private PublishedFileId_t? publishedFileId;
    private Texture2D preview;
    
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

        var previewPng = new FileInfo(Path.Join(jsonFileInfo.Directory.FullName, "preview.png"));
        if (previewPng.Exists) {
            preview = new Texture2D(16, 16);
            preview.LoadImage(File.ReadAllBytes(previewPng.FullName));
        }
    }

    public void Destroy() {
        if (preview != null) {
            Object.Destroy(preview);
        }
    }
}
