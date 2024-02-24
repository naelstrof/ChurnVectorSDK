using System.Collections.Generic;
using Naelstrof.Easing;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class PhysicsAudio : MonoBehaviour {
    private const double minWaitTime = 0.18f;
    private const float minScrapeSpeed = 0.1f;
    private const float minImpactMagnitude = 0.05f;
    private const float maxImpactMagnitude = 1f;
    
    private Dictionary<PhysicsMaterialExtension.ImpactInfo, double> voices;
    private AudioSource impactSource;
    private AudioSource scrapeSource;
    private AudioPack currentScrapePack;
    private double stopTime;
    private float scrapeVolumeTarget;
    private int contacts;
    private List<PhysicsAudioListener> listeners;
    private static ContactPoint[] contactPoints = new ContactPoint[8];

    private class PhysicsAudioListener : MonoBehaviour {
        public delegate void OnCollisionEvent(Rigidbody body, Collision collision);
        public event OnCollisionEvent onCollisionEnter;
        public event OnCollisionEvent onCollisionStay;
        public event OnCollisionEvent onCollisionExit;
        private Rigidbody body;

        void Awake() {
            body = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision) {
            onCollisionEnter?.Invoke(body, collision);
        }
        private void OnCollisionStay(Collision collision) {
            onCollisionStay?.Invoke(body, collision);
        }
        private void OnCollisionExit(Collision collision) {
            onCollisionExit?.Invoke(body, collision);
        }
    }

    void OnEnable() {
        voices = new Dictionary<PhysicsMaterialExtension.ImpactInfo, double>();
        Pauser.pauseChanged += OnPaused;
        contacts = 0;
        if (impactSource != null) {
            Destroy(impactSource);
        }

        impactSource = gameObject.AddComponent<AudioSource>();
        impactSource.maxDistance = 25f;
        impactSource.minDistance = 0.5f;
        impactSource.spatialBlend = 1f;
        impactSource.loop = false;
        impactSource.rolloffMode = AudioRolloffMode.Linear;
        impactSource.playOnAwake = false;
        impactSource.enabled = false;
        
        if (scrapeSource != null) {
            Destroy(scrapeSource);
        }
        scrapeSource = gameObject.AddComponent<AudioSource>();
        scrapeSource.maxDistance = 25f;
        scrapeSource.minDistance = 0.5f;
        scrapeSource.spatialBlend = 1f;
        scrapeSource.loop = true;
        scrapeSource.rolloffMode = AudioRolloffMode.Linear;
        scrapeSource.playOnAwake = false;
        scrapeSource.enabled = false;
        scrapeSource.priority = 256;
        listeners = new List<PhysicsAudioListener>();
        foreach (var body in GetComponentsInChildren<Rigidbody>()) {
            var listener = body.gameObject.AddComponent<PhysicsAudioListener>();
            listener.onCollisionEnter += CustomCollisionEnter;
            listener.onCollisionStay += CustomCollisionStay;
            listener.onCollisionExit += CustomCollisionExit;
            listeners.Add(listener);
        }
    }

    void OnPaused(bool paused) {
        if (paused) {
            scrapeSource.enabled = false;
        }
    }

    private void OnDisable() {
        foreach (var listener in listeners) {
            Destroy(listener);
        }

        Pauser.pauseChanged -= OnPaused;
        Destroy(impactSource);
        Destroy(scrapeSource);
        listeners.Clear();
    }

    void DoImpact(ImpactAnalysis impactAnalysis, ContactPoint targetContactPoint) {
        if (!isActiveAndEnabled) {
            return;
        }

        if (!GameManager.GetPhysicsMaterialExtensionDatabase().TryGetImpactInfo(targetContactPoint,
                PhysicsMaterialExtension.PhysicsResponseType.Impact, out PhysicsMaterialExtension.ImpactInfo impactInfo)) {
            return;
        }
        // Audio dispatch
        AudioPack impactPack = impactInfo.soundEffect;
        if (impactPack == null) {
            return;
        }

        if (!voices.ContainsKey(impactInfo)) {
            voices.Add(impactInfo, Time.timeAsDouble);
        }

        if (Time.timeAsDouble < voices[impactInfo] + minWaitTime) {
            return;
        }
        
        impactSource.enabled = true;
        voices[impactInfo] = Time.timeAsDouble;
        float volume = Easing.Cubic.Out(0.02f + Mathf.Clamp01((impactAnalysis.GetImpactMagnitude() - minImpactMagnitude) / maxImpactMagnitude));
        AudioClip clip = impactPack.PlayOneShot(impactSource, volume);
        // Double.max
        stopTime = stopTime < Time.timeAsDouble + clip.length ? Time.timeAsDouble + clip.length : stopTime;

        if (impactInfo.visualEffects == null || impactInfo.visualEffects.Count <= 0) {
            return;
        }
        GameObject visualEffectGameObject = new GameObject("TemporaryVFX", typeof(VisualEffect));
        visualEffectGameObject.transform.SetPositionAndRotation(targetContactPoint.point, Quaternion.FromToRotation(Vector3.up, targetContactPoint.normal));
        VisualEffect visualEffect = visualEffectGameObject.GetComponent<VisualEffect>();
        visualEffect.visualEffectAsset = impactInfo.visualEffects[Random.Range(0, impactInfo.visualEffects.Count)];
        visualEffect.Play();
        Destroy(visualEffectGameObject, 3f);
    }

    void DoScrape(ContactPoint point) {
        contacts++;
        // Ignore self scraping
        if (point.otherCollider.GetComponentInParent<PhysicsAudio>() ==
            point.thisCollider.GetComponentInParent<PhysicsAudio>()) {
            return;
        }

        if (GameManager.GetPhysicsMaterialExtensionDatabase().TryGetImpactInfo(point,
                PhysicsMaterialExtension.PhysicsResponseType.Scrape, out PhysicsMaterialExtension.ImpactInfo scrapeInfo)) {
            if (currentScrapePack != scrapeInfo.soundEffect || !scrapeSource.enabled) {
                currentScrapePack = scrapeInfo.soundEffect;
                scrapeSource.enabled = true;
                currentScrapePack.Play(scrapeSource);
                scrapeSource.volume = 0f;
            }
        }
    }

    void CustomCollisionEnter(Rigidbody body, Collision collision) {
        int count = collision.GetContacts(contactPoints);
        ImpactAnalysis impactAnalysis = new ImpactAnalysis(body, collision);
        DoScrape(contactPoints[0]);
        for(int i=0;i<count;i++) {
            DoImpact(impactAnalysis, contactPoints[i]);
        }
    }

    void Update() {
        scrapeVolumeTarget = Mathf.MoveTowards(scrapeVolumeTarget, 0f, Time.deltaTime*4f);
        scrapeSource.volume = Mathf.MoveTowards(scrapeSource.volume, scrapeVolumeTarget, Time.deltaTime*4f);
        if (scrapeSource.volume == 0f && contacts == 0 && scrapeSource.enabled) {
            scrapeSource.enabled = false;
        }

        if (Time.timeAsDouble > stopTime && impactSource.enabled) {
            impactSource.enabled = false;
        }
    }

    void CustomCollisionStay(Rigidbody body, Collision collision) {
        if (!scrapeSource.enabled) {
            return;
        }
        // Ignore self scraping
        ContactPoint point = collision.GetContact(0);
        if (point.otherCollider.GetComponentInParent<PhysicsAudio>() ==
            point.thisCollider.GetComponentInParent<PhysicsAudio>()) {
            return;
        }
        scrapeVolumeTarget = Mathf.Max(Easing.Cubic.In(Mathf.Clamp01(collision.relativeVelocity.magnitude - minScrapeSpeed))*currentScrapePack.GetVolume(), scrapeVolumeTarget);
    }

    void CustomCollisionExit(Rigidbody body, Collision collision) {
        contacts--;
        if (contacts == 0) {
            scrapeVolumeTarget = 0f;
        }
    }
}
