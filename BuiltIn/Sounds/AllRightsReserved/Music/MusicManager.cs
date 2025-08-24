using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour {
    [SerializeField] private MusicalState gameOverState;
    [SerializeField] private BriefingState briefingState;
    [SerializeField] private MusicalState firstChurnState;
    [SerializeField] private StingerTrack discoveredState;
    [SerializeField] private StingerTrack alertedState;
    [SerializeField] private MusicalState cockVoreState;
    [SerializeField] private MusicalState completedPrimaryObjectives;
    [SerializeField] private MusicalState mainMenuState;
    [SerializeField] private MusicalState successState;

    [SerializeField] private GameEvent startBriefing;
    [SerializeField] private GameEvent endBriefing;

    [SerializeField] private AudioMixerGroup group;

    [CanBeNull] private MusicalState currentState;
    private const float overallVolume = 0.6f;
    private bool hasChurned = false;
    private bool hasPrimary = false;
    private bool hasPlayer = false;

    private KnowledgeDatabase.KnowledgeLevel lastKnowledge;
    private void Start() {
        KnowledgeDatabase.globalKnowledgeLevelChanged += OnGlobalKnowledgeLevelChanged;
        Pauser.pauseChanged += OnPauseChanged;
        SceneManager.sceneLoaded += OnSceneLoaded;
        startBriefing.triggered += OnStartBriefing;
        endBriefing.triggered += OnEndBriefing;
        LevelManager.levelCompleted += OnLevelEnded;
        LevelManager.levelGameOver += OnGameOver;
        if (SceneManager.GetActiveScene().name == "MainMenu") {
            Switch(mainMenuState, false);
        }
    }

    private void OnLevelEnded() {
        Switch(successState, true);
    }

    private void Update() {
        if (!hasPlayer) {
            var player = CharacterBase.GetPlayer();
            if (player != null) {
                player.voreMachine.voreStart += OnCockCockVoreStart;
                player.voreMachine.voreEnd += OnCockCockVoreEnd;
                hasPlayer = true;
            }
        }
    }

    private void Switch(MusicalState newState, bool instantly) {
        currentState?.OnEnd(this, instantly);
        currentState = newState;
        currentState?.OnStart(this, overallVolume, group);
    }

    private void OnGameOver() {
        if (currentState != gameOverState) {
            Switch(gameOverState, true);
        }
    }

    private void OnEndBriefing() {
        Switch(null, false);
    }

    private void OnStartBriefing() {
        Switch(briefingState, false);
    }

    private void OnCockCockVoreStart(CockVoreMachine.VoreStatus status) {
        if (KnowledgeDatabase.GetMaxPlayerKnowledgeLevel() == KnowledgeDatabase.KnowledgeLevel.Ignorant) {
            Switch(cockVoreState, false);
        }
    }
    private void OnCockCockVoreEnd(CockVoreMachine.VoreStatus status) {
        if (KnowledgeDatabase.GetMaxPlayerKnowledgeLevel() == KnowledgeDatabase.KnowledgeLevel.Ignorant) {            
            if(!hasPrimary && ObjectiveManager.HasCompletedObjectives()) {
                Switch(completedPrimaryObjectives, false);
                hasPrimary = true;
                hasChurned = true;
            } else if(!hasChurned){
                Switch(firstChurnState, false);
                hasChurned = true;
            }
        }
    }

    private void OnPauseChanged(bool paused) {
        currentState?.SetPaused(paused);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode) {
        KnowledgeDatabase.ForcePoll();
        hasPlayer = false;
        hasChurned = false;
        if (scene.name == "MainMenu") {
            Switch(mainMenuState, false);
        } else {
            Switch(null, false);
        }
    }

    private void OnGlobalKnowledgeLevelChanged(KnowledgeDatabase.KnowledgeLevel knowledgelevel) {
        // can only happen in editor.
        if (this == null || Pauser.GetPaused()) {
            return;
        }
        if (knowledgelevel == KnowledgeDatabase.KnowledgeLevel.Ignorant && lastKnowledge != KnowledgeDatabase.KnowledgeLevel.Ignorant) {
            Switch(null, false);
        }
        if (knowledgelevel == KnowledgeDatabase.KnowledgeLevel.Investigative && lastKnowledge != KnowledgeDatabase.KnowledgeLevel.Alert && currentState != discoveredState) {
            Switch(discoveredState, true);
        }
        if (knowledgelevel == KnowledgeDatabase.KnowledgeLevel.Alert && lastKnowledge != KnowledgeDatabase.KnowledgeLevel.Alert && currentState != alertedState) {
            Switch(alertedState, true);
        }
        lastKnowledge = knowledgelevel;
    }
}