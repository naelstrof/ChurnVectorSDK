using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ObjectivesDescription : MonoBehaviour {
    [SerializeField, SerializeReference, SubclassSelector]
    private List<Objective> objectives;

    private static ObjectivesDescription instance;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        FindObjectOfType<ObjectiveManager>(true).SetObjectives(objectives);
    }

    private void OnEnable() {
        foreach (var objective in objectives) {
            objective.OnRegister();
        }
    }

    private void OnDisable() {
        foreach (var objective in objectives) {
            objective.OnUnregister();
        }
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
