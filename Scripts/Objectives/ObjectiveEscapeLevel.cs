
[System.Serializable]
public class ObjectiveEscapeLevel : Objective {
    private ObjectiveStatus status;
    public override void OnRegister() {
        NeedStation.escaped += OnEscape;
    }

    public override void OnUnregister() {
        NeedStation.escaped -= OnEscape;
    }

    private void OnEscape(CharacterBase target) {
        status = ObjectiveStatus.Completed;
        Complete();
        LevelManager.TriggerCompleteLevel();
    }
    public override ObjectiveStatus GetStatus() => status;
}
