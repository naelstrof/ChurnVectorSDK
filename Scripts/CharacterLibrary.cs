using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class CharacterLibrary : MonoBehaviour
{
    public List<CivilianReference> defaultCharacters;

    private static bool loading = true;
    private static CharacterLibrary instance;
    private static Dictionary<string, CharacterData> variants = new Dictionary<string, CharacterData>();

    public static bool IsLoading() => loading;

    public static Task GetLoadingTask()
    {
        if (!IsLoading())
        {
            return Task.CompletedTask;
        }

        return new Task(() => {
            while (IsLoading())
            {
                Thread.Sleep(10);
            }
        });
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            StartCoroutine(Initiaize());
        }
    }

    private IEnumerator Initiaize()
    {
        foreach (var character in defaultCharacters)
        {
            string guid = character.AssetGUID;
            if(!variants.ContainsKey(guid))
                variants.Add(guid, new CharacterData(character));
        }

        yield return new WaitUntil(() => !Modding.IsLoading());

        foreach (var mod in Modding.GetMods(true))
        {
            foreach(var character in mod.GetDescription().GetReplacementCharacters())
            {
                if (variants.ContainsKey(character.existingGUID))
                    variants[character.existingGUID].AddVariant(mod, character.existingGUID, character.replacementGUID);

            }
        }

        loading = false;
    }

    public static CivilianReference GetCharacter(CivilianReference civilian)
    {
        if(!variants.ContainsKey(civilian.AssetGUID))
            return null;

        return variants[civilian.AssetGUID].GetCharacter();
    }

    public static IReadOnlyCollection<CharacterVariant> GetVariants(Mod mod)
    {
        var result = new List<CharacterVariant>();

        if (mod != null)
        {
            foreach (var replacement in mod.GetDescription().GetReplacementCharacters())
            {
                if (variants.ContainsKey(replacement.existingGUID))
                {
                    var variant = variants[replacement.existingGUID].GetVariant(replacement.replacementGUID);
                    if (variant != null)
                        result.Add(variant);
                }
            }
        }

        return result;
    }

    private class CharacterData
    {
        private CharacterVariant baseVariant;
        private List<CharacterVariant> variants;

        public CharacterData(CivilianReference baseCharacter)
        {
            variants = new List<CharacterVariant>();
            CharacterVariant newVariant = new CharacterVariant(null, baseCharacter.AssetGUID);
            baseVariant = newVariant;
        }

        public void AddVariant(Mod source, string baseGUID, string assetGUID)
        {
            CharacterVariant variant = new CharacterVariant(source, baseGUID, assetGUID);
            if (!variants.Contains(variant))
                variants.Add(variant);
        }

        public CivilianReference GetCharacter()
        {
            var activeVariants = variants.Where(variant => variant.IsActive()).ToList();

            if (activeVariants.Count == 0)
                return baseVariant.GetReference();

            return activeVariants[UnityEngine.Random.Range(0, activeVariants.Count)].GetReference();
        }

        public CharacterVariant GetVariant(string assetGUID)
        {
            foreach (var variant in variants)
            {
                if (variant.GetReference().AssetGUID == assetGUID)
                    return variant;
            }

            return null;
        }
    }
}
