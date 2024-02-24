
namespace AI.Events {
    public class Shoved : Event {
        private CharacterBase other;
        private ImpactAnalysis impactAnalysis;
        public Shoved( CharacterBase other, ImpactAnalysis impactAnalysis) {
            this.other = other;
            this.impactAnalysis = impactAnalysis;
        }

        public ImpactAnalysis GetImpactAnalysis() => impactAnalysis;
        public CharacterBase GetOther() => other;
    }
}