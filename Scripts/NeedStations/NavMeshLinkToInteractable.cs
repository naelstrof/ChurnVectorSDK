using UnityEngine;

public class NavMeshLinkToInteractable : MonoBehaviour {
    [SerializeField]
    private MonoBehaviour interactableTarget;
    public IInteractable GetInteractable() => interactableTarget as IInteractable;
}
