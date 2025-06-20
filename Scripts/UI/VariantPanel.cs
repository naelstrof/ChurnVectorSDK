using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class VariantPanel : MonoBehaviour
{
    [SerializeField] private Image variantPreview;
    [SerializeField] private TMP_Text variantName;
    [SerializeField] private TMP_Text modSource;
    [SerializeField] private Toggle variantToggle;
    private CharacterVariant variant;

    public IEnumerator SetVariant(CharacterVariant variant)
    {
        CanvasGroup group = GetComponent<CanvasGroup>();
        group.alpha = 0;
        yield return variant.LoadCharacterData();

        this.variant = variant;
        variantName.text = variant.GetName();
        variantPreview.sprite = variant.GetIcon();
        modSource.text = variant.GetSource().GetDescription().GetTitle();
        variantToggle.isOn = variant.IsActive(true);

        StartCoroutine(FadeIn(group));
    }

    private IEnumerator FadeIn(CanvasGroup group)
    {
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * 4f;
            group.alpha = t;
            yield return null;
        }
        group.alpha = 1;
    }

    public void ToggleVariant()
    {
        variant.SetVariantActive(variantToggle.isOn);
    }
}
