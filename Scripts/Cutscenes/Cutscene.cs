using System.Collections;
using System.Collections.Generic;
using Cutscenes.Subscenes;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class Cutscene {
    private class CutsceneBehaviour : MonoBehaviour {
        private Cutscene playingCutscene;

        public void SetPlayingCutscene(Cutscene playingCutscene) {
            this.playingCutscene = playingCutscene;
        }

        void OnDisable() {
            if (playingCutscene == currentPlayingCutscene) {
                currentPlayingCutscene = null;
            }
        }
    }

    private bool hasPlayed;
    private bool playing;
    private int currentSubscene;
    private Coroutine currentRoutine;
    private CutsceneBehaviour currentBehaviour;
    private static Cutscene currentPlayingCutscene;

    public delegate void CutsceneAction();
    public static event CutsceneAction cutsceneStarted;
    public static event CutsceneAction cutsceneEnded;
    public static bool CutsceneIsPlaying() => currentPlayingCutscene != null;
    
    [SerializeField,SerializeReference,SubclassSelector]
    protected List<Subscene> subscenes;
    
    protected virtual void OnStart() {
    }

    protected virtual void OnEnd() {
    }

    public bool IsDone() => hasPlayed && !playing;
    public bool IsPlaying() => playing;

    private void Skip() {
        if (currentRoutine == null) {
            return;
        }

        if (currentBehaviour != null) {
            currentBehaviour.StopCoroutine(currentRoutine);
        }

        currentRoutine = null;
        if (currentSubscene < subscenes.Count && currentSubscene > -1) {
            subscenes[currentSubscene].OnEnd();
        }
    }
    private void OnSkip(InputAction.CallbackContext obj) {
        Skip();
    }
    public IEnumerator Begin(GameObject owner) {
        yield return null;
        yield return new WaitUntil(()=>currentPlayingCutscene == null && InitializationManager.GetCurrentStage() == InitializationManager.InitializationStage.FinishedLoading);
        GameManager.GetPlayerInput().actions["SkipCutscene"].performed += OnSkip;
        try {
            currentPlayingCutscene = this;
            currentBehaviour = owner.AddComponent<CutsceneBehaviour>();
            currentBehaviour.SetPlayingCutscene(this);
            hasPlayed = true;
            playing = true;
            cutsceneStarted?.Invoke();
            OnStart();
            currentSubscene = 0;
            while (currentSubscene < subscenes.Count) {
                var subscene = subscenes[currentSubscene];
                currentRoutine = currentBehaviour.StartCoroutine(subscene.Begin());
                yield return new WaitUntil(() => subscene.IsDone() || currentBehaviour == null);
                currentSubscene++;
            }
            OnEnd();
            cutsceneEnded?.Invoke();
        } finally {
            if (this == currentPlayingCutscene) {
                currentPlayingCutscene = null;
            }

            playing = false;
            Object.Destroy(currentBehaviour);
            GameManager.GetPlayerInput().actions["SkipCutscene"].performed -= OnSkip;
        }
    }
}
