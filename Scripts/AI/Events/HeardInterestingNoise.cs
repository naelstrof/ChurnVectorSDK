using UnityEngine;

namespace AI.Events {
    public class HeardInterestingNoise : Event {
        private Vector3 noisePosition;
        private Vector3 noiseHeardDirection;
        private AudioPack heardSound;
        private float normalizedAudioDistance;
        private CharacterBase owner;
        public HeardInterestingNoise(CharacterBase owner, Vector3 noisePosition, Vector3 noiseHeardDirection, AudioPack heardSound, float normalizedAudioDistance) {
            this.noiseHeardDirection = noiseHeardDirection;
            this.noisePosition = noisePosition;
            this.heardSound = heardSound;
            this.normalizedAudioDistance = normalizedAudioDistance;
            this.owner = owner;
        }

        public Vector3 GetNoisePosition() => noisePosition;
        public Vector3 GetNoiseHeardDirection() => noiseHeardDirection;
        public float GetAudioDistance() => normalizedAudioDistance;
        public AudioPack GetHeardSound() => heardSound;
        public CharacterBase GetOwner() => owner;
    }
}