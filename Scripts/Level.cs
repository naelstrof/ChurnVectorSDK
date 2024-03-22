using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimpleJSON;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New Level", menuName = "Data/Level", order = 56)]
public class Level : ScriptableObject {
    [SerializeField] private LocalizedString localizedLevelName;
    [SerializeField] private LocalizedString localizedLevelDescription;
    [SerializeField] private Sprite levelPreview;
    [SerializeField] private AssetReferenceScene scene;
    [SerializeField] private AssetReferenceLevelCategory category;
    [SerializeField] private int levelOrderPriority;
    [SerializeField] private bool requiresPreviousLevelToBeBeatenToPlay = false;
    
    [Serializable]
    private class AssetReferenceLevelCategory : AssetReferenceT<LevelCategory> {
        public AssetReferenceLevelCategory(string guid) : base(guid) { }
    }
    private Scene? sceneDirect;

    public bool GetRequiresPreviousLevelToBeBeatenToPlay() {
        return requiresPreviousLevelToBeBeatenToPlay;
    }

    public int CompareTo(Level b) {
        return levelOrderPriority.CompareTo(b.levelOrderPriority);
    }
    public bool IsPartOfCategory(LevelCategory categoryToCheck) {
        return category.AssetGUID == categoryToCheck.GetAssetGuid();
    }
    public bool IsPartOfCategory(Level levelToCompareTo) {
        return category.AssetGUID == levelToCompareTo.category.AssetGUID;
    }
    public void SetScene(Scene newScene) {
        sceneDirect = newScene;
    }
    
    public delegate void LevelAction(Level self);
    public event LevelAction levelAwake;
    
    private List<LevelObjective> objectives = new();
    public struct LevelObjective {
        public LocalizedString name;
        public Objective.ObjectiveStatus status;
    }

    public void GetCompletionStatus(out bool completed, out List<LevelObjective> objectives, out double completionTime) {
        completed = this.completed;
        objectives = this.objectives;
        completionTime = this.completionTime;
    }

    public bool SetCompletionStatus(bool newCompleted, List<Objective> objectives, double newCompletionTime) {
        List<LevelObjective> newObjectives = new List<LevelObjective>();
        foreach (var objective in objectives) {
            newObjectives.Add(new LevelObjective {
                name = objective.GetLocalizedLabel(),
                status = objective.GetStatus(),
            });
        }
        var oldCount = this.objectives.Count((obj)=>obj.status==Objective.ObjectiveStatus.Completed);
        var newCount = newObjectives.Count((obj)=>obj.status==Objective.ObjectiveStatus.Completed);

        if (completed && !newCompleted) {
            return false;
        }
        
        if (completed && oldCount > newCount) {
            return false;
        }

        if (completed && oldCount == newCount && completionTime < newCompletionTime) {
            return false;
        }

        // New high score!
        completionTime = newCompletionTime;
        this.objectives = newObjectives;
        completed = newCompleted;
        return true;
    }
    private double completionTime { get; set; }
    private bool completed { get; set; }
    public string GetLevelName() => localizedLevelName?.GetLocalizedString() ?? GetSceneName();
    public LocalizedString GetLocalizedLevelName() => localizedLevelName;
    public string GetLevelDescription() => localizedLevelDescription.GetLocalizedString();
    public LocalizedString GetLocalizedLevelDescription() => localizedLevelDescription;
    public Sprite GetLevelPreview() => levelPreview;

    private string GetSceneName() => sceneDirect?.name ?? scene.GetName();  

    public bool IsSceneLoaded() {
        return GetSceneName() == SceneManager.GetActiveScene().name;
    }

    public void Save(JSONNode rootNode) {
        JSONNode node = rootNode[name].Or(JSONNode.Parse("{}"));
        node[nameof(completed)] = completed;
        node[nameof(completionTime)] = completionTime;
        JSONArray objectiveArray = new JSONArray();
        foreach (var obj in objectives) {
            JSONNode objectiveNode = JSONNode.Parse("{}");
            objectiveNode["TableReference"] = obj.name.TableReference.TableCollectionNameGuid.ToString();
            objectiveNode["TableEntryReference"] = obj.name.TableEntryReference.KeyId;
            objectiveNode["status"] = obj.status.ToString();
            objectiveArray.Add(objectiveNode);
        }
        node[nameof(objectives)] = objectiveArray;
        rootNode[name] = node;
    }

