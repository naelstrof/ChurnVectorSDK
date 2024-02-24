using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {
    private CanvasGroup group;
    void Awake() {
        LevelManager.levelGameOver += OnLevelGameOver;
        SceneManager.sceneUnloaded += OnSceneUnload;
        group = GetComponent<CanvasGroup>();
    }

    private void OnSceneUnload(Scene arg0) {
        StartCoroutine(LerpGameOverToAlpha(0f));
    }

    private void OnLevelGameOver() {
        StartCoroutine(LerpGameOverToAlpha(1f));
    }

    private IEnumerator LerpGameOverToAlpha(float target) {
        yield return new WaitUntil(()=>!Cutscene.CutsceneIsPlaying());
        float startTime = Time.unscaledTime;
        float duration = 1f;
        float startAlpha = group.alpha;
        
        while (Time.unscaledTime - startTime < duration) {
            float t = (Time.unscaledTime - startTime)/duration;
            group.alpha = Mathf.Lerp(startAlpha, target, t);
            yield return null;
        }
    }

    void OnDestroy() {
        LevelManager.levelGameOver -= OnLevelGameOver;
        SceneManager.sceneUnloaded -= OnSceneUnload;
    }
}
