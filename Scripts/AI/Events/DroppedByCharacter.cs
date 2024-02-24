namespace AI.Events {
    public class DroppedByCharacter : Event {
        private CharacterBase character;

        public DroppedByCharacter(CharacterBase character) {
            this.character = character;
        }
    }
}