using AI;
using UnityEngine;
using DPG;
using Packages.ChurnVectorSDK.Scripts.Characters;
using static UnityEngine.UI.CanvasScaler;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Civilian))]
public class CivilianBaseEditor : CharacterBaseEditor { }
#endif

[SelectionBase]
public partial class Civilian : CharacterDetector {
    public override Actor GetActor() {
        if (inputGenerator is Actor actor) {
            return actor;
        }
        return null;
    }

    protected override void Start() {
        var dicks = GetComponentsInChildren<Penetrator>();
        foreach (Penetrator d in dicks) {
            var t = gameObject.AddComponent<PlapSource>();
            t.Init(d);
        }
        base.Start();
    }

    protected override void OnEnable() {
        base.OnEnable();
        CopOnEnable();
    }

    protected override void Update() {
        base.Update();
        GetActor()?.Update();
        //UpdateCop();
    }

    protected override void OnDisable() {
        base.OnDisable();
        CopOnDisable();
    }

}

[System.Serializable]
public class CivilianReference : ComponentReference<Civilian> {
    public CivilianReference(string guid) : base(guid) { }
}