    public bool GetCompleted() {
        return completed;
    }

    public void Load(JSONNode rootNode) {
        JSONNode node = rootNode[name].Or(JSONNode.Parse("{}"));
        completed = node[nameof(completed)];
        completionTime = node[nameof(completionTime)];
        JSONArray objectiveArray = node[nameof(objectives)].AsArray;
        objectives.Clear();
        foreach (var objNode in objectiveArray) {
            if (!Enum.TryParse(objNode.Value["status"], out Objective.ObjectiveStatus status)) {
                status = Objective.ObjectiveStatus.Failed;
            }
            objectives.Add(new LevelObjective {
                name = new LocalizedString(new Guid(objNode.Value["TableReference"].ToString().Trim('"')), objNode.Value["TableEntryReference"].AsLong),
                status = status,
            });
        }
    }

    public IEnumerator OnStart(bool forceReload) {
        Pauser.ForcePause(true);
        if (scene != null && (forceReload || !IsSceneLoaded())) {
            yield return SceneLoader.LoadScene(scene);
        } else if (forceReload && IsSceneLoaded()) {
#if UNITY_EDITOR
            var handle = EditorSceneManager.LoadSceneAsyncInPlayMode(SceneManager.GetActiveScene().path, new LoadSceneParameters() { loadSceneMode = LoadSceneMode.Single, localPhysicsMode = LocalPhysicsMode.Physics3D });
            yield return new WaitUntil(() => handle.isDone);
#endif
        } else if (!IsSceneLoaded()) {
            Debug.Log(IsSceneLoaded());
            throw new UnityException("Can't load arbitrary unaddressable scenes. Please configure them properly!");
        }
        SceneManager.sceneUnloaded += OnEnd;
        InitializationManager.InitializeAll();
        var cameraSetupRoutine = SetupOrbitCamera();
        var objectiveSetupRoutine = SetupObjectives();
        yield return cameraSetupRoutine;
        yield return objectiveSetupRoutine;
        yield return new WaitUntil(()=>InitializationManager.GetCurrentStage() == InitializationManager.InitializationStage.FinishedLoading);
        
        if (FindObjectsOfType<CharacterLoader>(true).Length == 0) {
            Debug.LogError("No character loaders found, place them to spawn the player and NPCs!");
        }

        // Quickly do a save to write out blank objectives.
        SetCompletionStatus(false, ObjectivesDescription.GetObjectives(), 0f);
        
        // We're finally playing!
        Pauser.ForcePause(false);
        levelAwake?.Invoke(this);
    }
    
    private void OnEnd(Scene arg0) {
        SceneManager.sceneUnloaded -= OnEnd;
    }
    
    private static IEnumerator SetupOrbitCamera() {
        if (OrbitCamera.GetCamera() == null) {
            var handle = Addressables.InstantiateAsync("ed124e2780a80b14d8c3442fbb7349d7"); // OrbitCamera GUID
            yield return handle;
            var pivot = new GameObject("Default pivot", typeof(OrbitCameraPivotBasic));
            var basicConfig = new OrbitCameraBasicConfiguration();
            basicConfig.SetPivot(pivot.GetComponent<OrbitCameraPivotBasic>());
            OrbitCamera.AddConfiguration(basicConfig);
        }

        var orbitCam = OrbitCamera.GetCamera();
        foreach (var camera in FindObjectsOfType<Camera>()) {
            if (camera == orbitCam) {
                continue;
            }

            if (camera.targetTexture == null) {
                Debug.LogError("Found floating camera " + camera + " attempting to render to the screen. This will cause performance problems in HDRP and will be disabled. Consider deleting it.", camera.gameObject);
                camera.gameObject.SetActive(false);
            }
        }
    }

    private static IEnumerator SetupObjectives() {
        if (FindObjectOfType<ObjectivesDescription>() != null) yield break;
        var handle = Addressables.InstantiateAsync("f0a78b55e6b12b84ea06b8da9c12d7f2");
        yield return handle;
        Debug.LogError( "No objectives found in level, spawned a default set. Consider creating an ObjectivesDescription to track objectives in this level.");
    }

#if UNITY_EDITOR
    public void OnValidate() {
        scene.OnValidate();
    }
#endif
}
