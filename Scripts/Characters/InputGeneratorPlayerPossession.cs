using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityScriptableSettings;

[System.Serializable]
public class InputGeneratorPlayerPossession : InputGenerator {
    private OrbitCameraConfiguration shoulderConfig;
    private OrbitCameraConfiguration oogleConfig;
    private OrbitCameraConfiguration fpsConfig;

    private enum CameraMode {
        LeftShoulder,
        RightShoulder,
        FPS,
    }
    
    private CameraMode cameraMode = CameraMode.LeftShoulder;
    private Vector3 lastLook;
    private PlayerInput input;
    private float toggleCrouchAmount = 0f;
    private float minCrouch = 0f;
    private float maxCrouch = 0.5f;
    private CharacterBase character;
    private Coroutine switchShoulderRoutine;
    private CatmullSpline cachedSpline;
    
    private OrbitCameraPivotBasic headPivot;
    private OrbitCameraPivotBasic crouchPivot;

    private bool hasCreatedConfig = false;
    private bool oogling = false;
    private bool shouldOogle = false;
    private Vector3 neckLocalScale;

    private OrbitCameraConfiguration currentConfiguration;
    
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
        neckLocalScale = character.GetDisplayAnimator().GetBoneTransform(HumanBodyBones.Neck).localScale;
        if (character.voreMachine != null) {
            character.voreMachine.voreStart += OnCockCockVoreStart;
            character.voreMachine.voreEnd += OnCockCockVoreEnd;
        }

        OrbitCamera.configurationChanged += OnConfigurationChanged;
        
        if (!hasCreatedConfig) {
            shoulderConfig = CreateShoulderConfig(character);
            oogleConfig = CreateOogleConfig(character);
            fpsConfig = CreateFPSConfig(character);
            currentConfiguration = shoulderConfig;
            OrbitCamera.AddConfiguration(shoulderConfig);
            hasCreatedConfig = true;
        }

