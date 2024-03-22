using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Level Category", menuName = "Data/Level Category", order = 57)]
public class LevelCategory : ScriptableObject {
    [SerializeField] private Sprite thumbnail;
    [SerializeField] private LocalizedString localizedCategoryName;
    [SerializeField, HideInInspector] private string assetGuid;
    public Sprite GetSprite() => thumbnail;
    public string GetAssetGuid() => assetGuid;
    public LocalizedString GetName() => localizedCategoryName;

    private void OnValidate() {
#if UNITY_EDITOR
        AssetDatabase.TryGetGUIDAndLocalFileIdentifier(this, out assetGuid, out long localid);
#endif
    }
}
