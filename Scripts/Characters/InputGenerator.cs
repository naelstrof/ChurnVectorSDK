using UnityEngine;

[System.Serializable]
public abstract class InputGenerator {
    public delegate void Vector3InputChangedAction(Vector3 newWishDirection);
    public delegate void BoolInputChangedAction(bool newPress);
    public delegate void InputTriggeredAction();

    public BoolInputChangedAction interactInputChanged;
    
    public InputTriggeredAction cameraSwitch;
    
    public InputTriggeredAction fireWeapon;
    public InputTriggeredAction reloadWeapon;

    public abstract void Initialize(GameObject gameObject);

    public abstract void CleanUp();

    public abstract Vector3 GetWishDirection();
    public abstract bool GetJumpInput();
    public abstract float GetCrouchInput();
    public abstract Vector3 GetLookDirection();
    public abstract bool GetSprint();
    public abstract bool GetAimingWeapon();
}
