
public class Modifier {
    private float multiplier;
    public Modifier(float multiplier) {
        this.multiplier = multiplier;
    }
    public void SetMultiplier(float multiplier) {
        this.multiplier = multiplier;
    }
    public float GetMultiplier() => multiplier;
}
