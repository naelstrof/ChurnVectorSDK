using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenObjective : Objective
{
    [SerializeField]
    private Objective objective;

    private ObjectiveStatus status;

    public override ObjectiveStatus GetStatus()
    {
        return status;
    }

    public override void OnRegister()
    {
        objective.OnRegister();
    }

    public override void OnUnregister()
    {
        objective.OnUnregister();
    }
}
