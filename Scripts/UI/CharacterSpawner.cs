using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using TMPro;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField] private LocalizedString noModString;
    [SerializeField] private GameObject characterPanelPrefab;
    [SerializeField] private GameObject errorText;
    [SerializeField] private ModReplacementSpawner replacementSpawner;

    [SerializeField] private GameObject selfPanel;
    [SerializeField] private GameObject activatePanel;
    [SerializeField] private GameObject junk;
    private List<CharacterPanel> spawnedPrefabs = new List<CharacterPanel>();

    private void OnEnable()
    {
        replacementSpawner.SetPreviousMenu(selfPanel);
        junk.SetActive(true);
        StartCoroutine(SpawnRoutine());
    }

    private void OnDisable()
    {
        junk.SetActive(false);
        if (spawnedPrefabs == null)
            return;

        foreach (var obj in spawnedPrefabs)
            Destroy(obj.gameObject);

        spawnedPrefabs = null;
        Modding.SavePreferences();
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitUntil(() => !CharacterLibrary.IsLoading());

        foreach(CharacterVariant variant in CharacterLibrary.GetDefaults())
        {
            StartCoroutine(OnFoundCharacter(variant));
        }


        if (spawnedPrefabs == null || spawnedPrefabs.Count == 0)
        {
            errorText.GetComponent<TMP_Text>().text = noModString.GetLocalizedString();
        }
    }

    private IEnumerator OnFoundCharacter(CharacterVariant character)
    {
        spawnedPrefabs ??= new List<CharacterPanel>();
        var uiObj = Instantiate(characterPanelPrefab, transform);

        CharacterPanel panel = uiObj.GetComponent<CharacterPanel>();
        yield return panel.SetCharacter(character, replacementSpawner);

        spawnedPrefabs.Add(panel);
        errorText.gameObject.SetActive(false);

        panel.GetComponent<Button>().onClick.AddListener(() => {
            selfPanel.SetActive(false);
            activatePanel.SetActive(true);
        });
    }
}
