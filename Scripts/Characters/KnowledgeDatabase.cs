using System.Collections.Generic;
using UnityEngine;

public class KnowledgeDatabase {
    public delegate void KnowledgeChangedAction(KnowledgeLevel lastKnowledgeLevel, Knowledge knowledge);
    public delegate void KnowledgeLevelChangedAction(KnowledgeLevel knowledgeLevel);
    public event KnowledgeChangedAction knowledgeLevelChanged;
    public event KnowledgeChangedAction awarenessChanged;
    private List<Knowledge> database;
    private CharacterBase owner;
    private static KnowledgeLevel maxPlayerKnowledge;

    public void ForgetImmediately(GameObject target) {
        for (int i = 0; i < database.Count; i++) {
            if (database[i].target == target) {
                database.RemoveAt(i);
                break;
            }
        }

        if (target.TryGetComponent(out CharacterBase player) && player.IsPlayer()) {
            overallPlayerKnowledge.Remove(this);
        }
    }
    public static KnowledgeLevel GetMaxPlayerKnowledgeLevel() {
        int maxLevel = (int)KnowledgeLevel.Ignorant;
        foreach (var pair in overallPlayerKnowledge) {
            if (pair.Key.owner.IsGrabbed()) {
                continue;
            }
            maxLevel = Mathf.Max(maxLevel, (int)pair.Value);
        }

        return (KnowledgeLevel)maxLevel;
    }

    public static void ForcePoll() {
        var max = GetMaxPlayerKnowledgeLevel();
        if (maxPlayerKnowledge != max) {
            maxPlayerKnowledge = max;
            globalKnowledgeLevelChanged?.Invoke(max);
        }
    }

    public static event KnowledgeLevelChangedAction globalKnowledgeLevelChanged;

    private static Dictionary<KnowledgeDatabase, KnowledgeLevel> overallPlayerKnowledge = new();
    //private const float memorySpan = 15f;

    public KnowledgeDatabase(CharacterBase owner) {
        this.owner = owner;
        database = new List<Knowledge>();
    }

    public enum KnowledgeLevel {
        Ignorant = 0,
        Investigative = 1,
        Alert = 2,
    }
    public struct Knowledge {
        public CharacterBase owner;
        public GameObject target;
        public float awareness;
        private float lastSeenTime;
        public float awarenessBuffer;

        public bool CanRemember() => Time.time - lastSeenTime < GetMemorySpan() || keepAlive;
        private float GetMemorySpan() {
            switch (GetKnowledgeLevel()) {
                default:
                case KnowledgeLevel.Ignorant: return 2.5f;
                case KnowledgeLevel.Investigative: return 3.5f;
                case KnowledgeLevel.Alert: return 4f;
            }
        }

        public bool HasLastKnownPosition() => Time.time-lastSeenTime<GetMemorySpan();
        public bool HasLastKnownDirection() => lastSeenTime != 0;
        private Vector3 lastKnownPosition;
        private Vector3 lastKnownMoveDirection;
        private bool keepAlive;

        public bool TryGetLastKnownPosition(out Vector3 position) {
            position = lastKnownPosition;
            return HasLastKnownPosition();
        }
        public bool TryGetLastKnownDirection(out Vector3 direction) {
            direction = lastKnownMoveDirection;
            return HasLastKnownDirection();
        }
        
        public void SetLastKnownPosition(Vector3 position) {
            Vector3 dir = position - lastKnownPosition;
            if (dir.magnitude != 0) {
                lastKnownMoveDirection = dir.normalized;
            }
            lastKnownPosition = position;
            lastSeenTime = Time.time;
        }
        public void KeepMemoryAlive(bool keepAlive) {
            this.keepAlive = keepAlive;
        }
        public KnowledgeLevel GetKnowledgeLevel() {
            return (KnowledgeLevel)Mathf.Max(Mathf.FloorToInt(awareness),0);
        }

        public Knowledge(CharacterBase owner, GameObject target) {
            this.owner = owner;
            this.target = target;
            awareness = 0f;
            awarenessBuffer = 0f;
            lastKnownPosition = Vector3.zero;
            lastKnownMoveDirection = Vector3.forward;
            lastSeenTime = Time.time-3.1f;
            keepAlive = false;
        }
    }

    public void OnEnable() {
        overallPlayerKnowledge.Add(this, KnowledgeLevel.Ignorant);
    }

    public void OnDisable() {
        overallPlayerKnowledge.Remove(this);
        ForcePoll();
    }

    private void HandleKnowledgeLevelChange(ref Knowledge knowledge, KnowledgeLevel lastLevel) {
        if (lastLevel == knowledge.GetKnowledgeLevel()) {
            return;
        }

        if (knowledge.target == CharacterBase.GetPlayer().gameObject) {
            overallPlayerKnowledge[this] = knowledge.GetKnowledgeLevel();
        }

        ForcePoll();
        knowledgeLevelChanged?.Invoke(lastLevel, knowledge);
    }

