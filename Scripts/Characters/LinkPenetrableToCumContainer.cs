using UnityEngine;

public class LinkPenetrableToCumContainer : MonoBehaviour {
    private ICumContainer target;

    private void Awake() {
        target ??= GetComponentInParent<ICumContainer>();
    }

    public void SetCumContainer(ICumContainer container) {
        target = container;
    }

    public ICumContainer GetCumContainer() => target;
}
