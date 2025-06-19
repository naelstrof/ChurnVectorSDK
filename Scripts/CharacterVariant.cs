using System.Collections;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CharacterVariant
{
    private Mod source;
    private CivilianReference reference;

    private string name;
    private Sprite characterIcon;

    public string GetName() => name;
    public Sprite GetIcon() => characterIcon;
    public Mod GetSource() => source;
    public CivilianReference GetReference() => reference;
    public virtual bool IsActive()
    {
        if (source == null)
            return true;

        return source.GetDescription().IsReplacementActive(reference.AssetGUID);
    }

    public CharacterVariant(Mod source, string assetGUID)
    {
        this.source = source;
        this.reference = new CivilianReference(assetGUID);
    }

    public virtual void SetVariantActive(bool active)
    {
        source.GetDescription().SetReplacementActive(reference.AssetGUID, active);
    }

    private AsyncOperationHandle<GameObject>? characterHandle;

    public IEnumerator LoadCharacterData()
    {
        if (characterHandle.HasValue && characterHandle.Value.IsDone)
            yield break;

        characterHandle ??= reference.LoadAssetAsync<GameObject>();
        yield return new WaitUntil(() => characterHandle.Value.IsDone);

        var characterObject = characterHandle.Value.Result;
        name = characterObject.name;
        characterIcon = ((IChurnable)characterObject.GetComponent<Civilian>()).GetHeadSprite();
    }
}