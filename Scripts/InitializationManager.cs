using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        if (targetStage < currentStage) {
            obj.OnInitialized();
        } else {
            trackedBehaviors[targetStage].Add(obj);
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
        trackedBehaviors?[targetStage]?.Remove(obj);
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

    private async Task InitializeStage(InitializationStage stage) {
        List<Task> taskPool = new List<Task>();
        if (trackedBehaviors.TryGetValue(stage, out var behaviors)) {
            foreach (var obj in behaviors) {
                try {
                    taskPool.Add(obj.OnInitialized());
                } catch (Exception e) {
                    Debug.LogException(e);
                }
            }
            await Task.WhenAll(taskPool);
            trackedBehaviors[stage].Clear();
        }
    }

    private IEnumerator InitializeAllRoutine() {
        currentStage = InitializationStage.Unloaded;
        yield return new WaitUntil(() => !Modding.IsLoading());
        currentStage = InitializationStage.AfterMods;
        var afterModsTask = InitializeStage(currentStage);
        yield return new WaitUntil(() => afterModsTask.IsCompleted);
        currentStage = InitializationStage.AfterLevelLoad;
        var afterLevelLoadTask = InitializeStage(currentStage);
        yield return new WaitUntil(() => afterLevelLoadTask.IsCompleted);
        currentStage = InitializationStage.FinishedLoading;
        initializeRoutine = null;
    }
}
