using System.Threading.Tasks;
using DPG;
using UnityEngine;
using UnityEngine.VFX;

public class PlushFuckStation : BreedingStand {
    [SerializeField]
    private GameObject plushInUse;

    [SerializeField]
    private Color clothColor;

    [SerializeField]
    private Texture2D clothTexture;

    public override void OnBeginInteract(CharacterBase from) {
        base.OnBeginInteract(from);
        plushInUse.SetActive(true);
        foreach (var obj in workingGraphics) {
            obj.gameObject.SetActive(false);
        }
    }

    protected override void SetBroken(bool broken) {
        if (!this.broken && broken) {
            GameObject visualEffectGameObject = new GameObject("TemporaryVFX", typeof(VisualEffect));
            visualEffectGameObject.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            VisualEffect visualEffect = visualEffectGameObject.GetComponent<VisualEffect>();
            visualEffect.visualEffectAsset = breakVFX;
            visualEffect.SetVector4("ThreadColor", clothColor);
            visualEffect.SetTexture("ClothingTexture", clothTexture);
            visualEffect.Play();
            Destroy(visualEffectGameObject, 3f);
            AudioPack.PlayClipAtPoint(breakAudioPack, transform.position);
        }

        this.broken = broken;
        foreach (var obj in brokenGraphics) {
            obj.gameObject.SetActive(broken);
        }
        foreach (var obj in workingGraphics) {
            obj.gameObject.SetActive(!broken);
        }
    }

    public override void OnEndInteract(CharacterBase from) {
        base.OnEndInteract(from);
        plushInUse.SetActive(false);
        if (!broken) {
            foreach (var obj in workingGraphics) {
                obj.gameObject.SetActive(true);
            }
        }
    }

    public override Task OnInitialized() {
        penetrable = plushInUse.GetComponentInChildren<Penetrable>();
        plushInUse.SetActive(false);
        return base.OnInitialized();
    }
}