        if (currentConfiguration == fpsConfig) {
            character.GetDisplayAnimator().GetBoneTransform(HumanBodyBones.Neck).localScale = neckLocalScale * 0.025f;
        }
    }

    private void OnConfigurationChanged(OrbitCameraConfiguration previousconfiguration, OrbitCameraConfiguration newconfiguration) {
        if (newconfiguration == fpsConfig) {
            character.GetDisplayAnimator().GetBoneTransform(HumanBodyBones.Neck).localScale = neckLocalScale * 0.025f;
        } else {
            character.GetDisplayAnimator().GetBoneTransform(HumanBodyBones.Neck).localScale = neckLocalScale;
        }
    }

    private void OnReloadWeapon(InputAction.CallbackContext obj) {
        reloadWeapon?.Invoke();
    }

    private void OnFireWeapon(InputAction.CallbackContext obj) {
        fireWeapon?.Invoke();
    }

    private OrbitCameraConfiguration CreateFPSConfig(CharacterBase character) {
        var animator = character.GetDisplayAnimator();
        GameObject headPivotObj = new GameObject("FPSPivot", typeof(OrbitCameraPivotBasic));
        headPivotObj.transform.SetParent(animator.transform, false);
        headPivotObj.transform.position = animator.GetBoneTransform(HumanBodyBones.Head).position + animator.transform.forward * 0.2f + animator.transform.up * 0.1f;
        var fpsPivot = headPivotObj.GetComponent<OrbitCameraPivotBasic>();
        fpsPivot.SetInfo(new Vector2(0.5f, 0f), 0.01f, 65f);
        
        GameObject crouchPivotObj = new GameObject("FPSCrouchPivot", typeof(OrbitCameraPivotBasic));
        crouchPivotObj.transform.SetParent(animator.transform, false);
        crouchPivotObj.transform.position = animator.GetBoneTransform(HumanBodyBones.Head).position + animator.transform.forward * 0.5f - animator.transform.up * 0.35f;
        var fpsCrouchPivot = crouchPivotObj.GetComponent<OrbitCameraPivotBasic>();
        fpsCrouchPivot.SetInfo(new Vector2(0.5f, 0.5f), 0.01f, 65f);
        
        var config = new OrbitCameraCharacterFPSConfiguration();
        config.SetPivots(this.character, fpsPivot, fpsCrouchPivot);
        return config;
    }

    private OrbitCameraCharacterHitmanConfiguration CreateShoulderConfig(CharacterBase character) {
        var animator = character.GetDisplayAnimator();
        
        Vector3 headLocalPos = character.transform.InverseTransformPoint(animator.GetBoneTransform(HumanBodyBones.Head).position);
        
        GameObject headPivotObj = new GameObject("HeadPivot", typeof(OrbitCameraPivotBasic));
        headPivotObj.transform.SetParent(character.transform);
        headPivotObj.transform.localPosition = headLocalPos.With(x: 0f, z: 0f);
        headPivot = headPivotObj.GetComponent<OrbitCameraPivotBasic>();
        headPivot.SetInfo(new Vector2(0.3f, 0.5f), 1.75f, 65f);
        
        GameObject crouchPivotObj = new GameObject("CrouchPivot", typeof(OrbitCameraPivotBasic));
        crouchPivotObj.transform.SetParent(character.transform);
        crouchPivotObj.transform.localPosition = (headLocalPos-Vector3.down).With(x: 0f, z: 0f)*0.3f + Vector3.down;
        crouchPivot = crouchPivotObj.GetComponent<OrbitCameraPivotBasic>();
        crouchPivot.SetInfo(new Vector2(0.3f, 0.12f), 1.75f, 60f);
        
        GameObject buttPivotObj = new GameObject("ButtPivot", typeof(OrbitCameraPivotBasic));
        buttPivotObj.transform.SetParent(animator.GetBoneTransform(HumanBodyBones.Hips));
        buttPivotObj.transform.localPosition = Vector3.zero;
        var buttPivot = buttPivotObj.GetComponent<OrbitCameraPivotBasic>();
        buttPivot.SetInfo(new Vector2(0.5f, 0.25f), 1.75f, 85f);
        
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
        
        GameObject crouchPivotObj = new GameObject("CrouchCenterPivot", typeof(OrbitCameraPivotBasic));
        crouchPivotObj.transform.SetParent(character.transform);
        crouchPivotObj.transform.localPosition = headLocalPos.With(x: 0f, z: 0f)*0.5f;
        var crouchCenterPivot = crouchPivotObj.GetComponent<OrbitCameraPivotBasic>();
        crouchCenterPivot.SetInfo(new Vector2(0.5f, 0.12f), 1.75f, 60f);
        
        GameObject buttCenterPivotObj = new GameObject("ButtCenterPivot", typeof(OrbitCameraPivotBasic));
        buttCenterPivotObj.transform.SetParent(animator.GetBoneTransform(HumanBodyBones.Hips));
        buttCenterPivotObj.transform.localPosition = Vector3.zero;
        var buttCenterPivot = buttCenterPivotObj.GetComponent<OrbitCameraPivotBasic>();
        buttCenterPivot.SetInfo(new Vector2(0.5f, 0.25f), 1.75f, 85f);
        
        GameObject buttRightPivotObj = new GameObject("ButtRightPivot", typeof(OrbitCameraPivotBasic));
        buttRightPivotObj.transform.SetParent(animator.GetBoneTransform(HumanBodyBones.Hips));
        buttRightPivotObj.transform.localPosition = Vector3.zero;
        var buttRightPivot = buttRightPivotObj.GetComponent<OrbitCameraPivotBasic>();
        buttRightPivot.SetInfo(new Vector2(0.33f, 0.33f), 2f, 85f);
        
        GameObject buttLeftPivotObj = new GameObject("ButtRightPivot", typeof(OrbitCameraPivotBasic));
        buttLeftPivotObj.transform.SetParent(animator.GetBoneTransform(HumanBodyBones.Hips));
        buttLeftPivotObj.transform.localPosition = Vector3.zero;
        var buttLeftPivot = buttLeftPivotObj.GetComponent<OrbitCameraPivotBasic>();
        buttLeftPivot.SetInfo(new Vector2(0.66f, 0.33f), 2f, 85f);
        
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
        switch (cameraMode) {
            case CameraMode.LeftShoulder: cameraMode = CameraMode.RightShoulder; break;
            case CameraMode.RightShoulder: cameraMode = CameraMode.FPS; break;
            case CameraMode.FPS: cameraMode = CameraMode.LeftShoulder; break;
        }

        switch (cameraMode) {
            case CameraMode.LeftShoulder:
            case CameraMode.RightShoulder:
                if (currentConfiguration != shoulderConfig) {
                    OrbitCamera.ReplaceConfiguration(currentConfiguration, shoulderConfig);
                    currentConfiguration = shoulderConfig;
                }
                if (switchShoulderRoutine != null) {
                    character.StopCoroutine(switchShoulderRoutine);
                }
                
                if (!oogling && shouldOogle) {
                    OrbitCamera.AddConfiguration(oogleConfig);
                    oogling = true;
                }

                switchShoulderRoutine = character.StartCoroutine(SetShoulder(cameraMode == CameraMode.LeftShoulder));
                break;
            case CameraMode.FPS:
                if (currentConfiguration != fpsConfig) {
                    OrbitCamera.ReplaceConfiguration(currentConfiguration, fpsConfig);
                    currentConfiguration = fpsConfig;
                }

                if (oogling) {
                    OrbitCamera.RemoveConfiguration(oogleConfig);
                    oogling = false;
                }
                break;
        }
    }

    public override void CleanUp() {
        if (input == null) {
            return;
        }
        character.GetDisplayAnimator().GetBoneTransform(HumanBodyBones.Neck).localScale = neckLocalScale;
        OrbitCamera.configurationChanged -= OnConfigurationChanged;
        input.actions["Interact"].started -= OnInteractInputStarted;
        input.actions["Interact"].canceled -= OnInteractInputCancelled;
        input.actions["ToggleCamera"].performed -= OnCameraPerformed;
        input.actions["CrouchToggle"].performed -= OnCrouchPerformed;
        input.actions["FireWeapon"].canceled -= OnFireWeapon;
        input.actions["Reload"].performed -= OnReloadWeapon;
        if (character.voreMachine != null) {
            character.voreMachine.voreStart -= OnCockCockVoreStart;
            character.voreMachine.voreEnd -= OnCockCockVoreEnd;
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
        if (GetWishDirection().magnitude > 0.1f || GetAimingWeapon() || currentConfiguration == fpsConfig) {
            lastLook = OrbitCamera.GetPlayerIntendedRotation() * Vector3.forward;
        }

        return lastLook;
    }

    public override bool GetAimingWeapon() {
        return input.actions["AimWeapon"].IsPressed();
    }

    private void OnCockCockVoreStart(CockVoreMachine.VoreStatus status) {
        shouldOogle = true;
        if (currentConfiguration != fpsConfig) {
            oogling = true;
            OrbitCamera.AddConfiguration(oogleConfig);
        }
    }
    private void OnCockCockVoreEnd(CockVoreMachine.VoreStatus status) {
        shouldOogle = false;
        if (oogling) {
            oogling = false;
            OrbitCamera.RemoveConfiguration(oogleConfig);
        }
    }
}
