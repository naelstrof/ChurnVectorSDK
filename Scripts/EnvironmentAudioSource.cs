using System;
using System.Threading.Tasks;
using UnityEngine;

public class EnvironmentAudioSource : InitializationManagerInitialized {
    public override InitializationManager.InitializationStage GetInitializationStage() => InitializationManager.InitializationStage.AfterLevelLoad;

    private AudioSource audioSource;
    private Camera cam;
    private bool run = false;

    public override Task OnInitialized() {
        cam = OrbitCamera.GetCamera();
        run = true;
        return base.OnInitialized();
    }
    void Awake() {
        audioSource = GetComponent<AudioSource>();
        if(audioSource != null && audioSource.outputAudioMixerGroup == null) {
            Debug.LogWarning("Audio source is missing output group, disabling");
            audioSource.enabled = false;
            this.enabled = false;
        }
    }

    void Update() {
        if(run) {
            float distance = Vector3.Distance(cam.transform.position, transform.position);
            if (!audioSource.enabled && distance <= audioSource.maxDistance) {
                audioSource.enabled = true;
            } else if (audioSource.enabled && distance > audioSource.maxDistance) {
                audioSource.enabled = false;
            }
        }   
    }
}
