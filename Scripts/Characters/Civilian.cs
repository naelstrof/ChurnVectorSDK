using AI;
using UnityEngine;

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
