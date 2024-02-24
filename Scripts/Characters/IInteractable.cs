using UnityEngine;

public interface IInteractable {
    Transform transform { get; }
    /// <summary>
    /// Get if it's technically possible for a character to use this, this is literal physical limitations and such.
    /// Like if the usable is already in use, or out of batteries.
    /// </summary>
    /// <param name="from">The character to check if they can interact</param>
    /// <returns></returns>
    bool CanInteract(CharacterBase from);
    
    /// <summary>
    /// Get if it makes sense to use, this is more social structure, like a worker wouldn't use a police locker, for example.
    /// And nobody would use a door or lightswitch arbitrarily.
    /// Players will respect this and avoid using them. You'll have to account for this if you want them to be able to turn off lightswitches!
    /// </summary>
    /// <param name="from">Character to check if they should interact.</param>
    /// <returns></returns>
    bool ShouldInteract(CharacterBase from);
    void OnBeginInteract(CharacterBase from);
    void OnEndInteract(CharacterBase from);
    Bounds GetInteractBounds();
}
