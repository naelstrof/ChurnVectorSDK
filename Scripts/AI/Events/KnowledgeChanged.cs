
namespace AI.Events {
    
public class KnowledgeChanged : Event {
    private KnowledgeDatabase.Knowledge knowledge;
    public KnowledgeChanged(KnowledgeDatabase.Knowledge knowledge) {
        this.knowledge = knowledge;
    }
    public KnowledgeDatabase.Knowledge GetKnowledge() => knowledge;
}

}
