using System.IO;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;
using SimpleJSON;
using Steamworks;

public static class SaveManager {
    [CanBeNull] private static JSONNode save;
    private const string saveDataLocation = "saves/";
    private static int? currentslot;
    
    private static string GetSaveDirectory(int slot) {
        var path = $"{Application.persistentDataPath}/defaultUser/{saveDataLocation}/";
        if (SteamManager.Initialized) {
            path = $"{Application.persistentDataPath}/{SteamUser.GetSteamID().ToString()}/{saveDataLocation}/";
        }
        return path;
    }
    
    private static string GetSaveFilePath(int slot) {
        return $"{GetSaveDirectory(slot)}save{slot.ToString()}.json";
    }

    public static void SelectSaveSlot(int slot) {
        currentslot = slot;
        save = GetData(slot);
    }

    public static int? GetSaveSlot() {
        return currentslot;
    }

    public static void DeleteSaveSlot(int slot) {
        var path = GetSaveFilePath(slot);
        if (File.Exists(path)) {
            File.Delete(path);
        }
    }

    public static JSONNode GetData() {
        return save;
    }
    
    public static JSONNode GetData(int slot) {
        var path = GetSaveFilePath(slot);
        FileInfo fileInfo = new FileInfo(path);
        if (!fileInfo.Exists) {
            Directory.CreateDirectory(GetSaveDirectory(slot));
            JSONNode rootNode = JSONNode.Parse("{}");
            using FileStream file = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
            using StreamWriter writer = new StreamWriter(file);
            writer.Write(rootNode.ToString());
        }
        using FileStream saveFile = File.Open(path, FileMode.Open);
        byte[] b = new byte[saveFile.Length];
        saveFile.Read(b,0,(int)saveFile.Length);
        saveFile.Close();
        string data = Encoding.UTF8.GetString(b);
        return JSON.Parse(data);
    }
    
    public static void Save() {
        if (!currentslot.HasValue || save == null) {
            Debug.LogError("Tried to save without a selected save slot!");
            return;
        }

        var saveDirectory = GetSaveDirectory(currentslot.Value);
        if (!Directory.Exists(saveDirectory)) {
            Directory.CreateDirectory(saveDirectory);
        }
        var filePath = GetSaveFilePath(currentslot.Value);
        using FileStream quickWrite = File.Open(filePath, FileMode.Create);
        var chars = save.ToString(2);
        quickWrite.Write(Encoding.UTF8.GetBytes(chars), 0, chars.Length);
        quickWrite.Close();
    }
}
