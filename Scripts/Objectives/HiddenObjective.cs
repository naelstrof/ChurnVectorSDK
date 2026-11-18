using UnityEngine;
using UnityEngine.Localization;

[System.Serializable]
public class HiddenObjective : Objective
{
    [SerializeField]
    private GameEvent revealEvent;

    [SerializeField, SerializeReference, SubclassSelector]
    private Objective hiddenObjective;

    private ObjectiveStatus status;
    public GameEvent RevealEvent => revealEvent;

    public override ObjectiveStatus GetStatus()
    {
        return status;
    }

    public override void OnRegister()
    {
        hiddenObjective.OnRegister();
        hiddenObjective.completed += OnComplete;
        hiddenObjective.failed += OnFail;
    }

    public override void OnUnregister()
    {
        hiddenObjective.OnUnregister();
        hiddenObjective.completed -= OnComplete;
        hiddenObjective.failed -= OnFail;
    }

    public override bool IsPrimaryObjective()
    {
        return hiddenObjective.IsPrimaryObjective();
    }

    private void OnComplete()
    {
        if (status == ObjectiveStatus.Incomplete)
        {
            status = ObjectiveStatus.Completed;
            Complete();
        }
    }

    private void OnFail()
    {
        if (status == ObjectiveStatus.Incomplete)
        {
            status = ObjectiveStatus.Failed;
            Fail();
        }
    }

    public override LocalizedString GetLocalizedLabel()
    {
        return hiddenObjective.GetLocalizedLabel();
    }

    public override LocalizedString GetLocalizedDescription()
    {
        return hiddenObjective.GetLocalizedDescription();
    }
}
