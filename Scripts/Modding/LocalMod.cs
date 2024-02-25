using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LocalMod : Mod {
    private DirectoryInfo modPath;
    private ModDescription description;
    
    public LocalMod(DirectoryInfo modPath) {
        this.modPath = modPath;
        foreach (var file in modPath.GetFiles()) {
            if (file.Name != "info.json") continue;
            description = new ModDescription(file);
            break;
        }
    }

    protected override DirectoryInfo GetModPath() {
        return modPath;
    }

    public override ModDescription GetDescription() {
        return description;
    }
}
