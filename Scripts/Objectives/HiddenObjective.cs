using UnityEngine;

[System.Serializable]
public class HiddenObjective : Objective
{
    [SerializeField, SerializeReference, SubclassSelector]
    private Objective objective;

    private ObjectiveStatus status;

    public override ObjectiveStatus GetStatus()
    {
        return status;
    }

    public override void OnRegister()
    {
        objective.OnRegister();
        objective.completed += OnComplete;
        objective.failed += OnFail;
    }

    public override void OnUnregister()
    {
        objective.OnUnregister();
        objective.completed -= OnComplete;
        objective.failed -= OnFail;
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
}
