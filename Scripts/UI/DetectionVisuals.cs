using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DetectionVisuals : MonoBehaviour {
    [SerializeField] private AudioPack windRush;
    [SerializeField] private AnimationCurve volumeCurve;
    [SerializeField] private Sprite arcSprite;
    [SerializeField] private Canvas targetCanvas;
    
    private Dictionary<CharacterBase, Detection> detections;

    public class Detection {
        private AudioSource source;
        //private Disc arc;
        private Image arc;
        private CharacterBase target;
        private float lastTime;
        private const float memory = 0.1f;
        private float volume;
        private AnimationCurve volumeCurve;
        private const float fullVolume = 0.4f;

        public Detection(CharacterBase target, Sprite arcSprite, Canvas targetCanvas, AudioPack woosh, AnimationCurve volumeCurve) {
            this.volumeCurve = volumeCurve;
            var obj = new GameObject("WooshingSource", typeof(AudioSource));
            obj.transform.SetParent(target.transform);
            obj.transform.position = target.transform.position;
            source = obj.GetComponent<AudioSource>();
            source.spatialBlend = 1f;
            source.minDistance = 1f;
            source.maxDistance = 50f;
            source.volume = 0f;
            source.rolloffMode = AudioRolloffMode.Custom;
            source.SetCustomCurve(AudioSourceCurveType.SpatialBlend, new AnimationCurve { keys = new[] { new Keyframe(0f, 1f), new Keyframe(1f, 1f) } });
            source.loop = true;
            source.clip = woosh.GetClip();
            source.outputAudioMixerGroup = woosh.GetAudioMixerGroup();
            source.Pause();
            lastTime = Time.time;
            
            var arcobj = new GameObject("DetectionArc", typeof(Image));
            arcobj.transform.SetParent(targetCanvas.transform);
            arc = arcobj.GetComponent<Image>();
            arc.sprite = arcSprite;
            arc.preserveAspect = true;
            arc.rectTransform.anchorMin = Vector3.zero;
            arc.rectTransform.anchorMax = Vector3.one;
            arc.rectTransform.anchoredPosition = Vector3.zero;
            arc.color = Color.white.With(a: 0f);
            this.target = target;
        }


        public float GetVolume() => volume;
        public void SetMute(bool muted) {
            source.volume = muted ? 0f : volumeCurve.Evaluate(volume)*fullVolume;
        }
        public void SetVisible(bool visible) {
            arc.enabled = visible;
        }

        public void OnSee(KnowledgeDatabase.Knowledge knowledge) {
            // Been spotted!
            if (knowledge.GetKnowledgeLevel() != KnowledgeDatabase.KnowledgeLevel.Ignorant) {
                lastTime = Time.time - memory*2f;
                return;
            } else {
                lastTime = Time.time;
            }

            if (!source.isPlaying && knowledge.awareness > 0f) {
                source.Play();
            }
            float awareness = Mathf.Clamp01(knowledge.awareness);

            Vector3 dirToDetector = (target.transform.position - OrbitCamera.GetPlayerIntendedPosition()).normalized;
            Vector3 screenDir = Vector3.ProjectOnPlane(dirToDetector, OrbitCamera.GetPlayerIntendedRotation() * Vector3.forward);
            float angle = Vector3.SignedAngle(OrbitCamera.GetPlayerIntendedRotation() * Vector3.right, screenDir.normalized, OrbitCamera.GetPlayerIntendedRotation() * Vector3.forward);

            float lookingRightAt = screenDir.magnitude;
            volume = Mathf.MoveTowards(volume,awareness, Time.deltaTime*8f);
            arc.rectTransform.sizeDelta = Vector2.one * (20f + volume * 20f);
            arc.color = arc.color.With(a: Mathf.Clamp01(knowledge.awareness+knowledge.awarenessBuffer*2f)*lookingRightAt*lookingRightAt);
            arc.rectTransform.rotation = Quaternion.Euler(0f, 0f, angle-90f);
            source.volume = volumeCurve.Evaluate(volume)*fullVolume;
            source.pitch = 0.9f+volume*1.9f;
        }

        public void Update() {
            if (Time.time - lastTime < memory) {
                return;
            }

            volume = Mathf.MoveTowards(volume, 0f, Time.deltaTime);
            source.volume = volumeCurve.Evaluate(volume)*fullVolume;
            if (source.isPlaying && Mathf.Approximately(volume, 0f)) {
                source.Pause();
            }
            arc.color = arc.color.With(a:Mathf.MoveTowards(arc.color.a, 0f, Time.deltaTime));
        }

        public void Destroy() {
            if (arc != null) {
                Object.Destroy(arc.gameObject);
            }
            if (source != null) {
                Object.Destroy(source.gameObject);
            }
        }
    }

    private void Awake() {
        detections = new Dictionary<CharacterBase, Detection>();
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene arg0) {
        foreach (var pair in detections) {
            if (pair.Value == null) {
                continue;
            }

            pair.Value.Destroy();
        }
        detections = new Dictionary<CharacterBase, Detection>();
    }

    private void OnEnable() {
        CharacterBase.playerEnabled += OnPlayerEnabled;
        CharacterBase.playerDisabled += OnPlayerDisabled;
        detections = new Dictionary<CharacterBase, Detection>();
    }

    private void OnPlayerDisabled(CharacterBase player) {
        player.seen -= OnSeen;
    }

    private void OnPlayerEnabled(CharacterBase player) {
        player.seen += OnSeen;
    }

    private void OnDisable() {
        CharacterBase.playerEnabled -= OnPlayerEnabled;
        CharacterBase.playerDisabled -= OnPlayerDisabled;
        foreach (var pair in detections) {
            if (pair.Value == null) {
                continue;
            }

            pair.Value.Destroy();
        }
    }

    private void Update() {
        Detection maxVolumeDetection = null;
        float maxVolume = float.MinValue;
        foreach (var pair in detections) {
            pair.Value.Update();
            if (pair.Value.GetVolume() > maxVolume) {
                maxVolumeDetection = pair.Value;
                maxVolume = pair.Value.GetVolume();
            }
        }

        bool shouldDoVisuals =
            KnowledgeDatabase.GetMaxPlayerKnowledgeLevel() == KnowledgeDatabase.KnowledgeLevel.Ignorant;
        foreach (var pair in detections) {
            pair.Value.SetVisible(shouldDoVisuals);
            pair.Value.SetMute(!shouldDoVisuals || pair.Value != maxVolumeDetection);
        }
    }

    private void OnSeen(KnowledgeDatabase.Knowledge knowledge, CharacterBase by) {
        if (!enabled) {
            return;
        }

        if (!detections.ContainsKey(by)) {
            detections.Add(by, new Detection(by, arcSprite, targetCanvas, windRush, volumeCurve));
        }
        var detection = detections[by];
        detection.OnSee(knowledge);
    }
}
