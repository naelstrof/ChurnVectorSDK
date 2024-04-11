using System;
using System.Collections;
using System.Collections.Generic;
using JigglePhysics;
using Naelstrof.Easing;
using Naelstrof.Inflatable;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class GenericVoreContainer : VoreContainer {
    [SerializeField] private Inflatable inflater;
    private AudioSource audioSource;
    private Animator targetAnimator;
    private CharacterBase target;
    private CumStorage storage;
    public override CumStorage GetStorage() => storage;
    private float churnAccumulator;
    private const float churnTick = 3f;
    private bool churning = false;
    public override Transform GetStorageTransform() {
        return targetAnimator.GetBoneTransform(HumanBodyBones.Spine);
    }
    public override void Initialize(CharacterBase target) {
        storage = new CumStorage();
        this.target = target;
        targetAnimator = target.GetDisplayAnimator();
        inflater?.OnEnable();
        audioSource = target.gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 25f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        UpdateInflater();
    }

    public override void LateUpdate() {
        churnAccumulator += Time.deltaTime;
        if (churnAccumulator > churnTick) {
            churnAccumulator -= churnTick;
            var churnPack = GameManager.GetLibrary().churnPack;
            var gurglePack = GameManager.GetLibrary().tummyGurglesPack;
            if (!storage.GetDoneChurning()) {
                if (target.IsPlayer()) {
                    CharacterDetector.PlayInvestigativeAudioPackOnSource(target, churnPack, audioSource, 8f, Easing.Cubic.Out(1f - storage.GetChurnProgress()));
                } else {
                    churnPack.Play(audioSource);
                }

                float pitchShift = 1f - Mathf.Pow(storage.GetVolume()/10f + 1f, -2f);
                float pitchVariance = churnPack.GetPitchVariance();
                audioSource.pitch = Mathf.Lerp(1f+pitchVariance, 1f-pitchVariance, pitchShift);
            } else {
                if (!audioSource.isPlaying && storage.GetVolume() >= 0.5f) {
                    if (target.IsPlayer()) {
                        CharacterDetector.PlayInvestigativeAudioPackOnSource(target, gurglePack, audioSource, Mathf.Min(2f*storage.GetVolume(),8f), Easing.Cubic.Out(Mathf.Clamp01(storage.GetVolume()*0.5f)));
                    } else {
                        churnPack.Play(audioSource);
                    }

                    float pitchShift = 1f - Mathf.Pow(storage.GetVolume()/10f + 1f, -2f);
                    float pitchVariance = gurglePack.GetPitchVariance();
                    audioSource.pitch = Mathf.Lerp(1f+pitchVariance, 1f-pitchVariance, pitchShift);
                }
            }

            UpdateInflater();
        }
    }

    public override void AddChurnable(IChurnable churnable) {
        storage.AddChurnable(churnable);
        UpdateInflater();
    }

    private void UpdateInflater() {
        if (inflater == null) {
            return;
        }
        //inflater.SetSize(Mathf.Log(cumAmount*2f + 1f, 2f)+1f+leftToChurn*4f, target);
        float newVolume = Mathf.Sqrt(storage.GetVolume() * 4f) + 1f;
        if (Math.Abs(inflater.GetSize() - newVolume) > Mathf.Epsilon) {
            inflater.SetSize(newVolume, target);
        }
    }

    public void BeginEmission(CumStorage.ChurnedAction startEvent = null, CumStorage.EmitCumAction emitEvent = null, CumStorage.ChurnedAction endEvent = null) {
        storage.BeginEmission(1.2f, target, startEvent, emitEvent, endEvent);
    }
}
