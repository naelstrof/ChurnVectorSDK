using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using UnityEngine.Events;

public class ModOptions : MonoBehaviour
{
    [SerializeField] private ModSpawner spawner;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private Button enableButton;
    [SerializeField] private Button disableButton;

    private CharacterLibrary.ReplacementMethod selectMethod;
    bool initialized = false;

    private void OnEnable()
    {
        if (!initialized)
        {
            InitializeDropdown();
            InitializeButtons();
            initialized = true;
        }
    }

    private void InitializeDropdown()
    {
        selectMethod = CharacterLibrary.GetReplacementMethod();

        List<TMP_Dropdown.OptionData> data = new List<TMP_Dropdown.OptionData>();
        foreach (string str in Enum.GetNames(typeof(CharacterLibrary.ReplacementMethod)))
        {
            data.Add(new TMP_Dropdown.OptionData(str));
        }
        dropdown.options = data;
        dropdown.onValueChanged.AddListener(SetValue);
        dropdown.SetValueWithoutNotify((int)selectMethod);
    }

    private void InitializeButtons()
    {
        enableButton.GetComponent<Button>().onClick.AddListener(() => {
            ToggleAll(true);
        });

        disableButton.GetComponent<Button>().onClick.AddListener(() => {
            ToggleAll(false);
        });
    }

    private void SetValue(int value)
    {
        dropdown.SetValueWithoutNotify(value);
        CharacterLibrary.SetReplacementMethod((CharacterLibrary.ReplacementMethod)value);
    }

    public void ToggleAll(bool active)
    {
        foreach(Mod mod in Modding.GetMods(true))
        {
            mod.GetDescription().SetModActive(active);
        }
        spawner.RefreshAll();
    }
}
