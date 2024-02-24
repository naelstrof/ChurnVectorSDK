
namespace AI.Events {
    public class AddTrackingCharacter : Event {
        private CharacterBase other;
        public AddTrackingCharacter( CharacterBase other) {
            this.other = other;
        }
        public CharacterBase GetOther() => other;
    }
}