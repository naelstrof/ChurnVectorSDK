using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializationManager : MonoBehaviour {
    private static InitializationManager instance;
    private static Dictionary<InitializationStage,List<InitializationManagerInitialized>> trackedBehaviors = new();
    private static InitializationStage currentStage = InitializationStage.Unloaded;
    private static Coroutine initializeRoutine = null;
    public static InitializationStage GetCurrentStage() => currentStage;
    public enum InitializationStage {
        Unloaded,
        AfterMods,
        AfterLevelLoad,
        FinishedLoading,
    }
    
    public static void TrackObject(InitializationManagerInitialized obj) {
        var targetStage = obj.GetInitializationStage();
        if (!trackedBehaviors.ContainsKey(targetStage)) {
            trackedBehaviors.Add(targetStage, new List<InitializationManagerInitialized>());
        }
        
        trackedBehaviors[targetStage].Add(obj);
        if (targetStage < currentStage) {
            obj.OnInitialized(UntrackObject);
        }
    }

    private static bool FinishedLoading(InitializationStage targetStage) {
        if (!trackedBehaviors.ContainsKey(targetStage)) {
            return true;
        }
        return trackedBehaviors[targetStage].Count == 0;
    }
    public static void UntrackObject(InitializationManagerInitialized obj) {
        var targetStage = obj.GetInitializationStage();
        trackedBehaviors[targetStage].Remove(obj);
    }
    private void Awake() {
        if (instance == null) {
            instance = this;
        }

        SceneManager.sceneUnloaded += OnSceneUnload;
    }

    private void OnSceneUnload(Scene arg0) {
        trackedBehaviors.Clear();
        currentStage = InitializationStage.Unloaded;
    }

    public static void InitializeAll() {
        if (initializeRoutine == null) {
            instance.StartCoroutine(instance.InitializeAllRoutine());
        }
    }

    private IEnumerator InitializeAllRoutine() {
        currentStage = InitializationStage.Unloaded;
        yield return new WaitUntil(() => !Modding.IsLoading());
        currentStage = InitializationStage.AfterMods;
        if (trackedBehaviors.ContainsKey(InitializationStage.AfterMods)) {
            foreach (var obj in trackedBehaviors[InitializationStage.AfterMods]) {
                obj.OnInitialized(UntrackObject);
            }
            yield return new WaitUntil(() => FinishedLoading(InitializationStage.AfterMods));
        }
        currentStage = InitializationStage.AfterLevelLoad;
        if (trackedBehaviors.ContainsKey(InitializationStage.AfterLevelLoad)) {
            foreach (var obj in trackedBehaviors[InitializationStage.AfterLevelLoad]) {
                obj.OnInitialized(UntrackObject);
            }
            yield return new WaitUntil(() => FinishedLoading(InitializationStage.AfterLevelLoad));
        }
        currentStage = InitializationStage.FinishedLoading;
        initializeRoutine = null;
    }
}
