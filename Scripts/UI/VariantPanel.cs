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

    public void SetVariant(CharacterVariant variant)
    {
        this.variant = variant;
        variantName.text = variant.GetName();
        variantPreview.sprite = variant.GetIcon();
        modSource.text = variant.GetSource().GetDescription().GetTitle();
        variantToggle.isOn = variant.IsActive();
    }

    public void ToggleVariant()
    {
        variant.SetVariantActive(variantToggle.isOn);
    }
}
