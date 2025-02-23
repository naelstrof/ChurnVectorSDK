using DPG;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Packages.ChurnVectorSDK.Scripts.Characters {
    public class PlapSource : MonoBehaviour{
        private AudioPack plapPack;
        private AudioPack movePack;

        private Penetrator penetrator;
        private float? lastPenetrationDepth;
        private float? dickLength;
        private bool hasPlapped = false;
        private AudioSource plapSource;
        private AudioSource moveSource;

        private float moveVolume = 0f;
        private bool loaded = false;
        private float plapTTL = 0f;

        AsyncOperationHandle<AudioPack> plapHandle;
        AsyncOperationHandle<AudioPack> moveHandle;

        private static AnimationCurve audioFalloff = new() { keys = new Keyframe[] { new(0f, 1f, 0, -3.1f), new(1f, 0f, 0f, 0f) } };
        public void Init(Penetrator dick) {
            penetrator = dick;
            penetrator.penetrated += OnPenetration;

            plapSource = gameObject.AddComponent<AudioSource>();
            plapSource.loop = true;
            plapSource.spatialBlend = 1f;
            plapSource.minDistance = 1f;
            plapSource.maxDistance = 10f;
            plapSource.rolloffMode = AudioRolloffMode.Custom;
            plapSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, audioFalloff);
            plapSource.enabled = false;

            moveSource = gameObject.AddComponent<AudioSource>();
            moveSource.loop = true;
            moveSource.spatialBlend = 1f;
            moveSource.minDistance = 1f;
            moveSource.maxDistance = 10f;
            moveSource.priority = 129;
            moveSource.rolloffMode = AudioRolloffMode.Custom;
            moveSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, audioFalloff);
            moveSource.enabled = false;

            plapHandle = Addressables.LoadAssetAsync<AudioPack>("Plap");
            moveHandle = Addressables.LoadAssetAsync<AudioPack>("FuckableSlide");
        }

        private void OnPenetration(Penetrator penetrator, Penetrable penetrable, Penetrator.PenetrationArgs penetrationArgs, Penetrable.PenetrationResult result) {
            if (!loaded) return;

            plapSource.enabled = true;
            moveSource.enabled = true;

            float movement = penetrationArgs.penetrationDepth - (lastPenetrationDepth ?? penetrationArgs.penetrationDepth);
            lastPenetrationDepth = penetrationArgs.penetrationDepth;
            if (Mathf.Abs(movement) < 0.001f || penetrationArgs.penetrationDepth < 0f) {
                return;
            }
            // slide sounds
            if (movement > Mathf.Epsilon) {
                movePack.Play(moveSource);
            }
            moveVolume += Mathf.Abs(movement);
            moveVolume = Mathf.Clamp(moveVolume, 0, 0.6f);

            // plap sounds
            dickLength = penetrator.GetSquashStretchedWorldLength();
            if(dickLength - penetrationArgs.penetrationDepth < 0.1f) {
                if (!hasPlapped) {
                    plapPack.PlayOneShot(plapSource);
                    plapTTL = plapPack.GetClip().length + 0.5f;
                }
                hasPlapped = true;                
            } else {
                hasPlapped = false;
            }
        }


        private void Update() {
            if (!loaded && plapHandle.IsDone && moveHandle.IsDone) {
                plapPack = plapHandle.Result;
                movePack = moveHandle.Result;
                loaded = true;
            }
            moveVolume = Mathf.MoveTowards(moveVolume, 0f, Time.deltaTime);
            plapTTL = Mathf.MoveTowards(plapTTL, 0f, Time.deltaTime);
            moveSource.volume = moveVolume;
            if (moveVolume == 0f && moveSource.enabled) {
                moveSource.enabled = false;
            }
            if(plapTTL == 0f && plapSource.enabled) {
                plapSource.enabled = false;
            }
        }
    }
}
