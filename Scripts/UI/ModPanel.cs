using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class ModPanel : MonoBehaviour
{
    [SerializeField] private Image modImage;
    [SerializeField] private TMP_Text modTitle;
    [SerializeField] private Toggle modToggle;
    [SerializeField] private Button detailsButton;
    private Mod mod;

    public void SetMod(Mod mod)
    {
        this.mod = mod;
        ModDescription modDesc = mod.GetDescription();
        modImage.sprite = modDesc.GetPreviewImage();
        modTitle.text = modDesc.GetTitle();
        modToggle.isOn = modDesc.IsActive();
    }

    public Button DetailsButton() => detailsButton;

    public void ToggleMod()
    {
        mod.GetDescription().SetModActive(modToggle.isOn);
    }

    public void Refresh()
    {
        modToggle.isOn = mod.GetDescription().IsActive();
    }
}
