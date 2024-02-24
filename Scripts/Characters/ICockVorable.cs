using UnityEngine;

public interface ICockVorable {
    void OnStartCockvoreAsPrey(CharacterBase from);
    void OnCockVoreVisualsUpdateAsPrey(VoreMachine.CockVoreStatus status);
    void OnFinishedCockvoreAsPrey(CharacterBase from);
    void OnCancelledCockvoreAsPrey(CharacterBase from);
    Transform transform { get; }
}
