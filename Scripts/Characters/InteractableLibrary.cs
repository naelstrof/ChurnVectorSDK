using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class InteractableLibrary : MonoBehaviour {
    private static InteractableLibrary instance;
    private List<IInteractable> interactables;
    private List<IInteractable> possibleInteractables;

    private void Awake() {
        if (instance == null) {
            interactables = new List<IInteractable>();
            possibleInteractables = new List<IInteractable>();
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private struct InteractableWithDistance<T> where T : IInteractable {
        public T interactable;
        public float distance;
    }

    public static bool TryGetInteractableOfType<T>([CanBeNull] out T interactableOut, Vector3 targetPosition) where T : IInteractable {
        InteractableWithDistance<T>? nearestThing = null;
        foreach (var interactable in instance.interactables) {
            if (interactable is not T real) continue;
            float distance = Vector3.Distance(interactable.transform.position, targetPosition);
            if (!nearestThing.HasValue || distance < nearestThing.Value.distance) {
                nearestThing = new InteractableWithDistance<T>{
                    interactable = real,
                    distance = distance,
                };
            }
        }

        if (!nearestThing.HasValue) {
            interactableOut = default;
            return false;
        }
        
        interactableOut = nearestThing.Value.interactable;
        return true;
    }

    public static IInteractable GetRandomInteractable(CharacterBase character) {
        instance.possibleInteractables.Clear();
        foreach (var needStation in instance.interactables) {
            if (needStation.ShouldInteract(character) && needStation.CanInteract(character)) {
                instance.possibleInteractables.Add(needStation);
            }
        }
        if (instance.possibleInteractables.Count == 0) {
            return null;
        }
        // If the player is the one vored, make the NPC diceroll to empty them rather than rely on random interactable choice
        foreach (var source in character.voreContainer.GetStorage().GetSources()) {
            if(source.GetChurnable() is CharacterBase churnable && churnable.IsPlayer()) {
                if(Random.Range(0, 0.4f) < 0.1f) {
                    if(TryGetInteractableOfType<BreedingStand>(out var b, character.transform.position)) {
                        return b;
                    }
                }
            }
        }

        return instance.possibleInteractables[Random.Range(0, instance.possibleInteractables.Count)];
    }

    public static void AddInteractable(IInteractable self) {
        #if UNITY_EDITOR
        if (self is NeedStation need) {
            if (!NavMesh.SamplePosition(self.transform.position, out NavMeshHit targetHit, FollowPathToPoint.maxDistanceFromNavmesh, NavMesh.AllAreas)) {
                Debug.LogWarning("Need station " + need + " is too far off the navmesh! AI will get stuck trying to get to it.", need.gameObject);
            }
        }
        #endif

        instance.interactables.Add(self);
    }

    public static void RemoveInteractable(IInteractable self) {
        instance.interactables.Remove(self);
    }
}
