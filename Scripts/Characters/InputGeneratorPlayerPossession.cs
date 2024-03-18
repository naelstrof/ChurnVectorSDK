using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityScriptableSettings;

[System.Serializable]
public class InputGeneratorPlayerPossession : InputGenerator {
    private OrbitCameraConfiguration shoulderConfig;
    private OrbitCameraConfiguration oogleConfig;
    
    private bool toggleCamera = true;
    private Vector3 lastLook;
    private PlayerInput input;
    private float minCrouch = 0f;
    private float maxCrouch = 0.5f;
    private float toggleCrouchAmount = 0f;
    private CharacterBase character;
    private Coroutine switchShoulderRoutine;
    private CatmullSpline cachedSpline;
    
    private OrbitCameraPivotBasic headPivot;
    private OrbitCameraPivotBasic crouchPivot;

    private List<OrbitCameraPivotBase> pivots;
    
    public override void Initialize(GameObject gameObject) {
        input = GameManager.GetPlayerInput();
        input.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
        input.actions["Interact"].started += OnInteractInputStarted;
        input.actions["Interact"].canceled += OnInteractInputCancelled;
        input.actions["ToggleCamera"].performed += OnCameraPerformed;
        input.actions["CrouchToggle"].performed += OnCrouchPerformed;
        input.actions["FireWeapon"].canceled += OnFireWeapon;
        input.actions["Reload"].performed += OnReloadWeapon;
        input.actions.Enable();
        character = gameObject.GetComponent<CharacterBase>();
        if (character.voreMachine != null) {
            character.voreMachine.cockVoreStart += OnCockCockVoreStart;
            character.voreMachine.cockVoreEnd += OnCockCockVoreEnd;
        }
        pivots = new List<OrbitCameraPivotBase>();
        shoulderConfig = CreateShoulderConfig(character);
        oogleConfig = CreateOogleConfig(character);
        OrbitCamera.AddConfiguration(shoulderConfig);
    }

    private void OnReloadWeapon(InputAction.CallbackContext obj) {
        reloadWeapon?.Invoke();
    }

    private void OnFireWeapon(InputAction.CallbackContext obj) {
        fireWeapon?.Invoke();
    }

    private OrbitCameraCharacterHitmanConfiguration CreateShoulderConfig(CharacterBase character) {
        var animator = character.GetDisplayAnimator();
        
        Vector3 headLocalPos = character.transform.InverseTransformPoint(animator.GetBoneTransform(HumanBodyBones.Head).position);
        
        GameObject headPivotObj = new GameObject("HeadPivot", typeof(OrbitCameraPivotBasic));
        headPivotObj.transform.SetParent(character.transform);
        headPivotObj.transform.localPosition = headLocalPos.With(x: 0f, z: 0f);
        headPivot = headPivotObj.GetComponent<OrbitCameraPivotBasic>();
        headPivot.SetInfo(new Vector2(0.3f, 0.5f), 1.75f, 65f);
        pivots.Add(headPivot);
        
        GameObject crouchPivotObj = new GameObject("CrouchPivot", typeof(OrbitCameraPivotBasic));
        crouchPivotObj.transform.SetParent(character.transform);
        crouchPivotObj.transform.localPosition = (headLocalPos-Vector3.down).With(x: 0f, z: 0f)*0.3f + Vector3.down;
        crouchPivot = crouchPivotObj.GetComponent<OrbitCameraPivotBasic>();
        crouchPivot.SetInfo(new Vector2(0.3f, 0.12f), 1.75f, 60f);
        pivots.Add(crouchPivot);
        
        GameObject buttPivotObj = new GameObject("ButtPivot", typeof(OrbitCameraPivotBasic));
        buttPivotObj.transform.SetParent(animator.GetBoneTransform(HumanBodyBones.Hips));
        buttPivotObj.transform.localPosition = Vector3.zero;
        var buttPivot = buttPivotObj.GetComponent<OrbitCameraPivotBasic>();
        buttPivot.SetInfo(new Vector2(0.5f, 0.25f), 1.75f, 85f);
        pivots.Add(buttPivot);
        
        var config = new OrbitCameraCharacterHitmanConfiguration();
        config.SetPivots(character, headPivot, crouchPivot, buttPivot);
        return config;
    }

