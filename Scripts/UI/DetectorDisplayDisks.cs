using System;
using System.Collections;
using UnityEngine;

public class DetectorDisplayDisks : MonoBehaviour {
    [SerializeField] private MeshRenderer innerDisk;
    [SerializeField] private MeshRenderer secondInnerDisk;
    [SerializeField] private MeshRenderer outerDisk;

    private bool known;
    private CharacterDetector detector;
    private Animator displayAnimator;

    private float alpha {
        get => innerDisk.material.color.a;
        set {
            innerDisk.material.color = innerDisk.material.color.With(a: value);
            outerDisk.material.color = outerDisk.material.color.With(a: value);
            secondInnerDisk.material.color = secondInnerDisk.material.color.With(a: value);
        }
    }
    private void OnEnable() {
        detector = GetComponentInParent<CharacterDetector>();
        displayAnimator = detector.GetDisplayAnimator();
        innerDisk.material.color = Color.white.With(a:0f);
        secondInnerDisk.material.color = Color.red.With(a:0f);
        outerDisk.material.color = innerDisk.material.color;
        if (detector.knowledgeDatabase == null) return;
        detector.knowledgeDatabase.awarenessChanged += OnAwarenessChanged;
        if (CharacterBase.GetPlayer() == null) return;
        var knowledge = detector.knowledgeDatabase.GetKnowledge(CharacterBase.GetPlayer().gameObject);
        OnAwarenessChanged(KnowledgeDatabase.KnowledgeLevel.Ignorant, knowledge);
    }

    //private void Start() {
        //detector.knowledgeDatabase.awarenessChanged -= OnAwarenessChanged;
        //detector.knowledgeDatabase.awarenessChanged += OnAwarenessChanged;
    //}

    private void OnDisable() {
        detector.knowledgeDatabase.awarenessChanged -= OnAwarenessChanged;
    }

    private void Update() {
        Vector3 diff = OrbitCamera.GetPlayerIntendedPosition() - transform.position;
        transform.forward = diff.normalized;
    }

    private IEnumerator FadeTo(float targetAlpha) {
        float startTime = Time.time;
        float duration = 0.25f;
        float startAlpha = alpha;
        while (Time.time-startTime < duration) {
            float t = (Time.time - startTime) / duration;
            alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            yield return null;
        }
        alpha = targetAlpha;
    }

    private void SetRadius(MeshRenderer image, float newRadius) {
        image.transform.localScale = Vector2.one * newRadius;
    }

    private float GetRadius(MeshRenderer image) {
        return image.transform.localScale.x;
    }

    void OnAwarenessChanged(KnowledgeDatabase.KnowledgeLevel lastLevel, KnowledgeDatabase.Knowledge knowledge) {
        if (knowledge.target != CharacterBase.GetPlayer().gameObject) {
            return;
        }

        if (!known && knowledge.awareness != 0f) {
            StartCoroutine(FadeTo(1f));
            known = true;
        } else if (known && knowledge.awareness == 0f) {
            StartCoroutine(FadeTo(0f));
            known = false;
        }

        SetRadius(innerDisk, Mathf.Lerp(0f, GetRadius(outerDisk), Mathf.Clamp01(knowledge.awareness)));
        SetRadius(secondInnerDisk, Mathf.Lerp(0f, GetRadius(outerDisk), Mathf.Clamp01(knowledge.awareness - 1f)));
        switch (knowledge.GetKnowledgeLevel()) {
            case KnowledgeDatabase.KnowledgeLevel.Ignorant:
                innerDisk.material.color = Color.white.With(a: innerDisk.material.color.a);
                break;
            case KnowledgeDatabase.KnowledgeLevel.Investigative:
                innerDisk.material.color = Color.yellow.With(a: innerDisk.material.color.a);
                break;
            case KnowledgeDatabase.KnowledgeLevel.Alert:
                innerDisk.material.color = Color.red.With(a: innerDisk.material.color.a);
                break;
        }
        outerDisk.material.color = innerDisk.material.color;
    }
}
