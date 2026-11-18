using UnityEngine;

[System.Serializable]
public class ObjectiveTriggerGameEvent : Objective
{
    [SerializeField]
    private GameEvent successEvent;
    [SerializeField]
    private GameEvent failureEvent;
    private ObjectiveStatus status;

    public override void OnRegister()
    {
        successEvent.triggered += OnSuccess;

        if (failureEvent)
            failureEvent.triggered += OnFailure;
    }
    public override void OnUnregister()
    {
        successEvent.triggered -= OnSuccess;

        if (failureEvent)
            failureEvent.triggered -= OnFailure;
    }

    private void OnFailure()
    {
        if (status != ObjectiveStatus.Completed)
        {
            status = ObjectiveStatus.Failed;
            Fail();
        }
    }

    private void OnSuccess()
    {
        if(!failureEvent || status != ObjectiveStatus.Failed)
        {
            status = ObjectiveStatus.Completed;
            Complete();
        }
    }

    public override ObjectiveStatus GetStatus()
    {
        return status;
    }
}
