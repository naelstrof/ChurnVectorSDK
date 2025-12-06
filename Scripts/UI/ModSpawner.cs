using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;

public class ModSpawner : MonoBehaviour
{
    [SerializeField] private LocalizedString noModString;
    [SerializeField] private GameObject modPanelPrefab;
    [SerializeField] private GameObject errorText;
    [SerializeField] private ModReplacementSpawner replacementSpawner;

    [SerializeField] private GameObject selfPanel;
    [SerializeField] private GameObject activatePanel;
    [SerializeField] private GameObject modPanel;
    private List<ModPanel> spawnedPrefabs = new List<ModPanel>();

    private void OnEnable()
    {
        StartCoroutine(SpawnRoutine());
    }

    private void OnDisable()
    {
        modPanel.SetActive(false);

        if (spawnedPrefabs == null)
            return;

        foreach(var obj in spawnedPrefabs)
            Destroy(obj.gameObject);

        spawnedPrefabs = null;
        Modding.SavePreferences();
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitUntil(() => !Modding.IsLoading());

        IReadOnlyCollection<Mod> mods = Modding.GetMods();
        modPanel.SetActive(true);

        foreach (Mod mod in mods)
        {
            if (mod != null && mod.GetDescription().GetReplacementCharacters().Count > 0)
                OnFoundMod(mod);
        }

        if(spawnedPrefabs == null || spawnedPrefabs.Count == 0)
        {
            errorText.GetComponent<TMP_Text>().text = noModString.GetLocalizedString();
        }
    }

    private void OnFoundMod(Mod mod)
    {
        spawnedPrefabs ??= new List<ModPanel>();
        var modObj = Instantiate(modPanelPrefab, transform);

        ModPanel panel = modObj.GetComponent<ModPanel>();
        panel.SetMod(mod);

        spawnedPrefabs.Add(panel);
        errorText.gameObject.SetActive(false);

        panel.DetailsButton().onClick.AddListener(() => {
            replacementSpawner.AssignMod(mod);
            selfPanel.SetActive(false);
            activatePanel.SetActive(true);
        });
    }

    public void RefreshAll()
    {
        foreach (ModPanel panel in spawnedPrefabs)
            panel.Refresh();
    }
}