    public void Update() {
        for (int i=0;i<database.Count;i++) {
            var knowledge = database[i];
            //if (database[i].target == null) {
                //ForgetImmediately(database[i].target);
                //return;
            //}
            KnowledgeLevel lastLevel = knowledge.GetKnowledgeLevel();
            float oldAwareness = knowledge.awareness;
            if (!knowledge.CanRemember()) {
                knowledge.awarenessBuffer = 0f;
                knowledge.awareness = Mathf.MoveTowards(knowledge.awareness, 0f, Time.deltaTime * 0.2f);
            }
            HandleKnowledgeLevelChange(ref knowledge, lastLevel);
            if (!Mathf.Approximately(oldAwareness, knowledge.awareness)) {
                awarenessChanged?.Invoke(lastLevel, knowledge);
            }
            database[i] = knowledge;
        }
    }

    public Knowledge GetKnowledge(GameObject target) {
        for (int i = 0; i < database.Count; i++) {
            if (database[i].target == target) {
                return database[i];
            }
        }
        database.Add(new Knowledge(owner, target));
        return database[^1];
    }

    public void ReceiveKnowledge(GameObject about, Knowledge newKnowledge) {
        int targetIndex = 0;
        bool searchFound = false;
        for (int i = 0; i < database.Count; i++) {
            if (database[i].target == about) {
                targetIndex = i;
                searchFound = true;
                break;
            }
        }

        if (!searchFound) {
            database.Add(new Knowledge(owner,about));
            targetIndex = database.Count - 1;
        }

        var knowledge = database[targetIndex];
        KnowledgeLevel lastLevel = knowledge.GetKnowledgeLevel();
        
        knowledge.awareness = Mathf.Max(knowledge.awareness, newKnowledge.awareness);
        newKnowledge.TryGetLastKnownPosition(out Vector3 position);
        knowledge.SetLastKnownPosition(position);
        database[targetIndex] = knowledge;
        
        HandleKnowledgeLevelChange(ref knowledge, lastLevel);
        awarenessChanged?.Invoke(lastLevel, knowledge);
        if (about.TryGetComponent(out CharacterBase character)) {
            character.GotSeen(knowledge, owner);
        }
    }

    public void KeepMemoryAlive(GameObject target, bool keepAlive) {
        int targetIndex = 0;
        bool searchFound = false;
        for (int i = 0; i < database.Count; i++) {
            if (database[i].target == target) {
                targetIndex = i;
                searchFound = true;
                break;
            }
        }
        if (!searchFound) {
            database.Add(new Knowledge(owner, target));
            targetIndex = database.Count - 1;
        }
        var knowledge = database[targetIndex];
        knowledge.KeepMemoryAlive(keepAlive);
        database[targetIndex] = knowledge;
    }

    public Knowledge AddAwareness(GameObject target, float delta, KnowledgeLevel maxKnowledgeLevel, Vector3? awarenessPosition = null) {
        int targetIndex = 0;
        bool searchFound = false;
        for (int i = 0; i < database.Count; i++) {
            if (database[i].target == target) {
                targetIndex = i;
                searchFound = true;
                break;
            }
        }

        if (!searchFound) {
            database.Add(new Knowledge(owner, target));
            targetIndex = database.Count - 1;
        }

        var knowledge = database[targetIndex];

        if (awarenessPosition.HasValue) {
            knowledge.SetLastKnownPosition(awarenessPosition.Value);
        }
        if (delta == 0f) {
            database[targetIndex] = knowledge;
            return knowledge;
        }

        if (knowledge.awarenessBuffer < CharacterDetector.awarenessBuffer) {
            knowledge.awarenessBuffer += delta;
            if (target.TryGetComponent(out CharacterBase chara)) {
                chara.GotSeen(knowledge, owner);
            }
            database[targetIndex] = knowledge;
            return knowledge;
        }
        
        float maxAwareness;
        switch (maxKnowledgeLevel) {
            case KnowledgeLevel.Ignorant:
                maxAwareness = 0.5f;
                break;
            case KnowledgeLevel.Investigative:
                maxAwareness = 1.1f;
                break;
            default:
            case KnowledgeLevel.Alert:
                maxAwareness = 2.1f;
                break;
        }
        if (knowledge.awareness > maxAwareness) {
            database[targetIndex] = knowledge;
            return knowledge;
        }

        KnowledgeLevel lastLevel = knowledge.GetKnowledgeLevel();
        float oldAwareness = knowledge.awareness;
        knowledge.awareness = Mathf.Min(knowledge.awareness+delta, maxAwareness);
        if (Mathf.Approximately(oldAwareness,knowledge.awareness)) {
            database[targetIndex] = knowledge;
            return knowledge;
        }

        HandleKnowledgeLevelChange(ref knowledge, lastLevel);
        awarenessChanged?.Invoke(lastLevel, knowledge);
        if (target.TryGetComponent(out CharacterBase character)) {
            character.GotSeen(knowledge, owner);
        }

        database[targetIndex] = knowledge;
        return knowledge;
    }
}