    private OrbitCameraCharacterOogleConfiguration CreateOogleConfig(CharacterBase character) {
        var animator = character.GetDisplayAnimator();
        Vector3 headLocalPos = character.transform.InverseTransformPoint(animator.GetBoneTransform(HumanBodyBones.Head).position);
        
        GameObject headPivotObj = new GameObject("HeadCenterPivot", typeof(OrbitCameraPivotBasic));
        headPivotObj.transform.SetParent(character.transform);
        headPivotObj.transform.localPosition = headLocalPos.With(x: 0f, z: 0f);
        var headCenterPivot = headPivotObj.GetComponent<OrbitCameraPivotBasic>();
        headCenterPivot.SetInfo(new Vector2(0.5f, 0.5f), 1.75f, 65f);
        pivots.Add(headCenterPivot);
        
        GameObject crouchPivotObj = new GameObject("CrouchCenterPivot", typeof(OrbitCameraPivotBasic));
        crouchPivotObj.transform.SetParent(character.transform);
        crouchPivotObj.transform.localPosition = headLocalPos.With(x: 0f, z: 0f)*0.5f;
        var crouchCenterPivot = crouchPivotObj.GetComponent<OrbitCameraPivotBasic>();
        crouchCenterPivot.SetInfo(new Vector2(0.5f, 0.12f), 1.75f, 60f);
        pivots.Add(crouchCenterPivot);
        
        GameObject buttCenterPivotObj = new GameObject("ButtCenterPivot", typeof(OrbitCameraPivotBasic));
        buttCenterPivotObj.transform.SetParent(animator.GetBoneTransform(HumanBodyBones.Hips));
        buttCenterPivotObj.transform.localPosition = Vector3.zero;
        var buttCenterPivot = buttCenterPivotObj.GetComponent<OrbitCameraPivotBasic>();
        buttCenterPivot.SetInfo(new Vector2(0.5f, 0.25f), 1.75f, 85f);
        pivots.Add(buttCenterPivot);
        
        GameObject buttRightPivotObj = new GameObject("ButtRightPivot", typeof(OrbitCameraPivotBasic));
        buttRightPivotObj.transform.SetParent(animator.GetBoneTransform(HumanBodyBones.Hips));
        buttRightPivotObj.transform.localPosition = Vector3.zero;
        var buttRightPivot = buttRightPivotObj.GetComponent<OrbitCameraPivotBasic>();
        buttRightPivot.SetInfo(new Vector2(0.33f, 0.33f), 2f, 85f);
        pivots.Add(buttRightPivot);
        
        GameObject buttLeftPivotObj = new GameObject("ButtRightPivot", typeof(OrbitCameraPivotBasic));
        buttLeftPivotObj.transform.SetParent(animator.GetBoneTransform(HumanBodyBones.Hips));
        buttLeftPivotObj.transform.localPosition = Vector3.zero;
        var buttLeftPivot = buttLeftPivotObj.GetComponent<OrbitCameraPivotBasic>();
        buttLeftPivot.SetInfo(new Vector2(0.66f, 0.33f), 2f, 85f);
        pivots.Add(buttLeftPivot);
        
        GameObject dickPivotObj = new GameObject("DickPivot", typeof(OrbitCameraPivotBasic));
        dickPivotObj.transform.SetParent(animator.avatarRoot);
        if (character.GetDickPenetrator() == null) {
            dickPivotObj.transform.position = animator.GetBoneTransform(HumanBodyBones.Hips).position;
        } else {
            cachedSpline ??= new CatmullSpline(new [] { Vector3.zero, Vector3.one });
            character.GetDickPenetrator().GetFinalizedSpline(ref cachedSpline, out var distanceAlongSpline, out var insertionLerp, out var penetrationArgs);
            Vector3 tipPos = cachedSpline.GetPositionFromDistance(distanceAlongSpline+character.GetDickPenetrator().GetSquashStretchedWorldLength());
            Vector3 tipDir = cachedSpline.GetVelocityFromDistance(distanceAlongSpline+character.GetDickPenetrator().GetSquashStretchedWorldLength()).normalized;
            float projectionAmount = this.character.GetDickPenetrator().GetSquashStretchedWorldLength();
            dickPivotObj.transform.position = tipPos + tipDir * projectionAmount;
        }
        var dickPivot = dickPivotObj.GetComponent<OrbitCameraPivotBasic>();
        dickPivot.SetInfo(new Vector2(0.5f, 0.4f), 2f, 75f);
        pivots.Add(dickPivot);

        var config = new OrbitCameraCharacterOogleConfiguration();
        config.SetPivots(character, headCenterPivot, crouchCenterPivot, buttCenterPivot, buttRightPivot, buttLeftPivot, dickPivot);
        return config;
    }

