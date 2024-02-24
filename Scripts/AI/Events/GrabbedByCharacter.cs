namespace AI.Events {
    public class GrabbedByCharacter : Event {
        private CharacterBase character;

        public GrabbedByCharacter(CharacterBase character) {
            this.character = character;
        }

        public CharacterBase GetCharacter() => character;
    }
}