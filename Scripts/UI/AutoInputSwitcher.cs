using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using UnityScriptableSettings;

public class AutoInputSwitcher : MonoBehaviour {
    private static AutoInputSwitcher instance;
    private PlayerInput input;
    [SerializeField]
    private SettingInt glyphSetting;
    [SerializeField]
    private ActionHintDatabase actionHintDatabase;
    public enum ControlType {
        KeyboardMouse,
        Controller,
    }

    private ControlType controlType;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public static ControlType GetControlType() {
        return instance.controlType;
    }

    private void Start() {
        input = GetComponent<PlayerInput>();
        glyphSetting.changed += OnGlyphSettingChanged;
        OnGlyphSettingChanged(glyphSetting.GetValue());
        InputSystem.onDeviceChange += OnDeviceChanged;
        // For SteamDeck support, we must immediately show steam deck controls if we got a valid gamepad.
        if (Gamepad.current != null) {
            TrySetGlyphType(ActionHintDatabase.GlyphType.Xbox);
        }
    }

    private void OnDeviceChanged(InputDevice device, InputDeviceChange change) {
        switch (change) {
            case InputDeviceChange.Added:
            case InputDeviceChange.Removed:
                if (input != null) {
                    List<InputDevice> newDevices = new List<InputDevice>(InputSystem.devices);
                    input.actions.devices = newDevices.ToArray();
                }
                break;
        }
    }

    private void OnGlyphSettingChanged(int value) {
        switch (value) {
            case 1:
                actionHintDatabase.SetGlyphType(ActionHintDatabase.GlyphType.Xbox);
                break;
            case 2:
                actionHintDatabase.SetGlyphType(ActionHintDatabase.GlyphType.DualShock);
                break;
            case 3:
                actionHintDatabase.SetGlyphType(ActionHintDatabase.GlyphType.Keyboard);
                break;
        }
    }

    private void TrySetGlyphType(ActionHintDatabase.GlyphType type) {
        switch (type) {
            case ActionHintDatabase.GlyphType.Xbox:
            case ActionHintDatabase.GlyphType.DualShock:
                controlType = ControlType.Controller;
                break;
            case ActionHintDatabase.GlyphType.Keyboard:
                controlType = ControlType.KeyboardMouse;
                break;
        }

        if (glyphSetting.GetValue() != 0) {
            return;
        }
        actionHintDatabase.SetGlyphType(type);
    }

    private void Update() {
        foreach (var device in InputSystem.devices) {
            switch (device) {
                case Mouse mouse when mouse.leftButton.IsPressed() && (actionHintDatabase.GetGlyphType() != ActionHintDatabase.GlyphType.Keyboard || controlType != ControlType.KeyboardMouse): {
                    TrySetGlyphType(ActionHintDatabase.GlyphType.Keyboard);
                    return;
                }
                case Keyboard keyboard when keyboard.wKey.IsPressed() && (actionHintDatabase.GetGlyphType() != ActionHintDatabase.GlyphType.Keyboard || controlType != ControlType.KeyboardMouse): {
                    TrySetGlyphType(ActionHintDatabase.GlyphType.Keyboard);
                    return;
                }
                case XInputController xbox when (actionHintDatabase.GetGlyphType() != ActionHintDatabase.GlyphType.Xbox || controlType != ControlType.Controller): {
                    foreach(var control in xbox.allControls) {
                        if (control is not ButtonControl buttonControl) {
                            continue;
                        }
                        if (!buttonControl.IsPressed() || buttonControl.synthetic) continue;
                        TrySetGlyphType(ActionHintDatabase.GlyphType.Xbox);
                        return;
                    }
                    continue;
                }
                case DualShockGamepad dualShock when (actionHintDatabase.GetGlyphType() != ActionHintDatabase.GlyphType.DualShock || controlType != ControlType.Controller): {
                    foreach(var control in dualShock.allControls) {
                        if (control is not ButtonControl buttonControl) {
                            continue;
                        }
                        if (!buttonControl.IsPressed() || buttonControl.synthetic) continue;
                        TrySetGlyphType(ActionHintDatabase.GlyphType.DualShock);
                        return;
                    }
                    continue;
                }
                case Gamepad pad when (actionHintDatabase.GetGlyphType() == ActionHintDatabase.GlyphType.Keyboard || controlType == ControlType.KeyboardMouse): {
                    foreach(var control in pad.allControls) {
                        if (control is not ButtonControl buttonControl) {
                            continue;
                        }

                        if (!buttonControl.IsPressed() || buttonControl.synthetic) continue;
                        
                        if (pad.description.product.ToLowerInvariant().Contains("dualshock")) {
                            TrySetGlyphType(ActionHintDatabase.GlyphType.DualShock);
                        } else {
                            TrySetGlyphType(ActionHintDatabase.GlyphType.Xbox);
                        }
                        return;
                    }
                    continue;
                }
            }
        }
    }
}
