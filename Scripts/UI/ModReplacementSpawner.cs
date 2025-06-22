using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModReplacementSpawner : MonoBehaviour
{
    [SerializeField] private GameObject variantPanelPrefab;
    [SerializeField] private GameObject errorText;
    [SerializeField] private GameObject junk;
    private List<VariantPanel> spawnedPrefabs = new List<VariantPanel>();

    private Mod mod;

    private void OnEnable()
    {
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
                StartCoroutine(OnFoundVariant(variant));
            }
        }
    }

    private IEnumerator OnFoundVariant(CharacterVariant variant)
    {
        spawnedPrefabs ??= new List<VariantPanel>();
        var variantObj = Instantiate(variantPanelPrefab, transform);
        //variantObj.SetActive(false);

        VariantPanel panel = variantObj.GetComponent<VariantPanel>();
        yield return panel.SetVariant(variant);
        errorText.gameObject.SetActive(false);

        spawnedPrefabs.Add(panel);
        //variantObj.SetActive(true);
    }
}
