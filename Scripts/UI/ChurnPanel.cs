using UnityEngine;
using UnityEngine.UI;

public class ChurnPanel : MonoBehaviour {
    [SerializeField] private Image mugShotSprite;
    [SerializeField] private ProgressBar churnProgress;
    [SerializeField] private GameObject churnedStamp;
    private CumStorage.CumSource source;

    public void Initialize(CumStorage.CumSource target) {
        source = target;
        
        mugShotSprite.sprite = target.GetChurnable().GetHeadSprite();
        churnProgress.SetProgress(0f);
        churnedStamp.SetActive(false);
        churnProgress.SetFlash(true);
    }

    private void Update() {
        float progress = source.GetChurnProgress();
        churnProgress.SetProgress(progress);
        if (progress >= 1f && churnedStamp.activeSelf == false) {
            churnedStamp.SetActive(true);
            churnProgress.SetFlash(false);
        }
    }
}
