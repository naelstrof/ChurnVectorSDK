using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ActionHint : KeepOnScreen {
    [SerializeField] private Image targetDisplay;
    [SerializeField] private InputActionReference action;
    [SerializeField] private ActionHintDatabase database;
    private void OnEnable() {
        ActionHintDatabase.spritesChanged += OnSpritesChanged;
        action.action.Enable();
        OnSpritesChanged();
    }

    private void OnDisable() {
        ActionHintDatabase.spritesChanged -= OnSpritesChanged;
    }

    protected override void LateUpdate() {
        base.LateUpdate();
        targetDisplay.color = targetDisplay.color.With(a: alpha);
    }

    void OnSpritesChanged() {
        if (database.TryGetSprite(action, out Sprite newSprite)) {
            targetDisplay.sprite = newSprite;
        }
    }
}
