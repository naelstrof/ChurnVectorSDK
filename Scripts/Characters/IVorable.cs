using UnityEngine;

public interface IVorable {
    void OnStartVoreAsPrey(CharacterBase from);
    void OnVoreVisualsUpdateAsPrey(CockVoreMachine.VoreStatus status);
    void OnFinishedVoreAsPrey(CharacterBase from);
    void OnCancelledVoreAsPrey(CharacterBase from);
    Transform transform { get; }
}
