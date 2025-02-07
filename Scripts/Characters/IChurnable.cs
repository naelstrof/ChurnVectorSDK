using UnityEngine;

public interface IChurnable {
    public float GetVolumeSolid();
    public float GetVolumeChurned();
    public float GetChurnDuration();
    public Sprite GetHeadSprite();
    public Transform transform { get; }
}
