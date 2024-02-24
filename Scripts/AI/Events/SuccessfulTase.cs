
namespace AI.Events {
    public class SuccessfulTase : Event {
        private CharacterBase target;
        public SuccessfulTase(CharacterBase target) {
            this.target = target;
        }

        public CharacterBase GetTarget() => target;
    }
}