using System.Collections.Generic;
using System.Text;
using ActorActions;
using AI.Actions;
using TMPro;
using UnityEngine;
using Event = AI.Events.Event;

namespace AI {
    
[System.Serializable]
public abstract class Actor : InputGenerator {
    [SerializeField] private bool debug;
    [SerializeField, SerializeReference, SubclassSelector]
    private BaseAction startingAction;
    private static ActionTransitionContinue continueTransition = new();

    public bool IsCop() => startingAction is ActionBeACop or ActionBeAPatrollingCop;

    private TMP_Text debugText;

    private Vector3 wishDirection;
    private Vector3 lookDirection;
    private bool jump;
    private bool sprint;
    private Stack<Action> actionStack;
    private StringBuilder thoughtBuilder;
    private int eventTransitions = 0;
    private bool aimingWeapon;

    public void SetWishDirection(Vector3 value) => wishDirection = value;
    public void SetLookDirection(Vector3 value) => lookDirection = value;
    public void SetJump(bool value) => jump = value;
    public void SetSprint(bool value) => sprint = value;

    public GameObject gameObject { private set; get; }
    public Transform transform { private set; get; }

    private ActionTransition ProcessTransition(ActionTransition transition, int depth = 0) {
        if (depth > 16) {
            Debug.LogWarning($"Tried to process too many transitions in one tick, infinite loop? Offending Action {actionStack.Peek()}", gameObject);
            return new ActionTransitionContinue();
        }

        if (transition is ActionTransitionWithReason transitionWithReason) {
            thoughtBuilder.Append($"\t{transitionWithReason.GetTransitionReason()}\n");
        }
        if (transition is ActionTransitionWithTarget transitionWithTarget) {
            thoughtBuilder.Append($"->{transitionWithTarget.GetTargetAction().GetType().Name}\n");
        }
        debugText.text = thoughtBuilder.ToString();
        switch (transition) {
            case ActionTransitionChangeTo transitionChangeTo: {
                Action currentAction = actionStack.Pop();
                currentAction.OnEnd(this);
                actionStack.Push(transitionChangeTo.GetTargetAction());
                return ProcessTransition(actionStack.Peek().OnStart(this), depth+1);
            }
            case ActionTransitionSuspendFor transitionSuspendFor: {
                actionStack.Peek().OnSuspend(this);
                actionStack.Push(transitionSuspendFor.GetTargetAction());
                return ProcessTransition(actionStack.Peek().OnStart(this), depth+1);
            }
            case ActionTransitionDone: {
                Action currentAction = actionStack.Pop();
                currentAction.OnEnd(this);
                if (actionStack.Count == 0) {
                    actionStack.Push(startingAction);
                    return ProcessTransition(actionStack.Peek().OnStart(this), depth+1);
                }
                thoughtBuilder.Append($"->{actionStack.Peek().GetType().Name}\n");
                debugText.text = thoughtBuilder.ToString();
                return ProcessTransition(actionStack.Peek().OnResume(this), depth + 1);
            }
            case ActionTransitionContinue: return transition;
            default: throw new UnityException($"Unknown transition {transition}");
        }
    }

    public virtual ActionEventResponse RaiseEvent(Event e) {
        Action reactingAction = null;
        ActionEventResponse response = null;
        foreach (var action in actionStack) {
            response = action.OnReceivedEvent(this, e);
            if (response is ActionEventResponseIgnore) {
                continue;
            }
            reactingAction = action;
            break;
        }
        
        eventTransitions++;
        if (eventTransitions > 16) {
            Debug.LogWarning($"Tried to process too many event transitions in one tick, infinite loop? Offending Action {reactingAction}", gameObject);
            return new ActionEventResponseIgnore();
        }

        if (reactingAction != null) {
            while (actionStack.Peek() != reactingAction) {
                actionStack.Pop().OnEnd(this);
            }
        }

        switch (response) {
            case ActionEventResponseTransition responseTransition:
                var transition = responseTransition.GetActionTransition();
                if (transition is ActionTransitionContinue) {
                    actionStack.Peek().OnResume(this);
                } else {
                    ProcessTransition(transition);
                }
                break;
        }

        return response;
    }

    public void Update() {
        eventTransitions = 0;
        if ((debug && Application.isEditor) && !debugText.enabled) {
            debugText.enabled = true;
        }

        if ((!debug || !Application.isEditor) && debugText.enabled) {
            debugText.enabled = false;
        }

        if (thoughtBuilder.Length > 300) {
            thoughtBuilder.Remove(0, Mathf.Max(thoughtBuilder.Length - 300, 0));
        }

        if (actionStack.Count == 0) {
            actionStack.Push(startingAction);
            ProcessTransition(startingAction.OnStart(this));
        }
        ProcessTransition(actionStack.Peek().Update(this));
    }

    public override void Initialize(GameObject character) {
        thoughtBuilder ??= new StringBuilder();
        actionStack ??= new Stack<Action>();
        gameObject = character;
        transform = character.transform;
        if (debugText == null) {
            GameObject obj = new GameObject("Debug Thoughts", typeof(TextMeshPro));
            obj.transform.SetParent(character.transform);
            obj.transform.position = character.transform.position + Vector3.up * 2f;
            obj.transform.localScale = Vector3.one * 0.05f;

            debugText = obj.GetComponent<TMP_Text>();
            debugText.fontSize = 11;
            debugText.rectTransform.sizeDelta =
                new Vector2(debugText.rectTransform.sizeDelta.x * 4f, debugText.rectTransform.sizeDelta.y);
            debugText.alignment = TextAlignmentOptions.Center;
        }
    }

    public override void CleanUp() {
    }

    public override Vector3 GetWishDirection() {
        return wishDirection;
    }

    public override bool GetJumpInput() {
        return jump;
    }

    public override Vector3 GetLookDirection() {
        return lookDirection;
    }

    public override float GetCrouchInput() {
        return 0f;
    }

    public override bool GetSprint() {
        return sprint;
    }

    public abstract KnowledgeDatabase.Knowledge GetKnowledgeOf(GameObject target);

    public abstract bool GetGrabbed();

    public void OverrideActionNow(Action action) {
        while (actionStack.Count > 0) {
            actionStack.Pop().OnEnd(this);
        }
        actionStack.Push(action);
        ProcessTransition(actionStack.Peek().OnStart(this));
    }

    public void SetBaseAction(BaseAction baseAction) {
        startingAction = baseAction;
        OverrideActionNow(baseAction);
    }

    public void SetAimingWeapon(bool aimingWeapon) {
        this.aimingWeapon = aimingWeapon;
    }

    public override bool GetAimingWeapon() {
        return aimingWeapon;
    }

    public abstract CharacterBase GetCharacter();

    public abstract bool ShouldUse(IInteractable interactable);
    public abstract bool IsPointVisible(Vector3 point);

    public abstract bool IsVisible(GameObject target);

    public abstract bool IsDirectlyVisible(GameObject target);
    public abstract void GetNearbyCops(Vector3 point, List<Civilian> cops);
    public abstract List<Civilian> GetAllCops();
    public abstract bool UseInteractable(IInteractable interactable);
    public abstract IInteractable GetRandomInteractable();
    public abstract bool IsStillUsing(IInteractable target);
    public abstract void StopUsingAnything();
    public abstract int GetTaseCount();
    public abstract void KeepMemoryAlive(GameObject target, bool keepAlive);
    public abstract CharacterBase GetTaseTarget();
    public abstract bool TryGetAimPrediction(CharacterBase target, float delay, out Vector3 aimDirection);

}

}