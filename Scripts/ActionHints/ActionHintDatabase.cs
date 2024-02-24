using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[CreateAssetMenu(fileName = "New Action Hint Database", menuName = "Data/Action Hints/Database", order = 36)]
public class ActionHintDatabase : ScriptableObject {
    public delegate void InputSpriteChangedAction();
    public static event InputSpriteChangedAction spritesChanged;
    private GlyphType type;

    public enum GlyphType {
        Keyboard,
        Xbox,
        DualShock,
    }

    public GlyphType GetGlyphType() {
        return type;
    }

    public void SetGlyphType(GlyphType type) {
        this.type = type;
        OnControlsChanged(type);
    }

    [Serializable]
    private class ActionData {
        [Serializable]
        private class DeviceSprite {
            [SerializeField] private GlyphType glyphType;
            [SerializeField] private Sprite sprite;
            public Sprite GetSprite() => sprite;

            public bool GetMatched(GlyphType type) {
                return type == glyphType;
            }
        }
        [SerializeField] private InputActionReference action;
        [SerializeField] private List<DeviceSprite> sprites;

        public bool Matches(InputActionReference action) {
            return this.action == action;
        }

        public Sprite GetSprite(GlyphType type) {
            foreach (var deviceSprite in sprites) {
                if (deviceSprite.GetMatched(type)) {
                    return deviceSprite.GetSprite();
                }
            }
            return null;
        }
    }
    [SerializeField] private List<ActionData> database;

    public bool TryGetSprite(InputActionReference action, out Sprite outSprite) {
        foreach (var data in database) {
            if (!data.Matches(action)) continue;
            outSprite = data.GetSprite(type);
            if (outSprite != null) {
                return true;
            }
        }
        outSprite = null;
        return false;
    }
    
    static void OnControlsChanged(GlyphType newType) {
        spritesChanged?.Invoke();
    }
}
