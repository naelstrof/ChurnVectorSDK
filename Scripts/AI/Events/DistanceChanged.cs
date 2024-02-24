
namespace AI.Events {
    
public class DistanceChanged : Event {
    private float distance;
    public DistanceChanged(float distance) {
        this.distance = distance;
    }

    public float GetDistance() => distance;

    public void SetDistance(float distance) {
        this.distance = distance;
    }
}

}
