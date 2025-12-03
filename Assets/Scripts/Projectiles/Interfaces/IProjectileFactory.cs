using UnityEngine;

/// <summary>
/// Factory class responsible for creating and initializing projectiles from a pool using configuration from the ProjectileDatabase.
/// Manages subscription to the return-to-pool event to ensure proper cleanup.
/// </summary>
public interface IProjectileFactory
{
    /// <summary>
    /// Creates and initializes a projectile of the specified type.
    /// </summary>
    /// <param name="type">The type of projectile to create.</param>
    /// <param name="origin">The starting position of the projectile.</param>
    /// <param name="targetPosition">The target position the projectile will head toward.</param>
    /// <returns>The created and initialized projectile instance.</returns>
    Projectile Create(ProjectileData.ProjectileType type, Vector3 origin, Vector3 targetPosition);
}