using UnityEngine;

[System.Serializable]
public class GameEventResponsePlayShowMenu : GameEventResponse {
    [SerializeField] private GameManager.MainMenuMode menu;
    public override void Invoke(MonoBehaviour owner) {
        base.Invoke(owner);
        GameManager.ShowMenuStatic(menu);
    }
}
