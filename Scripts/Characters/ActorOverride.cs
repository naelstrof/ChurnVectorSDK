using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;
using AI.Actions;

public class ActorOverride : MonoBehaviour
{
    [SerializeField, SerializeReference, SubclassSelector]
    private BaseAction startingAction;
    [SerializeField] private List<CharacterGroup> overrideGroups;
    [SerializeField] private List<CharacterGroup> overrideUseByGroups;

    public void ApplyActionOverride(InputGenerator inputGenerator)
    {
        if (startingAction == null || inputGenerator == null || inputGenerator is InputGeneratorPlayerPossession)
            return;

        ((Actor)inputGenerator).SetStartingAction(startingAction);
    }

    public void ApplyGroupOverrides(CharacterBase character, List<CharacterGroup> defaultGroups, List<CharacterGroup> defaultUseByGroups)
    {
        List<CharacterGroup> newGroups = new List<CharacterGroup>(defaultGroups.Count + overrideGroups.Count);
        newGroups.AddRange(defaultGroups);
        newGroups.AddRange(overrideGroups);

        List<CharacterGroup> newUseByGroups = new List<CharacterGroup>(defaultUseByGroups.Count + overrideUseByGroups.Count);
        newUseByGroups.AddRange(defaultUseByGroups);
        newUseByGroups.AddRange(overrideUseByGroups);

        character.SetGroups(newGroups);
        character.SetUseByGroups(newUseByGroups);
    }
}