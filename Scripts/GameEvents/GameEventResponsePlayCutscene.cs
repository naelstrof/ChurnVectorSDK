using UnityEngine;

[System.Serializable]
public class GameEventResponsePlayCutscene : GameEventResponse {
    [SerializeField]
    private Cutscene cutscene;
    public override void Invoke(MonoBehaviour owner) {
        base.Invoke(owner);
        if (!cutscene.IsPlaying()) {
            GameManager.StaticStartCoroutine(cutscene.Begin(owner.gameObject));
        }
    }
}
