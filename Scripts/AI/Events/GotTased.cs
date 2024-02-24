
namespace AI.Events {
    public class GotTased : Event {
        private CharacterBase by;
        public GotTased(CharacterBase by) {
            this.by = by;
        }

        public CharacterBase GetTasedBy() => by;
    }
}