using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class MapCategoryPanel : MonoBehaviour {
    [SerializeField] private Image categoryImage;
    [SerializeField] private TMP_Text categoryText;
    private LevelCategory category;
    private MapSpawner levelSelect;
    
    public void SetCategory(LevelCategory category, MapSpawner levelSelect) {
        this.category = category;
        this.levelSelect = levelSelect;
        categoryImage.sprite = category.GetSprite();
        categoryText.text = category.GetName().GetLocalizedString();
        var stringEvent = categoryText.GetComponent<LocalizeStringEvent>();
        if (stringEvent != null) {
            stringEvent.StringReference = category.GetName();
        }
    }

    public void Start() {
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick() {
        levelSelect.SetCategory(category);
    }
}
