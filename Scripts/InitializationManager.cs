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
        
        if (trackedBehaviors[targetStage].Contains(obj)) {
            return;
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
    public static InitializationManagerInitialized.PleaseRememberToCallDoneInitialization UntrackObject(InitializationManagerInitialized obj) {
        var targetStage = obj.GetInitializationStage();
        trackedBehaviors[targetStage].Remove(obj);
        return new InitializationManagerInitialized.IWillRememberToCallDoneInitialization();
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
        if (trackedBehaviors.TryGetValue(InitializationStage.AfterMods, out var behaviors)) {
            List<InitializationManagerInitialized> copy = new List<InitializationManagerInitialized>(behaviors);
            foreach (var obj in copy) {
                try {
                    obj.OnInitialized(UntrackObject);
                } catch (Exception e) {
                    Debug.LogException(e);
                }
            }
            yield return new WaitUntil(() => FinishedLoading(InitializationStage.AfterMods));
        }
        
        currentStage = InitializationStage.AfterLevelLoad;
        if (trackedBehaviors.TryGetValue(InitializationStage.AfterLevelLoad, out var afterLevelLoadBehaviors)) {
            List<InitializationManagerInitialized> copy = new List<InitializationManagerInitialized>(afterLevelLoadBehaviors);
            foreach (var obj in copy) {
                try {
                    obj.OnInitialized(UntrackObject);
                } catch (Exception e) {
                    Debug.LogException(e);
                }
            }
            yield return new WaitUntil(() => FinishedLoading(InitializationStage.AfterLevelLoad));
        }
        currentStage = InitializationStage.FinishedLoading;
        initializeRoutine = null;
    }
}
