using AI.Actions;

[System.Serializable]
public class ObjectiveEvadeDetection : Objective {
    private ObjectiveStatus status;
    public override void OnRegister() {
        RadioForReinforcements.callRadio += OnAlert;
        NeedStation.escaped += OnEscape;
    }
    public override void OnUnregister() {
        RadioForReinforcements.callRadio -= OnAlert;
        NeedStation.escaped -= OnEscape;
    }
    private void OnAlert() {
        status = ObjectiveStatus.Failed;
        Fail();
    }
    private void OnEscape(CharacterBase target) {
        if (target == CharacterBase.GetPlayer() && status != ObjectiveStatus.Failed) {
            status = ObjectiveStatus.Completed;
            Complete();
        }
    }

    public override ObjectiveStatus GetStatus() {
        // Race condition sometimes makes the objective not be completed.
        if (LevelManager.IsLevelEnding() && status == ObjectiveStatus.Incomplete) {
            return ObjectiveStatus.Completed;
        }
        return status;
    }
}
