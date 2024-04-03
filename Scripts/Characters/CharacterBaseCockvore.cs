using System.Collections;
using System.Collections.Generic;
using Naelstrof.Easing;
using DPG;
using UnityEngine;

public partial class CharacterBase {
    private Penetrator cockVoreDick;
    private ParticleSystem leakSystem;
    private Modifier voreSpeedModifier;
    private Coroutine waitRoutine;
    [SerializeField, SerializeReference, SubclassSelector]
    public VoreMachine voreMachine;
    private Coroutine churnRoutine;
    private DickCum dickCum;

    private float? lastPenetrationDepth;
    
    
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
        dickCum = gameObject.AddComponent<DickCum>();
        var cumParticleSystem = Instantiate(GameManager.GetLibrary().cumPrefab.gameObject, transform).GetComponent<CumCollision>();
        leakSystem = Instantiate(GameManager.GetLibrary().cumPrefab.gameObject, transform).GetComponent<ParticleSystem>();
        leakSystem.GetComponent<CumCollision>().SetAttachedDick(cockVoreDick);
        cumParticleSystem.SetAttachedDick(cockVoreDick);
        dickCum.SetInfo(cumParticleSystem.GetComponent<ParticleSystem>(), cockVoreDick, GameManager.GetLibrary().glorpPack, this);
        cockVoreDick.penetrated += OnPenetration;
    }

    private void OnPenetration(Penetrator penetrator, Penetrable penetrable, Penetrator.PenetrationArgs penetrationArgs, Penetrable.PenetrationResult result) {
        float movement = penetrationArgs.penetrationDepth - (lastPenetrationDepth ?? penetrationArgs.penetrationDepth);
        lastPenetrationDepth = penetrationArgs.penetrationDepth;
        if (Mathf.Abs(movement) < 0.001f || penetrationArgs.penetrationDepth < 0f) { // || penetrationArgs.penetrationDepth < result.holeStartDepth) {
            return;
        }
        dickCum.AddStimulation(movement);
    }

    private void AwakeCockVore() {
        //cockVoreDick = GetComponentInChildren<Penetrator>();
        InitializeCockvoreDick();
        if (voreMachine != null) {
            voreMachine.Initialize(this);
            voreMachine.voreStart += OnCockCockVoreStart;
            voreMachine.voreUpdate += OnCockCockVoreUpdate;
            voreMachine.voreEnd += OnCockCockVoreEnd;
        }

        if (voreContainer != null) {
            voreContainer.GetStorage().startChurn += OnChurnStart;
        }

        voreSpeedModifier = new Modifier(1f);
    }

    private void OnChurnStart(CumStorage.CumSource churnable) {
        churnRoutine ??= StartCoroutine(ChurnRoutine(churnable));
    }

    private IEnumerator ChurnRoutine(CumStorage.CumSource churnable) {
        yield return new WaitUntil(churnable.GetDoneChurning);
        if (IsVoring()) {
            yield break;
        }

        StartCoroutine(DialogueLibrary.GetDialogue(DialogueLibrary.DialogueGroupType.ChurnFinished).Begin(new List<DialogueCharacter> {
            DialogueCharacterSpecificCharacter.Get(this),
            DialogueCharacterInanimateObject.Get(voreContainer.GetStorageTransform(), DialogueLibrary.GetCondomTheme()),
        }));
        churnRoutine = null;
    }

    private void UpdateCockVore() {
        if (activeInteractable != null) {
            var newCockVorable = activeInteractable.transform.GetComponent<IVorable>();
            if (newCockVorable != null && activeInteractable != null) {
                waitRoutine ??= StartCoroutine(WaitAndThenCockVore(newCockVorable));
            }
        }
    }

    private void LateUpdateCockVore() {
        voreMachine?.LateUpdate();
        SetLeakState();
    }

    private IEnumerator WaitAndThenCockVore(IVorable vorable) {
        if (voreMachine == null) {
            waitRoutine = null;
            yield break;
        }
        
        try {
            for (int i = 0; i < 10; i++) {
                if (inputGenerator.GetWishDirection() != Vector3.zero || voreMachine.IsVoring()) {
                    i = 0;
                }
                yield return new WaitForSeconds(0.12f);
            }

            if (!voreMachine.IsVoring() && activeInteractable != null && activeInteractable.transform.GetComponent<IVorable>() == vorable) {
                voreMachine.StartVore(vorable);
            }
        } finally {
            waitRoutine = null;
        }
    }
    
    private void OnCockCockVoreStart(CockVoreMachine.VoreStatus status) {
        if (Random.Range(0f, 1f) > 0.25f) {
            StartCoroutine(DialogueLibrary.GetDialogue(DialogueLibrary.DialogueGroupType.Vore).Begin(new List<DialogueCharacter> {
                DialogueCharacterSpecificCharacter.Get(status.other.transform.GetComponent<CharacterBase>()),
                DialogueCharacterSpecificCharacter.Get(this),
            }));
        }

        cockVoreFaceDirectionLock ??= ticketLock.AddLock(this, TicketLock.LockFlags.FacingDirectionLock | TicketLock.LockFlags.IgnoreUsables);
        AddSpeedModifier(voreSpeedModifier);
    }
    private void OnCockCockVoreEnd(CockVoreMachine.VoreStatus status) {
        ticketLock.RemoveLock(ref cockVoreFaceDirectionLock);
        RemoveSpeedModifier(voreSpeedModifier);
    }
    private void OnCockCockVoreUpdate(CockVoreMachine.VoreStatus status) {
        voreSpeedModifier.SetMultiplier(Easing.Cubic.In(status.progress));
    }
    
    private void SetLeakState() {
        if (leakSystem == null || voreContainer == null) {
            return;
        }
        var r = voreContainer.GetStorage().GetVolume() - 2f;
        if(r <= 0f) r = 0f;
        if (voreMachine.IsVoring() || !voreContainer.GetStorage().GetDoneChurning()) r += 1f;
        if (!leakSystem.isPlaying) {
            leakSystem.Play();
        }

        var rate = Mathf.Sqrt(r);

        var main = leakSystem.main;

        main.startSpeed = rate;
        main.startSizeMultiplier = rate * 0.1f;

        float emissionRateValue = rate > 0f ? 60f : 0f;
        var emissionRate = leakSystem.emission;
        var data = cockVoreDick.GetPenetrationData();
        if (data.HasValue) {
            emissionRate.rateOverTime = data.Value.tipIsInside ? 0f : emissionRateValue;
        } else {
            emissionRate.rateOverTime = emissionRateValue;
        }
        
    }
    
    public bool IsVoring() => voreMachine?.IsVoring() ?? false;
}
