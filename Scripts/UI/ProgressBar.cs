using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {
    [SerializeField] private Sprite[] churnMaskSprites;
    [SerializeField] private Image rectangleOutline;
    
    private bool flashing;
    private double startFlashTime;
    private Material material;
    private float progress;
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");
    private static readonly int Progress = Shader.PropertyToID("_Progress");
    private const float maxAlpha = 0.7f;
    private const float minAlpha = 0.3f;

    private void Awake() {
        material = Material.Instantiate(rectangleOutline.material);
        rectangleOutline.material = material;
        material.SetTexture(MainTex, churnMaskSprites[UnityEngine.Random.Range(0, churnMaskSprites.Length)].texture);
        progress = 0f;
        material.SetFloat(Progress, progress);
    }

    private void Update() {
        if (!flashing) {
            return;
        }

        double t = Time.timeAsDouble - startFlashTime;
        rectangleOutline.color = rectangleOutline.color.With(a: Mathf.Abs(Mathf.Sin((float)t*4f)).Remap(0f,1f,minAlpha,maxAlpha));
        material.SetFloat(Progress, Mathf.Clamp01(progress+Mathf.Sin((float)t*4f).Remap(-1f,1f,-0.05f,0.05f)));
    }

    public void SetProgress(float progress) {
        this.progress = progress;
        if (!flashing) {
            material.SetFloat(Progress, progress);
        }
    }

    public void SetFlash(bool flashing) {
        startFlashTime = Time.timeAsDouble;
        this.flashing = flashing;
        if (!flashing) {
            rectangleOutline.color = rectangleOutline.color.With(a: maxAlpha);
        }
    }

}
