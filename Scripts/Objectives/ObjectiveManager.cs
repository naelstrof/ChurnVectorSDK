using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class ObjectiveManager : MonoBehaviour {
    [SerializeField]
    private ObjectiveDisplay objectiveDisplayPrefab;
    [SerializeField]
    private Transform displayTransform;
    [SerializeField]
    private InputActionReference showDescriptionsAction;
    [SerializeField]
    private Cutscene postGameSummaryScene;

    [SerializeField] private CanvasGroup group;

    public delegate void ObjectivesUpdatedAction(List<Objective> objectives, Objective changedObjective);

    public static ObjectivesUpdatedAction objectiveChanged;
    public static bool HasCompletedObjectives() {
        foreach (var objective in ObjectivesDescription.GetObjectives()) {
            if (objective.IsPrimaryObjective() && objective.GetStatus() != Objective.ObjectiveStatus.Completed) {
                return false;
            }
        }
        return true;
    }
    
    private Animator animator;
    //[SerializeField, SerializeReference, SerializeReferenceButton]
    //private Cutscene 
    private struct ObjectiveDisplayPack {
        public Objective.ObjectiveCompleteAction completedAction;
        public Objective.ObjectiveCompleteAction failedAction;
        public Objective objective;
        public ObjectiveDisplay objectiveDisplay;

        public void Destroy() {
            if (objectiveDisplay != null) {
                Object.Destroy(objectiveDisplay.gameObject);
            }
            objective.completed -= completedAction;
            objective.failed -= failedAction;
        }
    }

    private Dictionary<Objective, ObjectiveDisplayPack> objectiveDisplayCache;
    private static readonly int ObjectiveChanged = Animator.StringToHash("ObjectiveChanged");
    private static readonly int ShowDescriptions = Animator.StringToHash("ShowDescriptions");

    private void Awake() {
        animator = GetComponent<Animator>();
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene arg0) {
        OnDisable();
        group.alpha = 0f;
    }

    public void SetObjectives(List<Objective> objectives) {
        objectiveDisplayCache ??= new Dictionary<Objective, ObjectiveDisplayPack>();
        foreach (var pair in objectiveDisplayCache) {
            pair.Value.Destroy();
        }
        objectiveDisplayCache.Clear();
        if (objectives == null) {
            return;
        }
        foreach (var objective in objectives) {
            var displayObj = Instantiate(objectiveDisplayPrefab.gameObject, displayTransform);
            var display = displayObj.GetComponent<ObjectiveDisplay>();
            display.SetLabelAndDescription(objective.GetLocalizedLabel(), objective.GetLocalizedDescription());
            Objective.ObjectiveCompleteAction completeAction = () => {
                display.SetCompleted(true);
                animator.SetTrigger(ObjectiveChanged);
                objectiveChanged?.Invoke(objectives, objective);
            };
            Objective.ObjectiveCompleteAction failedAction = () => {
                display.SetFailed(true);
                animator.SetTrigger(ObjectiveChanged);
                objectiveChanged?.Invoke(objectives, objective);
            };
            objective.completed += completeAction;
            objective.failed += failedAction;
            objectiveDisplayCache.Add(objective, new ObjectiveDisplayPack {
                completedAction = completeAction,
                failedAction = failedAction,
                objective = objective,
                objectiveDisplay = display,
            });
            if (objective.GetStatus() == Objective.ObjectiveStatus.Completed) {
                display.SetCompleted(true);
                objectiveChanged?.Invoke(objectives, objective);
            } else if (objective.GetStatus() == Objective.ObjectiveStatus.Failed) {
                display.SetFailed(true);
                objectiveChanged?.Invoke(objectives, objective);
            }
        }

    }

    private void OnEnable() {
        SetObjectives(ObjectivesDescription.GetObjectives());
        showDescriptionsAction.action.started += OnShowDescriptions;
        showDescriptionsAction.action.canceled += OnHideDescriptions;
        LevelManager.levelCompleted += OnLevelEnded;
    }

    private void OnDisable() {
        SetObjectives(null);
        showDescriptionsAction.action.started -= OnShowDescriptions;
        showDescriptionsAction.action.canceled -= OnHideDescriptions;
        LevelManager.levelCompleted -= OnLevelEnded;
    }

    private void OnLevelEnded() {
        StartCoroutine(postGameSummaryScene.Begin(gameObject));
    }

    private void OnShowDescriptions(InputAction.CallbackContext callbackContext) {
        foreach (var pair in objectiveDisplayCache) {
            pair.Value.objectiveDisplay.SetShowDescription(true);
        }

        animator.SetBool(ShowDescriptions, true);
    }

    private void OnHideDescriptions(InputAction.CallbackContext callbackContext) {
        foreach (var pair in objectiveDisplayCache) {
            pair.Value.objectiveDisplay.SetShowDescription(false);
        }
        animator.SetBool(ShowDescriptions, false);
    }

    private void OnDestroy() {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
}
