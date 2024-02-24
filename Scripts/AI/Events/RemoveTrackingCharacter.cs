
namespace AI.Events {
    public class RemoveTrackingCharacter : Event {
        private CharacterBase other;
        public RemoveTrackingCharacter( CharacterBase other) {
            this.other = other;
        }
        public CharacterBase GetOther() => other;
    }
}