using UnityEngine;
using UnityEngine.Localization;

[System.Serializable]
public abstract class Objective {
    public delegate void ObjectiveCompleteAction();
    public event ObjectiveCompleteAction completed;
    public event ObjectiveCompleteAction failed;

    public enum ObjectiveStatus {
        Incomplete,
        Failed,
        Completed,
    }
    
    [SerializeField]
    private LocalizedString objectiveLabelLocalizedString;
    [SerializeField]
    private LocalizedString objectiveDescriptionLocalizedString;

    [SerializeField] private bool primaryObjective;
    public virtual LocalizedString GetLocalizedLabel() => objectiveLabelLocalizedString;
    public virtual LocalizedString GetLocalizedDescription() => objectiveDescriptionLocalizedString;
    public bool IsPrimaryObjective() => primaryObjective;
    
    public abstract void OnRegister();
    public abstract void OnUnregister();
    public abstract ObjectiveStatus GetStatus();

    protected void Complete() {
        completed?.Invoke();
    }

    protected void Fail() {
        failed?.Invoke();
    }
}
