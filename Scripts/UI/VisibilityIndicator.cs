using System;
using UnityEngine;

public class VisibilityIndicator : MonoBehaviour {
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private UnityEngine.UI.Image image;
    
    private bool hasPlayer = false;
    private CharacterDetector player;
    private void OnEnable() {
        CharacterBase.playerEnabled += OnPlayerEnabled;
        if (CharacterBase.GetPlayer() != null && CharacterBase.GetPlayer().enabled) {
            OnPlayerEnabled(CharacterBase.GetPlayer());
        }
    }

    private void OnDisable() {
        CharacterBase.playerDisabled += OnPlayerDisabled;
    }

    private void OnPlayerEnabled(CharacterBase player) {
        hasPlayer = true;
        this.player = player as CharacterDetector;
    }

    private void OnPlayerDisabled(CharacterBase player) {
        this.player = null;
    }

    private void Update() {
        if (!hasPlayer) {
            return;
        }

        float visibility = player.GetNoticability()/player.GetMaxNoticability();
        image.sprite = sprites[Mathf.Clamp(Mathf.FloorToInt(sprites.Length*visibility),0, sprites.Length-1)];
    }
}
