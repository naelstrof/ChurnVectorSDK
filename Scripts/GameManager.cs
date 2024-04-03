using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.VFX;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
#endif

public class GameManager : MonoBehaviour {
    private static GameManager instance;
    
    [SerializeField]
    private InputActionReference jumpButton;
    
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject creditsMenu;
    [SerializeField]
    private GameObject optionsMenu;
    [SerializeField]
    private GameObject campaignMenu;
    [SerializeField]
    private GameObject saveSelectMenu;
    [SerializeField] private GameObject orbitCameraPrefab;
    [SerializeField] private GameObject defaultObjectivesPrefab;

    public enum MainMenuMode {
        Pause,
        Options,
        CampaignSelect,
        Credits,
        SaveSelect,
    }

    public void ShowMenu(MainMenuMode mode) {
        pauseMenu.SetActive(false);
        creditsMenu.SetActive(false);
        optionsMenu.SetActive(false);
        campaignMenu.SetActive(false);
        saveSelectMenu.SetActive(false);
        switch (mode) {
            case MainMenuMode.Pause: pauseMenu.SetActive(true); break;
            case MainMenuMode.Credits: creditsMenu.SetActive(true); break;
            case MainMenuMode.Options: optionsMenu.SetActive(true); break;
            case MainMenuMode.CampaignSelect: campaignMenu.SetActive(true); break;
            case MainMenuMode.SaveSelect: saveSelectMenu.SetActive(true); break;
        }
    }


    public static void ShowMenuStatic(MainMenuMode mode) {
        instance.ShowMenu(mode);
    }

    public static bool IsReady() => instance != null;
    [Serializable]
    public class SharedLibrary {
        public VisualEffectAsset audioVisualizer;
        public VisualEffectAsset bigSplash;
        public VisualEffectAsset lightning;
        public Projectile taserProjectile;
        public AudioPack taserShot;
        public AudioPack taserZapping;
        public PhysicMaterial ballsMaterial;
        public AudioPack landingPack;
        public AudioPack churnPack;
        public AudioPack yowl;
        public AudioPack tummyGurglesPack;
        public InflatableCurve cockVorePath;
        public AudioPack slippySlidePack;
        public AudioPack glorpPack;
        public GameObject interactVisualizationPrefab;
        public Material cumProjector;
        public ParticleSystem cumPrefab;
        public PhysicMaterial spaceLube;
        public GameObject detectorDisplayDisks;
        public InflatableCurve bulgeCurve;
        public InflatableCurve tipOpenCurve;
    }
    public SharedLibrary library;

    public static SharedLibrary GetLibrary() => instance.library;
    
    public GameEvent playerGotCockvoredEvent;
    [SerializeField] private InputActionAsset actions;
    [SerializeField] private PhysicsMaterialExtensionDatabase physicsMaterialExtensionDatabase;

#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnInitialize() {
        if (SceneManager.GetActiveScene().name == "StartScene") return;
        
        foreach(var check in FindObjectsOfType<GameManager>(true)) {
            Debug.LogError("Please don't include the game manager in your scene, as it might be replaced or changed in future builds.", check.gameObject);
            Destroy(check.gameObject);
        }
        var path = AssetDatabase.GUIDToAssetPath("cd22b2666f158cc45b56bd0d42959cac");
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        GameObject freshGameManager = Instantiate(prefab);
        instance = freshGameManager.GetComponent<GameManager>();
        DontDestroyOnLoad(freshGameManager);
    }
#endif

    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        var playerInput = gameObject.AddComponent<PlayerInput>();
        playerInput.actions = actions;
        playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
        playerInput.neverAutoSwitchControlSchemes = false;
        playerInput.actions.Enable();
        DontDestroyOnLoad(gameObject);
    }

    public static Coroutine StaticStartCoroutine(IEnumerator enumerator) => instance.StartCoroutine(enumerator);
    public static void StaticStopCoroutine(Coroutine coroutine) => instance.StopCoroutine(coroutine);




    public static PlayerInput GetPlayerInput() {
        if (instance == null) {
            return FindObjectOfType<PlayerInput>();
        }
        return instance.GetComponent<PlayerInput>();
    }

    public static void PlayerGotCockVored() {
        instance.playerGotCockvoredEvent.Raise();
    }

    public static AudioPack GetFootStep(PhysicMaterial material) {
        instance.physicsMaterialExtensionDatabase.TryGetImpactInfo(material, PhysicsMaterialExtension.PhysicMaterialInfoType.Soft, PhysicsMaterialExtension.PhysicsResponseType.Footstep, out PhysicsMaterialExtension.ImpactInfo info);
        return info.soundEffect;
    }
    public static PhysicsMaterialExtensionDatabase GetPhysicsMaterialExtensionDatabase() {
        return instance.physicsMaterialExtensionDatabase;
    }
}

internal class LocalizationEditorSettings {
}
