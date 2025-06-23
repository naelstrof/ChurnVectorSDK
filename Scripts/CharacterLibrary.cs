using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SimpleJSON;

public class CharacterLibrary : MonoBehaviour
{
    public List<CivilianReference> defaultCharacters;

    private static bool loading = true;
    private static CharacterLibrary instance;
    private static Dictionary<string, CharacterData> variants = new Dictionary<string, CharacterData>();
    private static ReplacementMethod replacementMethod;

    public enum ReplacementMethod
    {
        Default,
        Random,
        Alternating
    }

    public static bool IsLoading() => loading;
    public static ReplacementMethod GetReplacementMethod() => replacementMethod;

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

    public static void Load(JSONNode node)
    {
        replacementMethod = (ReplacementMethod)(int)node[nameof(replacementMethod)].Or(0);
    }

    public static void Save(JSONNode node)
    {
        node[nameof(replacementMethod)] = (int)replacementMethod;
    }

    public static CivilianReference GetCharacter(CivilianReference civilian)
    {
        if(!variants.ContainsKey(civilian.AssetGUID))
            return null;

        return variants[civilian.AssetGUID].GetCharacter(replacementMethod);
    }

    public static List<CharacterVariant> GetVariants(Mod mod)
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

    public static List<CharacterVariant> GetDefaults()
    {
        List<CharacterVariant> result = new List<CharacterVariant>();

        foreach(var character in variants.Values)
        {
            result.Add(character.GetBaseVariant());
        }

        return result;
    }

    public static List<CharacterVariant> GetVariants(CivilianReference baseCharacter)
    {
        var result = new List<CharacterVariant>();

        if(!variants.ContainsKey(baseCharacter.AssetGUID))
        {
            return result;
        }

        result = variants[baseCharacter.AssetGUID].GetVariants();
        return result;
    }

    public static void SetReplacementMethod(ReplacementMethod newMethod)
    {
        replacementMethod = newMethod;
    }

    private class CharacterData
    {
        private CharacterVariant baseVariant;
        private List<CharacterVariant> variants;

        private CivilianReference lastUsed;
        private float timeSinceAccess = -1f;
        private int index = 0;

        public CharacterVariant GetBaseVariant() => baseVariant;

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

        public CivilianReference GetCharacter(ReplacementMethod method = ReplacementMethod.Default)
        {
            var activeVariants = variants.Where(variant => variant.IsActive()).ToList();

            if (activeVariants.Count == 0)
                return baseVariant.GetReference();

            CivilianReference result;

            switch(method)
            {
                case ReplacementMethod.Random:
                    result = activeVariants[UnityEngine.Random.Range(0, activeVariants.Count)].GetReference();
                    break;
                case ReplacementMethod.Alternating:
                    if (timeSinceAccess > Time.time - 15f)
                        index++;
                    else
                        index = 0;
                    result = activeVariants[index % activeVariants.Count].GetReference();
                    break;
                default:
                    result = activeVariants[0].GetReference();
                    break;
            }

            timeSinceAccess = Time.time;
            return result;
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

        public List<CharacterVariant> GetVariants()
        {
            return variants.Where(variant => variant.GetSource().GetDescription().IsActive()).ToList();
        }
    }
}
