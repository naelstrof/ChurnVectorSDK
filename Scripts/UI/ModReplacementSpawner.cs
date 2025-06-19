using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModReplacementSpawner : MonoBehaviour
{
    [SerializeField] private GameObject variantPanelPrefab;
    [SerializeField] private GameObject errorText;
    private List<VariantPanel> spawnedPrefabs = new List<VariantPanel>();

    private Mod mod;

    private void OnEnable()
    {
        StartCoroutine(SpawnRoutine());
    }

    private void OnDisable()
    {
        if (spawnedPrefabs == null)
            return;

        foreach (var obj in spawnedPrefabs)
            Destroy(obj.gameObject);

        spawnedPrefabs = null;
        Modding.SavePreferences();
    }

    public void AssignMod(Mod mod)
    {
        this.mod = mod;
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitUntil(() => !CharacterLibrary.IsLoading());
        errorText.SetActive(true);

        foreach(var variant in CharacterLibrary.GetVariants(mod))
        {
            if(variant != null)
            {
                OnFoundVariant(variant);
            }
        }
    }

    private void OnFoundVariant(CharacterVariant variant)
    {
        spawnedPrefabs ??= new List<VariantPanel>();
        var variantObj = Instantiate(variantPanelPrefab, transform);

        VariantPanel panel = variantObj.GetComponent<VariantPanel>();
        panel.SetVariant(variant);

        spawnedPrefabs.Add(panel);
        errorText.gameObject.SetActive(false);
    }
}
