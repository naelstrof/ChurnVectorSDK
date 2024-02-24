using UnityEngine;

namespace AI.Events {
    public class ShareKnowledge : Event {
        private CharacterDetector to;
        private GameObject about;
        public ShareKnowledge(CharacterDetector to, GameObject about) {
            this.to = to;
            this.about = about;
        }
        public CharacterDetector GetTarget() => to;
        public GameObject GetAbout() => about;
    }
}