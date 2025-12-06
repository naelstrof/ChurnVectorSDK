using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterPanel : MonoBehaviour
{
    [SerializeField] private Image modImage;
    [SerializeField] private TMP_Text modTitle;

    public IEnumerator SetCharacter(CharacterVariant variant, ModReplacementSpawner replacementSpawner)
    {
        yield return variant.LoadCharacterData();

        modImage.sprite = variant.GetIcon();
        modTitle.text = variant.GetName();

        GetComponent<Button>().onClick.AddListener(() =>
        {
            replacementSpawner.AssignBase(variant.GetReference());
        });
    }
}
