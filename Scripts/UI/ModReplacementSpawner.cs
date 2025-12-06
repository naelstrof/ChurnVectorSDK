using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using TMPro;

public class ModReplacementSpawner : MonoBehaviour
{
    [SerializeField] private GameObject variantPanelPrefab;
    [SerializeField] private TMP_Text errorText;
    [SerializeField] private GameObject junk;
    [SerializeField] private GameObject selfPanel;

    [SerializeField] private LocalizedString errorMessage;
    [SerializeField] private LocalizedString notFoundMessage;

    private List<VariantPanel> spawnedPrefabs = new List<VariantPanel>();
    private List<CharacterVariant> characterVariants = new List<CharacterVariant>();

    bool found = false;

    private void OnEnable()
    {
        found = false;

        selfPanel.SetActive(true);
        if(junk)
            junk.SetActive(true);
        StartCoroutine(SpawnRoutine());
    }

    private void OnDisable()
    {
        if (junk)
            junk.SetActive(false);
        selfPanel.SetActive(false);

        if (spawnedPrefabs == null)
            return;

        foreach (var obj in spawnedPrefabs)
            Destroy(obj.gameObject);

        spawnedPrefabs = null;
        Modding.SavePreferences();
    }

    public void AssignMod(Mod mod)
    {
        characterVariants = CharacterLibrary.GetVariants(mod);
    }

    public void AssignBase(CivilianReference baseCharacter)
    {
        characterVariants = CharacterLibrary.GetVariants(baseCharacter);
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitUntil(() => !CharacterLibrary.IsLoading());
        errorText.gameObject.SetActive(true);
        errorText.text = errorMessage.GetLocalizedString();

        foreach(var variant in characterVariants)
        {
            if(variant != null)
            {
                found = true;
                StartCoroutine(OnFoundVariant(variant));
            }
        }

        if (found == false)
            errorText.text = notFoundMessage.GetLocalizedString();
    }

    private IEnumerator OnFoundVariant(CharacterVariant variant)
    {
        spawnedPrefabs ??= new List<VariantPanel>();
        var variantObj = Instantiate(variantPanelPrefab, transform);
        //variantObj.SetActive(false);

        VariantPanel panel = variantObj.GetComponent<VariantPanel>();
        spawnedPrefabs.Add(panel);
        yield return panel.SetVariant(variant);
        errorText.gameObject.SetActive(false);

        //variantObj.SetActive(true);
    }
}
