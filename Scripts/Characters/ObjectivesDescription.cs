using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;

public class ObjectivesDescription : InitializationManagerInitialized {
    [SerializeField, SerializeReference, SubclassSelector]
    private List<Objective> objectives;

    [SerializeField] private LocalizedString debriefString;

    private static ObjectivesDescription instance;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        FindObjectOfType<ObjectiveManager>(true).SetObjectives(objectives);
    }

    public override InitializationManager.InitializationStage GetInitializationStage() {
        return InitializationManager.InitializationStage.AfterLevelLoad;
    }

    public static string GetDebrief() {
        if (instance == null) {
            instance = FindObjectOfType<ObjectivesDescription>();
        }

        if (instance == null) {
            return "";
        }
        
        if (instance.debriefString == null) {
            return "";
        }
        
        return instance.debriefString.GetLocalizedString();
    }

    public override Task OnInitialized() {
        foreach (var objective in objectives) {
            objective.OnRegister();
        }
        return base.OnInitialized();
    }
    protected override void OnDestroy() {
        foreach (var objective in objectives) {
            objective.OnUnregister();
        }
        base.OnDestroy();
    }
    
    public static List<Objective> GetObjectives() {
        if (instance == null) {
            instance = FindObjectOfType<ObjectivesDescription>();
        }

        if (instance == null) {
            return null;
        }

        return instance.objectives;
    }
}
