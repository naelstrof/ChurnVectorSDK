using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SimpleJSON;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    [SerializeField] private GameEvent levelStartEvent;
    [SerializeField] private GameEvent levelEndEvent;
    [SerializeField] private GameEvent gameOverEvent;
    private List<Level> levels;
    [SerializeField] private AssetReferenceScene mainMenu;
    private bool loading = false;
    
    private static LevelManager instance;
    private double levelTimer;
    public static double GetLevelTimer() => instance.levelTimer;
    private static Coroutine levelEnding;
    
    public delegate void LevelAction();
    public static event LevelAction levelGameOver;
    public static event LevelAction levelCompleted;

    public static int GetIndexOf(Level target) {
        return instance.levels.IndexOf(target);
    }

    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }

        loading = true;
        levels = new List<Level>();
        instance = this;
        List<string> keys = new List<string> {"ChurnVectorLevel"};
        var loadHandle = Addressables.LoadAssetsAsync<Level>(keys, OnFoundLevel, Addressables.MergeMode.Union, false);
        loadHandle.Completed += (handle) => loading = false;
    }

    private void OnFoundLevel(Level level) {
        if (levels.Contains(level)) {
            return;
        }

        for (int i = 0; i < levels.Count; i++) {
            if (levels[i].GetLevelName() != level.name) continue;
            levels[i] = level;
            return;
        }
        levels.Add(level);
    }

    public static bool IsLoading() => instance.loading;

    public static bool CanPlayLevel(Level targetLevel) {
        instance.Load();
        int index = instance.levels.IndexOf(targetLevel);
        if (index == 0) {
            return true;
        }

        //instance.levels[index - 1].GetCompletionStatus(out bool completed, out List<Level.LevelObjective> objectives, out double completionTime);
        return instance.levels[index-1].GetCompleted();
    }

    private void Load() {
        if (SaveManager.GetSaveSlot() == null) {
            SaveManager.SelectSaveSlot(0);
        }
        var data = SaveManager.GetData();
        var levelData = data["levels"].Or(JSON.Parse("{}"));
        foreach (var level in levels) {
            level.Load(levelData);
        }
    }

    private void Save() {
        var data = SaveManager.GetData();
        var levelData = data["levels"].Or(JSON.Parse("{}"));
        foreach (var level in levels) {
            level.Save(levelData);
        }
        data["levels"] = levelData;
        SaveManager.Save();
    }

    private void Start() {
        if (GetCurrentLevel() != null) {
            StartLevel(GetCurrentLevel(), false);
        }
    }

    public static ICollection<Level> GetLevels() {
        return instance.levels.AsReadOnly();
    }

    public static void StartLevel(Level level, bool forceReload) {
        instance.Load();
        level.levelAwake += instance.OnLevelAwake;
        instance.StartCoroutine(level.OnStart(forceReload));
    }

    public static void TriggerGameOver() {
        if (levelEnding != null) {
            throw new UnityException("Tried to end the level while it was already ending!");
        }

        instance.gameOverEvent.Raise();
        levelGameOver?.Invoke();
        levelEnding = instance.StartCoroutine(instance.LevelEndRoutine(false));
    }
    
    public static void TriggerCompleteLevel() {
        if (levelEnding != null) {
            throw new UnityException("Tried to end the level while it was already ending!");
        }
        levelCompleted?.Invoke();
        levelEnding = instance.StartCoroutine(instance.LevelEndRoutine(true));
    }
    
    private void Update() {
        if (!Cutscene.CutsceneIsPlaying()) {
            levelTimer += Time.deltaTime;
        }
    }

    private IEnumerator LevelEndRoutine(bool returnToMainMenu) {
        levelEndEvent.Raise();
        yield return null;
        yield return new WaitUntil(() => !Cutscene.CutsceneIsPlaying());
        if (returnToMainMenu) {
            yield return SceneLoader.LoadScene(mainMenu);
            GameManager.ShowMenuStatic(GameManager.MainMenuMode.CampaignSelect);
        } else {
            QuickRestart();
        }
    }

    private void OnLevelAwake(Level self) {
        self.levelAwake -= instance.OnLevelAwake;
        levelTimer = 0f;
        Save();
        levelStartEvent.Raise();
    }
    public static bool IsLevelEnding() => levelEnding != null;
    public static Level GetCurrentLevel() {
        foreach (var level in instance.levels) {
            if (level.IsSceneLoaded()) {
                return level;
            }
        }

        var scene = SceneManager.GetActiveScene();
        if (IsLevel(scene)) {
            var newLevel = ScriptableObject.CreateInstance<Level>();
            newLevel.SetScene(SceneManager.GetActiveScene());
            instance.levels.Add(newLevel);
            Debug.LogError("Created a level instance in memory for temporary use.");
            return newLevel;
        }
        return null;
    }
    
    public static bool IsLevel(Scene scene) {
        return scene.name != "MainMenu";
    }

    public static bool InLevel() {
        return IsLevel(SceneManager.GetActiveScene());
    }

    public static void QuickRestart() {
        Assert.IsTrue(InLevel());
        StartLevel(GetCurrentLevel(), true);
    }
    private void OnValidate() {
        mainMenu.OnValidate();
    }

}
