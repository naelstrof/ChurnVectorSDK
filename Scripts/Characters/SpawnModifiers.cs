using UnityEngine;
using System.Threading.Tasks;

[RequireComponent(typeof(CharacterLoader))]
public class SpawnModifiers : InitializationManagerInitialized
{
    [SerializeField]
    private string overrideName = "";
    [SerializeField]
    private Transform spawnParent;
    [SerializeField]
    private float speedModifier = 0;

    public override async Task OnInitialized()
    {
        CharacterLoader loader = GetComponent<CharacterLoader>();
        var handle = loader.GetCharacterAsync();

        await handle.Task;

        GameObject character = handle.Result.gameObject;

        if (!string.IsNullOrEmpty(overrideName))
            character.name = overrideName;

        if (spawnParent != null && spawnParent.gameObject.activeSelf)
            character.transform.SetParent(spawnParent, true);

        if (speedModifier > 0)
            ((CharacterBase)handle.Result).AddSpeedModifier(new Modifier(speedModifier));
    }


    public override InitializationManager.InitializationStage GetInitializationStage()
    {
        return InitializationManager.InitializationStage.AfterMods;
    }
}
