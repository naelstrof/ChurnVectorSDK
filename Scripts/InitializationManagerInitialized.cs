using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InitializationManagerInitialized : MonoBehaviour {
    public abstract InitializationManager.InitializationStage GetInitializationStage();

    public delegate void DoneInitializingAction(InitializationManagerInitialized obj);

    public abstract void OnInitialized(DoneInitializingAction doneInitializingAction);

    protected virtual void OnEnable() {
        InitializationManager.TrackObject(this);
    }

    protected virtual void OnDestroy() {
        InitializationManager.UntrackObject(this);
    }
}
