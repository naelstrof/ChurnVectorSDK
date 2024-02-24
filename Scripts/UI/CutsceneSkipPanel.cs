using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class CutsceneSkipPanel : MonoBehaviour {
    private CanvasGroup group;
    private IDisposable handle;
    private void Start() {
        group = GetComponent<CanvasGroup>();
        Cutscene.cutsceneStarted += OnCutsceneStart;
        Cutscene.cutsceneEnded += OnCutsceneEnd;
    }

    private void OnEnable() {
        handle = InputSystem.onAnyButtonPress.Call(OnAnyButtonPress);
    }

    private void OnDisable() {
        handle?.Dispose();
        Cutscene.cutsceneStarted -= OnCutsceneStart;
        Cutscene.cutsceneEnded -= OnCutsceneEnd;
    }

    private void OnCutsceneStart() {
        group.alpha = 0f;
    }
    private void OnAnyButtonPress(InputControl ctx) {
        if (!Cutscene.CutsceneIsPlaying()) {
            return;
        }
        StopAllCoroutines();
        StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine() {
        float startTime = Time.time;
        float startAlpha = group.alpha;
        float duration = 0.8f;
        while (Time.time < startTime + duration) {
            float t = (Time.time - startTime) / duration;
            group.alpha = Mathf.Lerp(startAlpha, 1f, Naelstrof.Easing.Easing.Cubic.Out(t));
            yield return null;
        }
        group.alpha = 1f;
        yield return new WaitForSeconds(1f);
        
        startTime = Time.time;
        while (Time.time < startTime + duration) {
            float t = (Time.time - startTime) / duration;
            group.alpha = Mathf.Lerp(1f, 0f, Naelstrof.Easing.Easing.Cubic.Out(t));
            yield return null;
        }
        group.alpha = 0f;
    }

    private void OnCutsceneEnd() {
        StopAllCoroutines();
        group.alpha = 0f;
    }
}
