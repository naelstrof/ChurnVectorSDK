using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class VoreContainer {
    public abstract void LateUpdate();
    public abstract Transform GetStorageTransform();
    public abstract CumStorage GetStorage();
    public abstract void AddChurnable(IChurnable churnable);
    public abstract void Initialize(CharacterBase character);

    public virtual void OnJump(float velocity) {
    }
}
