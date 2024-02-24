using System.Collections.Generic;
using UnityEngine;

namespace AI {
    
public partial class CharacterActor : Actor {
    private List<float> baseRoots = new();

    public override CharacterBase GetTaseTarget() => character.GetTaseTarget();
    
    public override List<Civilian> GetAllCops() {
        return Civilian.GetAllCops();
    }

    public override int GetTaseCount() {
        return character.GetTaseCount();
    }

    public override bool TryGetAimPrediction(CharacterBase target, float delay, out Vector3 aimDirection) {
        var knowledge = GetKnowledgeOf(target.gameObject);
        Vector3 velocity = target.GetBody().velocity;
        if (!character.CanSee(target.gameObject)) {
            velocity = Vector3.zero;
        }
        knowledge.TryGetLastKnownPosition(out Vector3 targetPosition);
        Vector3 dirToCharacter = Vector3.Normalize(character.transform.position - targetPosition);
        Vector3 barrelPosition = character.GetBarrelPosition();
        Vector3 dirToBarrel = Vector3.Normalize(barrelPosition - targetPosition);
        
        // The target is too close!!! Assume the projectile will come out of our center to prevent spinning
        if (Vector3.Dot(dirToBarrel, dirToCharacter) < 0f) {
            barrelPosition = character.transform.position;
        }
        var shot = new ProjectilePrediction.ShotConfiguration(
            barrelPosition, Vector3.zero, Physics.gravity,
            targetPosition, velocity,
            target.GetGrounded() ? Vector3.zero : Physics.gravity, Projectile.speed,
            delay);
        if (ProjectilePrediction.TryGetFastestShotDirection(baseRoots, shot, out Vector3 shootDirection)) {
            aimDirection = shootDirection;
            return true;
        }
        aimDirection = default;
        return false;
    }
}

}