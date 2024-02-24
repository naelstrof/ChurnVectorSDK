using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class ObjectiveDisplay : MonoBehaviour {
    [SerializeField] private TMP_Text label;
    [SerializeField] private TMP_Text description;
    [SerializeField] private UnityEngine.UI.Image completed;
    [SerializeField] private UnityEngine.UI.Image failed;
    private Animator animator;
    private string labelText;
    private string descriptionText;
    private static readonly int ShowDescription = Animator.StringToHash("ShowDescription");

    private void Awake() {
        animator = GetComponent<Animator>();
        //SetCompleted(false);
        //SetFailed(false);
    }

    public void SetLabelAndDescription(LocalizedString label, LocalizedString description) {
        labelText = label.GetLocalizedString();
        descriptionText = description.GetLocalizedString();
        var localizeStringEvent = this.label.gameObject.GetComponent<LocalizeStringEvent>();
        if (localizeStringEvent != null) {
            localizeStringEvent.StringReference = label;
        }
        
        var localizeStringEventDescription = this.description.gameObject.GetComponent<LocalizeStringEvent>();
        if (localizeStringEventDescription != null) {
            localizeStringEventDescription.StringReference = description;
        }

        this.label.text = labelText;
        this.description.text = descriptionText;
    }

    public void SetCompleted(bool completed) {
        this.completed.enabled = completed;
        label.text = completed ? $"<s>{labelText}</s>" : labelText;
        label.color = completed ? Color.gray : Color.white;
    }
    public void SetFailed(bool failed) {
        this.failed.enabled = failed;
        label.text = $"<s>{labelText}</s>";
        label.text = failed ? $"<s>{labelText}</s>" : labelText;
        label.color = failed ? Color.gray : Color.white;
    }

    public void SetShowDescription(bool showDescription) {
        animator.SetBool(ShowDescription, showDescription);
    }
}
