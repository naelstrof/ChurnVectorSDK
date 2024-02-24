using System.Collections;
using System.Collections.Generic;
using Naelstrof.Easing;
using PenetrationTech;
using UnityEngine;

public partial class CharacterBase {
    private Penetrator cockVoreDick;
    private ParticleSystem leakSystem;
    private Modifier voreSpeedModifier;
    private Coroutine waitRoutine;
    public VoreMachine cockVoreMachine = new VoreMachine();
    private Coroutine churnRoutine;
    
    
    private TicketLock.Ticket cockVoreFaceDirectionLock;
    public bool CanCockVorePlayer() => cockVoreDick != null;
    public Penetrator GetDickPenetrator() {
        InitializeCockvoreDick();
        return cockVoreDick;
    }

    private void InitializeCockvoreDick() {
        if (cockVoreDick != null) {
            return;
        }
        cockVoreDick = gameObject.GetComponentInChildren<Penetrator>();
        if (cockVoreDick == null) {
            return;
        }
        var dickCum = gameObject.AddComponent<DickCum>();
        var cumParticleSystem = Instantiate(GameManager.GetLibrary().cumPrefab.gameObject, transform).GetComponent<CumCollision>();
        leakSystem = Instantiate(GameManager.GetLibrary().cumPrefab.gameObject, transform).GetComponent<ParticleSystem>();
        leakSystem.GetComponent<CumCollision>().SetAttachedDick(cockVoreDick);
        cumParticleSystem.SetAttachedDick(cockVoreDick);
        dickCum.SetInfo(cumParticleSystem.GetComponent<ParticleSystem>(), cockVoreDick, GameManager.GetLibrary().glorpPack, this);
        var dickMovementListener = new DickMovementListener();
        dickMovementListener.SetDickCumTarget(dickCum);
        cockVoreDick.listeners.Add(dickMovementListener);
    }

    private void AwakeCockVore() {
        //cockVoreDick = GetComponentInChildren<Penetrator>();
        InitializeCockvoreDick();
        cockVoreMachine.Initialize(this, cockVoreDick);
        cockVoreMachine.cockVoreStart += OnCockVoreStart;
        cockVoreMachine.cockVoreUpdate += OnCockVoreUpdate;
        cockVoreMachine.cockVoreEnd += OnCockVoreEnd;
        voreSpeedModifier = new Modifier(1f);
        balls.GetStorage().startChurn += OnChurnStart;
    }

    private void OnChurnStart(CumStorage.CumSource churnable) {
        churnRoutine ??= StartCoroutine(ChurnRoutine(churnable));
    }

    private IEnumerator ChurnRoutine(CumStorage.CumSource churnable) {
        yield return new WaitUntil(churnable.GetDoneChurning);
        if (IsCockvoring()) {
            yield break;
        }

        StartCoroutine(DialogueLibrary.GetDialogue(DialogueLibrary.DialogueGroupType.ChurnFinished).Begin(new List<DialogueCharacter> {
            DialogueCharacterSpecificCharacter.Get(this),
            DialogueCharacterInanimateObject.Get(balls.GetBallsTransform(), DialogueLibrary.GetCondomTheme()),
        }));
        churnRoutine = null;
    }

    private void UpdateCockVore() {
        if (activeInteractable != null) {
            var newCockVorable = activeInteractable.transform.GetComponent<ICockVorable>();
            if (newCockVorable != null && activeInteractable != null) {
                waitRoutine ??= StartCoroutine(WaitAndThenCockVore(newCockVorable));
            }
        }
    }

    private void LateUpdateCockVore() {
        cockVoreMachine.LateUpdate();
        SetLeakState();
    }

    private IEnumerator WaitAndThenCockVore(ICockVorable cockVorable) {
        try {
            for (int i = 0; i < 10; i++) {
                if (inputGenerator.GetWishDirection() != Vector3.zero || cockVoreMachine.IsCockvoring()) {
                    i = 0;
                }
                yield return new WaitForSeconds(0.12f);
            }

            if (cockVoreDick != null && !cockVoreMachine.IsCockvoring() && activeInteractable != null && activeInteractable.transform.GetComponent<ICockVorable>() == cockVorable) {
                cockVoreMachine.StartVore(cockVorable);
            }
        } finally {
            waitRoutine = null;
        }
    }
    
    private void OnCockVoreStart(VoreMachine.CockVoreStatus status) {
        if (Random.Range(0f, 1f) > 0.25f) {
            StartCoroutine(DialogueLibrary.GetDialogue(DialogueLibrary.DialogueGroupType.Vore).Begin(new List<DialogueCharacter> {
                DialogueCharacterSpecificCharacter.Get(status.other.transform.GetComponent<CharacterBase>()),
                DialogueCharacterSpecificCharacter.Get(this),
            }));
        }

        cockVoreFaceDirectionLock ??= ticketLock.AddLock(this, TicketLock.LockFlags.FacingDirectionLock | TicketLock.LockFlags.IgnoreUsables);
        AddSpeedModifier(voreSpeedModifier);
    }
    private void OnCockVoreEnd(VoreMachine.CockVoreStatus status) {
        ticketLock.RemoveLock(ref cockVoreFaceDirectionLock);
        RemoveSpeedModifier(voreSpeedModifier);
    }
    private void OnCockVoreUpdate(VoreMachine.CockVoreStatus status) {
        voreSpeedModifier.SetMultiplier(Easing.Cubic.In(status.progress));
    }
    
    private void SetLeakState() {
        if (leakSystem == null) {
            return;
        }
        var r = GetStorage().GetVolume() - 2f;
        if(r <= 0f) r = 0f;
        if (cockVoreMachine.IsCockvoring() || !GetStorage().GetDoneChurning()) r += 1f;
        if (!leakSystem.isPlaying) {
            leakSystem.Play();
        }

        var rate = Mathf.Sqrt(r);

        var main = leakSystem.main;

        main.startSpeed = rate;
        main.startSizeMultiplier = rate * 0.1f;

        var emissionModule = leakSystem.emission;
        emissionModule.rateOverTime = rate > 0f ? 60f : 0f;
    }
    
    public bool IsCockvoring() => cockVoreMachine.IsCockvoring();
}
