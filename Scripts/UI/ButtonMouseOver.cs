using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler {
    private Vector3 defaultLocalScale;
    private Button attachedButton;
    [SerializeField,SerializeReference,SubclassSelector]
    private GameEventResponse onClick;

    private void Awake() {
        attachedButton = GetComponent<Button>();
        attachedButton.onClick.AddListener(() => {
            onClick?.Invoke(this);
        });
    }

    private void OnEnable() {
        attachedButton = GetComponent<Button>();
        defaultLocalScale = transform.localScale;
    }
    IEnumerator ScaleBack(float scaleDuration) {
        float startTime = Time.unscaledTime;
        while (isActiveAndEnabled && attachedButton.interactable && (startTime + scaleDuration) > Time.unscaledTime ) {
            transform.localScale = Vector3.Lerp(transform.localScale, defaultLocalScale, (Time.unscaledTime-startTime)/scaleDuration);
            yield return null;
        }
        transform.localScale = defaultLocalScale;
    }
    IEnumerator ScaleUp(float scaleDuration) {
        float startTime = Time.unscaledTime;
        while (isActiveAndEnabled && attachedButton.interactable && (startTime + scaleDuration) > Time.unscaledTime ) {
            transform.localScale = Vector3.Lerp(transform.localScale, defaultLocalScale*1.1f, (Time.unscaledTime-startTime)/scaleDuration);
            yield return null;
        }
        transform.localScale = defaultLocalScale*1.1f;
    }

    private void OnDisable() {
        transform.localScale = defaultLocalScale;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (!attachedButton.interactable) {
            return;
        }
        StopAllCoroutines();
        StartCoroutine(ScaleUp(0.3f));
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (!attachedButton.interactable) {
            return;
        }
        StopAllCoroutines();
        StartCoroutine(ScaleBack(0.3f));
    }

    public void OnSelect(BaseEventData eventData) {
        if (!attachedButton.interactable) {
            return;
        }
        StopAllCoroutines();
        StartCoroutine(ScaleUp(0.3f));
    }

    public void OnDeselect(BaseEventData eventData) {
        if (!attachedButton.interactable) {
            return;
        }
        StopAllCoroutines();
        StartCoroutine(ScaleBack(0.3f));
    }
}
