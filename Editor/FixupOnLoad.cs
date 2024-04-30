using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public static class FixupOnLoad {
    //Doesn't trigger on import, of course. Why would it??
    [InitializeOnLoadMethod]
    private static void OnLoad() {
        var localizationSettings = AssetDatabase.LoadAssetAtPath<LocalizationSettings>(AssetDatabase.GUIDToAssetPath("c281d4ec57da5fd4ebe03d7b7fd9a9a3"));
        if (UnityEditor.Localization.LocalizationEditorSettings.ActiveLocalizationSettings != localizationSettings) {
            UnityEditor.Localization.LocalizationEditorSettings.ActiveLocalizationSettings = localizationSettings;
        }
        var hdrpGraphicsPipelineAsset = AssetDatabase.LoadAssetAtPath<HDRenderPipelineAsset>(AssetDatabase.GUIDToAssetPath("0ae905cf973c7d740b0afccc16300884"));
        var overrideGraphicsPipelineAsset = AssetDatabase.LoadAssetAtPath<HDRenderPipelineAsset>(AssetDatabase.GUIDToAssetPath("4f5ebb1b57178a241a071e9ecb8b7857"));
        var graphicsSettings = AssetDatabase.LoadAssetAtPath<RenderPipelineGlobalSettings>(AssetDatabase.GUIDToAssetPath("bc58a23133f32d3419b72fe3fb2b75aa"));
        if (GraphicsSettings.defaultRenderPipeline != hdrpGraphicsPipelineAsset || QualitySettings.renderPipeline != overrideGraphicsPipelineAsset || GraphicsSettings.GetSettingsForRenderPipeline<HDRenderPipeline>() != graphicsSettings) {
            GraphicsSettings.defaultRenderPipeline = hdrpGraphicsPipelineAsset;
            QualitySettings.renderPipeline = overrideGraphicsPipelineAsset;
            GraphicsSettings.RegisterRenderPipelineSettings<HDRenderPipeline>(graphicsSettings);
        }
        
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings != null) {
            Undo.RecordObject(settings, "Ensuring settings are set up correctly.");
            settings.BuildRemoteCatalog = true;
            settings.UniqueBundleIds = false;
            settings.ShaderBundleNaming = ShaderBundleNaming.Custom;
            settings.ShaderBundleCustomNaming = "ChurnVector";
            settings.MonoScriptBundleNaming = MonoScriptBundleNaming.Disabled;
            EditorUtility.SetDirty(settings);
        }
    }
}
