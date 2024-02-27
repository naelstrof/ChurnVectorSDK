using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class InitializationManagerInitialized : MonoBehaviour {
    public abstract InitializationManager.InitializationStage GetInitializationStage();

    public virtual Task OnInitialized() {
        return Task.CompletedTask;
    }
    protected virtual void OnEnable() {
        InitializationManager.TrackObject(this);
    }

    protected virtual void OnDestroy() {
        InitializationManager.UntrackObject(this);
    }
}