    private IEnumerator SetShoulder(bool left) {
        float startTime = Time.time;
        float target = left ? 0.3f : 0.7f;
        float originalScreenOffset = headPivot.GetScreenOffset().x;
        float dist = Mathf.Abs(target - originalScreenOffset)/0.4f;
        float duration = 0.25f*dist;
        while (Time.time - startTime < duration) {
            float t = (Time.time - startTime) / duration;
            headPivot.SetInfo(new Vector2(Mathf.Lerp(originalScreenOffset, target, t), 0.5f), 1.75f, 65f);
            crouchPivot.SetInfo(new Vector2(Mathf.Lerp(originalScreenOffset, target, t), 0.12f), 1.75f, 65f);
            yield return null;
        }
        headPivot.SetInfo(new Vector2(target, 0.5f), 1.75f, 65f);
        crouchPivot.SetInfo(new Vector2(target, 0.12f), 1.75f, 65f);
        switchShoulderRoutine = null;
    }

    private void OnCrouchPerformed(InputAction.CallbackContext obj) {
        if (toggleCrouchAmount == 0f) {
            toggleCrouchAmount = 1f;
            return;
        }
        toggleCrouchAmount = 0f;
    }

    private void OnCameraPerformed(InputAction.CallbackContext ctx) {
        cameraSwitch?.Invoke();
        toggleCamera = !toggleCamera;
        if (switchShoulderRoutine != null) {
            character.StopCoroutine(switchShoulderRoutine);
        }
        switchShoulderRoutine = character.StartCoroutine(SetShoulder(toggleCamera));
    }

    public override void CleanUp() {
        if (input == null) {
            return;
        }

        foreach (var pivot in pivots) {
            Object.Destroy(pivot.gameObject);
        }

        input.actions["Interact"].started -= OnInteractInputStarted;
        input.actions["Interact"].canceled -= OnInteractInputCancelled;
        input.actions["ToggleCamera"].performed -= OnCameraPerformed;
        input.actions["CrouchToggle"].performed -= OnCrouchPerformed;
        input.actions["FireWeapon"].canceled -= OnFireWeapon;
        input.actions["Reload"].performed -= OnReloadWeapon;
        if (character.voreMachine != null) {
            character.voreMachine.cockVoreStart -= OnCockCockVoreStart;
            character.voreMachine.cockVoreEnd -= OnCockCockVoreEnd;
        }
    }

    private void OnInteractInputStarted(InputAction.CallbackContext ctx) {
        if (Cutscene.CutsceneIsPlaying()) {
            return;
        }
        interactInputChanged?.Invoke(true);
    }

    private void OnInteractInputCancelled(InputAction.CallbackContext ctx) {
        if (Cutscene.CutsceneIsPlaying()) {
            return;
        }
        interactInputChanged?.Invoke(false);
    }

    public override Vector3 GetWishDirection() {
        if (Cutscene.CutsceneIsPlaying()) {
            return Vector3.zero;
        }
        Vector2 inputSpace = input.actions["Move"].ReadValue<Vector2>();
        Vector3 inputSpaceAdjusted = new Vector3(inputSpace.x, 0f, inputSpace.y);
        Vector2 screenAim = OrbitCamera.GetPlayerIntendedScreenAim();
        Quaternion rot = Quaternion.Euler(0f, screenAim.x, 0f);
        Vector3 worldSpace = rot*inputSpaceAdjusted;
        return worldSpace;
    }

    public override bool GetJumpInput() {
        if (Cutscene.CutsceneIsPlaying()) {
            return false;
        }
        return input.actions["Jump"].IsPressed();
    }
    
    public override float GetCrouchInput() {
        if (Cutscene.CutsceneIsPlaying()) {
            return 0f;
        }

        if (SettingsManager.GetSetting("CrouchToggle") is not SettingInt crouchSetting) {
            return 0f;
        }

        if (crouchSetting.GetValue() == 0) {
            float readValue = input.actions["Crouch"].ReadValue<float>();
            minCrouch = Mathf.Min(minCrouch, readValue);
            maxCrouch = Mathf.Max(maxCrouch, readValue);
            return readValue.Remap(minCrouch, maxCrouch, 0f, 1f);
        } else {
            return toggleCrouchAmount;
        }
    }

    public override bool GetSprint() {
        if (Cutscene.CutsceneIsPlaying()) {
            return false;
        }
        return input.actions["Sprint"].IsPressed();
    }

    public override Vector3 GetLookDirection() {
        if (GetWishDirection().magnitude > 0.1f || GetAimingWeapon()) {
            lastLook = OrbitCamera.GetPlayerIntendedRotation() * Vector3.forward;
        }

        return lastLook;
    }

    public override bool GetAimingWeapon() {
        return input.actions["AimWeapon"].IsPressed();
    }

    private void OnCockCockVoreStart(CockVoreMachine.VoreStatus status) {
        OrbitCamera.AddConfiguration(oogleConfig);
    }
    private void OnCockCockVoreEnd(CockVoreMachine.VoreStatus status) {
        OrbitCamera.RemoveConfiguration(oogleConfig);
    }
}
