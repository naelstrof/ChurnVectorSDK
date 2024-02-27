using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InitializationManagerInitialized : MonoBehaviour {
    public abstract InitializationManager.InitializationStage GetInitializationStage();

    public class PleaseRememberToCallDoneInitialization {
        protected PleaseRememberToCallDoneInitialization() { }
    }

    public class IWillRememberToCallDoneInitialization : PleaseRememberToCallDoneInitialization {
        public IWillRememberToCallDoneInitialization() { }
    }

    public delegate PleaseRememberToCallDoneInitialization DoneInitializingAction(InitializationManagerInitialized obj);

    public abstract PleaseRememberToCallDoneInitialization OnInitialized(DoneInitializingAction doneInitializingAction);

    protected virtual void OnEnable() {
        InitializationManager.TrackObject(this);
    }

    protected virtual void OnDestroy() {
        InitializationManager.UntrackObject(this);
    }
}
