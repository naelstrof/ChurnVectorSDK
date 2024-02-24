using System;
using System.Collections.Generic;
using UnityEngine;

public partial class CharacterBase {
    protected IInteractable interactTarget;
    protected IInteractable activeInteractable;
    private static Collider[] colliderResults = new Collider[32];
    private ColliderSorter colliderSorter;
    private bool wasInteracting;

    public delegate void UseInteractableAction(IInteractable interactable);
    public event UseInteractableAction usedInteractable;

    public bool IsInteracting() => activeInteractable != null;

    private void OnEnableInteractor() {
        if (inputGenerator == null) {
            return;
        }
        inputGenerator.interactInputChanged += OnInteractInputChanged;
    }

    private void OnDisableInteractor() {
        if (inputGenerator == null) {
            return;
        }
        inputGenerator.interactInputChanged -= OnInteractInputChanged;
    }

    protected IInteractable QueryBestInteractable() {
        if (ticketLock.GetLocked(TicketLock.LockFlags.IgnoreUsables)) {
            return null;
        }
        int sphereHits = Physics.OverlapSphereNonAlloc(transform.position, 1.5f, colliderResults, interactableMask);
        Array.Sort(colliderResults, 0, sphereHits, colliderSorter);
        for (int i = 0; i < sphereHits; i++) {
            Collider col = colliderResults[i];
            IInteractable interactable = col.GetComponent<IInteractable>();
            if (interactable != null && interactable.CanInteract(this) && interactable.ShouldInteract(this)) {
                return interactable;
            }
        }
        return null;
    }

    private class ColliderSorter : IComparer<Collider> {
        private Transform transform;
        private CharacterBase character;

        public ColliderSorter(Transform transform, CharacterBase character) {
            this.transform = transform;
            this.character = character;
        }
        public int Compare(Collider x, Collider y) {
            if (x == null) {
                return 1;
            }
            if (x is MeshCollider { convex: false }) {
                return 1;
            }
            if (y == null) {
                return -1;
            }
            if (y is MeshCollider { convex: false }) {
                return -1;
            }
            var pos = transform.position;
            Vector3 facingDirection = character.GetFacingDirection() * Vector3.forward;
            if (character.IsPlayer()) {
                facingDirection = OrbitCamera.GetPlayerIntendedRotation() * Vector3.forward;
            }

            Vector3 xPos = x.ClosestPoint(pos);
            Vector3 yPos = y.ClosestPoint(pos);
            float xDist = Vector3.Distance(pos, xPos);
            float yDist = Vector3.Distance(pos, yPos);


            Vector3 dir = facingDirection;
            float xDot = Mathf.Clamp01(-Vector3.Dot((xPos-pos).normalized, dir));
            float yDot = Mathf.Clamp01(-Vector3.Dot((yPos-pos).normalized, dir));
            
            if (character.IsPlayer()) {
                pos = OrbitCamera.GetPlayerIntendedPosition();
            }

            float xDistToScreenCenter = Vector3.Distance(Vector3.Project(xPos-pos, dir)+pos, xPos);
            float yDistToScreenCenter = Vector3.Distance(Vector3.Project(yPos-pos, dir)+pos, yPos);

            float xScore = xDist + xDot + xDistToScreenCenter;
            float yScore = yDist + yDot + yDistToScreenCenter;
            
            return xScore.CompareTo(yScore);
        }
    }

    public bool IsInteractingWith(IInteractable target) {
        return activeInteractable == target;
    }

    public bool InteractWith(IInteractable target) {
        if (!target.CanInteract(this)) {
            return false;
        }
        if (activeInteractable != null) {
            OnEndInteractInput(activeInteractable);
        }
        OnStartInteractInput(target);
        return true;
    }

    public void StopInteractionWith(IInteractable target) {
        if (activeInteractable == target) {
            activeInteractable.OnEndInteract(this);
            OnEndInteractInput(activeInteractable);
            activeInteractable = null;
        }
    }

    public void StopInteraction() {
        if (activeInteractable != null) {
            activeInteractable.OnEndInteract(this);
            OnEndInteractInput(activeInteractable);
            activeInteractable = null;
        }
    }

    private void OnInteractInputChanged(bool interacting) {
        bool raisingEdge = interacting && !wasInteracting;
        wasInteracting = interacting;
        if (!raisingEdge) {
            return;
        }
        if (activeInteractable != null) {
            OnEndInteractInput(activeInteractable);
            return;
        }

        if (activeInteractable == null && interactTarget != null && !ticketLock.GetLocked(TicketLock.LockFlags.IgnoreUsables)) {
            OnStartInteractInput(interactTarget);
        }
    }

    protected virtual void OnStartInteractInput(IInteractable interactable) {
        activeInteractable = interactable;
        usedInteractable?.Invoke(interactable);
        activeInteractable.OnBeginInteract(this);
    }
    protected virtual void OnEndInteractInput(IInteractable interactable) {
        cockVoreMachine.StopVore();
        activeInteractable.OnEndInteract(this);
        activeInteractable = null;
    }
}
