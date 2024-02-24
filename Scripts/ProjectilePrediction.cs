using System.Collections.Generic;
using UnityEngine;

public static class ProjectilePrediction {
    public struct ShotConfiguration {
        public float delay;
        public float projectileSpeed;
        public Vector3 projectilePosition;
        public Vector3 projectileInheritedVelocity;
        public Vector3 projectileAcceleration;
        public Vector3 targetPosition;
        public Vector3 targetVelocity;
        public Vector3 targetAcceleration;

        /// <summary>
        /// A shot configuration used to make predictions on how to hit the target.
        /// </summary>
        /// <param name="projectilePosition">The position of the bullet when fired.</param>
        /// <param name="projectileInheritedVelocity">The inherited velocity of the bullet when fired, this should be zero unless it inherits velocity from the shooter, and the shooter has a non-zero velocity.</param>
        /// <param name="projectileAcceleration">The acceleration of the bullet when fired, this should be Physics.gravity if it's a normal projectile under the effects of gravity.</param>
        /// <param name="targetPosition">The current position of the target to hit.</param>
        /// <param name="targetVelocity">The current velocity of the target to hit.</param>
        /// <param name="targetAcceleration">The current acceleration of the target to hit, should be Physics.gravity if the target is airborne.</param>
        /// <param name="projectileSpeed">The speed of the bullet when fired.</param>
        /// <param name="delay">How far ahead are we predicting the shot. Should be 0 if we're taking the shot this frame.</param>
        public ShotConfiguration(Vector3 projectilePosition, Vector3 projectileInheritedVelocity, Vector3 projectileAcceleration, Vector3 targetPosition, Vector3 targetVelocity, Vector3 targetAcceleration, float projectileSpeed, float delay = 0f) {
            this.targetAcceleration = targetAcceleration;
            this.targetVelocity = targetVelocity;
            this.targetPosition = targetPosition;
            this.projectileAcceleration = projectileAcceleration;
            this.projectileInheritedVelocity = projectileInheritedVelocity;
            this.projectilePosition = projectilePosition;
            this.projectileSpeed = projectileSpeed;
            this.delay = delay;
        }
    }
    
    /// <summary>
    /// Finds timings of intersections between an expanding sphere representing all possible shots. Use in conjunction with ConvertShotTimingToAimDirection.
    /// Can give up to 4 solutions. The results are sorted, and negative time solutions are removed.
    /// </summary>
    /// <param name="rootBuffer">A buffer to hold all the timings.</param>
    /// <param name="shot">The shot to make.</param>
    /// <returns>The number of valid solutions found.</returns>
    public static int TryGetPossibleShotTimings(List<float> rootBuffer, ShotConfiguration shot) {
        float timeOffset = -shot.delay;
        Vector3 p = shot.targetPosition - shot.projectilePosition;
        Vector3 v = shot.targetVelocity - shot.projectileInheritedVelocity;
        Vector3 a = shot.targetAcceleration - shot.projectileAcceleration;
        float t4 = (a.x * a.x + a.y * a.y + a.z * a.z) / 4f;
        float t3 = Vector3.Dot(a, v);
        float t2 = v.x * v.x + p.x * a.x + v.y * v.y + p.y * a.y + v.z * v.z + p.z * a.z - shot.projectileSpeed * shot.projectileSpeed;
        float t1 = 2f * (p.x * v.x + p.y * v.y + p.z * v.z - shot.projectileSpeed * shot.projectileSpeed * timeOffset);
        float t0 = p.x * p.x + p.y * p.y + p.z * p.z - shot.projectileSpeed * shot.projectileSpeed * timeOffset * timeOffset;
        rootBuffer.Clear();
        Polynomials.RootSolver.GetRootsForQuartic(rootBuffer, t4, t3, t2, t1, t0);
        rootBuffer.RemoveAll(IsInvalidRoot);
        rootBuffer.Sort();
        return rootBuffer.Count;
    }

    // Roots that are negative are backward in time, we discard those.
    private static bool IsInvalidRoot(float x) => x < 0f;
    
    /// <summary>
    /// Converts a timing provided by TryGetPossibleShotTimings to a usable aim direction using standard projectile equations.
    /// </summary>
    /// <param name="t">The time to hit the target.</param>
    /// <param name="shot">The shot to make.</param>
    /// <returns>The direction to shoot to hit the target.</returns>
    public static Vector3 ConvertShotTimingToAimDirection(float t, ShotConfiguration shot) {
        Vector3 v = shot.targetVelocity - shot.projectileInheritedVelocity;
        Vector3 a = shot.targetAcceleration - shot.projectileAcceleration;
        Vector3 aimTarget = shot.targetPosition + v * t + a * (0.5f * t * t);
        return (aimTarget-shot.projectilePosition).normalized;
    }

    /// <summary>
    /// Returns the fastest shot to make, straight, rather than lobbing.
    /// </summary>
    /// <param name="rootBufffer">The buffer to contain shot timings. Just used to prevent allocations.</param>
    /// <param name="shot">The shot to make.</param>
    /// <param name="shotDirection">The direction the shot needs to be made in to hit the target.</param>
    /// <returns>True if a solution was found, false if there wasn't.</returns>
    public static bool TryGetFastestShotDirection(List<float> rootBufffer, ShotConfiguration shot, out Vector3 shotDirection) {
        int possibleShots = TryGetPossibleShotTimings(rootBufffer, shot);
        if (possibleShots>0) {
            shotDirection = ConvertShotTimingToAimDirection(rootBufffer[0], shot);
            return true;
        }
        shotDirection = Vector3.zero;
        return false;
    }
}
