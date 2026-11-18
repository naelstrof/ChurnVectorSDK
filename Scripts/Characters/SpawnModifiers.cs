using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

[RequireComponent(typeof(CharacterLoader))]
public class SpawnModifiers : MonoBehaviour
{
    [SerializeField]
    private string overrideName = "";
    [SerializeField]
    private Transform spawnParent;
    [SerializeField]
    private float speedModifier = 0;

    //Since the CharacterLoader disables the gameobject before Awake/Start, OnDisable should be used instead.
    private void OnDisable()
    {
        CharacterLoader loader = GetComponent<CharacterLoader>();

        var handle = loader.GetCharacterAsync();
        if (handle.IsDone)
        {
            OnCompletedLoadCharacter(handle);
        }
        else
        {
            handle.Completed += OnCompletedLoadCharacter;
        }
    }

    private void OnCompletedLoadCharacter(AsyncOperationHandle<Civilian> obj)
    {
        GameObject character = obj.Result.gameObject;

        if(!string.IsNullOrEmpty(overrideName))
            character.name = overrideName;

        if(spawnParent != null && spawnParent.gameObject.activeSelf)
            character.transform.SetParent(spawnParent, true);

        if(speedModifier > 0)
            ((CharacterBase)obj.Result).AddSpeedModifier(new Modifier(speedModifier));
    }
}
