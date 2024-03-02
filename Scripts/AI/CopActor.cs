using System.Collections.Generic;
using UnityEngine;

namespace AI {
    
public partial class CharacterActor : Actor {
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

        var shooter = ProjectilePrediction.Target.GetShotTarget(barrelPosition, character.GetBody().velocity, character.GetGrounded());
        var pTarget = ProjectilePrediction.Target.GetShotTarget(target.transform.position, velocity, target.GetGrounded());
        var shotQuery = ProjectilePrediction.ShotQuery.GetShotQuery(shooter, pTarget, Projectile.speed, Physics.gravity);
        if (shotQuery.TryGetAimDirectionForFastball(out Vector3 shootDirection, delay)) {
            aimDirection = shootDirection;
            return true;
        }
        aimDirection = default;
        return false;
    }
}

}