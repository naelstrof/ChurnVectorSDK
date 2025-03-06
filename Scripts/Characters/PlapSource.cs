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
        private bool loadedPlap = false;
        private bool loadedSlide = false;
        private float plapTTL = 0f;

        private static AnimationCurve audioFalloff = new() { keys = new Keyframe[] { new(0f, 1f, 0, -3.1f), new(1f, 0f, 0f, 0f) } };
        public void Init(Penetrator dick) {
            penetrator = dick;
            penetrator.penetrated += OnPenetration;

            plapSource = gameObject.AddComponent<AudioSource>();
            plapSource.loop = false;
            plapSource.spatialBlend = 1f;
            plapSource.minDistance = 1f;
            plapSource.maxDistance = 10f;
            plapSource.rolloffMode = AudioRolloffMode.Custom;
            plapSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, audioFalloff);
            plapSource.spread = 30f;
            plapSource.enabled = false;

            moveSource = gameObject.AddComponent<AudioSource>();
            moveSource.transform.position.With(x: moveSource.transform.position.x + 2f);
            moveSource.loop = true;
            moveSource.spatialBlend = 1f;
            moveSource.minDistance = 1f;
            moveSource.maxDistance = 10f;
            moveSource.priority = 129;
            moveSource.rolloffMode = AudioRolloffMode.Custom;
            moveSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, audioFalloff);
            moveSource.spread = 120f;
            moveSource.enabled = false;

            var plapHandle = Addressables.LoadAssetAsync<AudioPack>("Plap");
            plapHandle.Completed += (handle) => {
                plapPack = handle.Result;
                loadedPlap = true;
                Addressables.Release(plapHandle);
            };
            var moveHandle = Addressables.LoadAssetAsync<AudioPack>("FuckableSlide");
            moveHandle.Completed += (handle) => {
                movePack = handle.Result;
                loadedSlide = true;
                Addressables.Release(moveHandle);
            };
        }

        private void OnPenetration(Penetrator penetrator, Penetrable penetrable, Penetrator.PenetrationArgs penetrationArgs, Penetrable.PenetrationResult result) {
            if (!loadedPlap || !loadedSlide) return;     

            float movement = penetrationArgs.penetrationDepth - (lastPenetrationDepth ?? penetrationArgs.penetrationDepth);
            lastPenetrationDepth = penetrationArgs.penetrationDepth;

            moveSource.enabled = true;
            if (Mathf.Abs(movement) < 0.001f || penetrationArgs.penetrationDepth < 0f) {
                return;
            }
            // slide sounds
            if (movement > Mathf.Epsilon && !moveSource.isPlaying) {
                movePack.Play(moveSource);
            }
            moveVolume += Mathf.Abs(movement);
            moveVolume = Mathf.Clamp(moveVolume, 0, 0.6f);

            // plap sounds
            dickLength = penetrator.GetSquashStretchedWorldLength();
            var penetrationDepth = dickLength - penetrationArgs.penetrationDepth;
            if(!hasPlapped && penetrationDepth < 0.1f) {
                plapSource.enabled = true;
                plapPack.PlayOneShot(plapSource);
                plapTTL = plapPack.GetClip().length + 0.5f;
                hasPlapped = true;                
            } else if(penetrationDepth > 0.12f){
                hasPlapped = false;
            }
        }


        private void Update() {
            if (!loadedPlap || !loadedSlide) return;

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
