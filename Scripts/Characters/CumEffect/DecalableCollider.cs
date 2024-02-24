using UnityEngine;

public class DecalableCollider : MonoBehaviour {
    [SerializeField]
    private Renderer[] renderers;

    public Renderer[] GetDecalableRenderers() {
        return renderers;
    }

    public void SetDecalableRenderers(Renderer[] renderers) {
        this.renderers = renderers;
    }
    
}
