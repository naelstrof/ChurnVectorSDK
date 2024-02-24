using UnityEngine;

public class KeepOnScreen : MonoBehaviour {
    [SerializeField] private RectTransform targetRect;
    [SerializeField] private Transform attachedTransform;
    private Vector3 worldPosition;
    protected float alpha = 1f;
    private Vector3 offset = Vector3.up * 0.5f;
    [SerializeField]
    private bool worldPositionUpdates = false;
    
    public void AttachTo(Transform target) {
        attachedTransform = target;
        //transform.SetParent(attachedTransform);
        //transform.position += offset;
        worldPosition = target.position+offset;
    }

    public Vector3 GetAttachPosition() {
        if (worldPositionUpdates) {
            return attachedTransform.position + offset;
        } else {
            return worldPosition;
        }
    }

    protected virtual void LateUpdate() {
        if (attachedTransform == null) {
            return;
        }
        var screenPoint = OrbitCamera.GetCamera().WorldToScreenPoint(GetAttachPosition());
        Vector2 size = targetRect.sizeDelta;
        targetRect.anchoredPosition = new Vector2(Mathf.Clamp(screenPoint.x, size.x*0.5f, Screen.width-size.x*0.5f), Mathf.Clamp(screenPoint.y, size.y*0.5f, Screen.height-size.y*0.5f));
        alpha = Mathf.Clamp01(screenPoint.z);
    }
}
