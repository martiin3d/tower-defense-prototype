using UnityEngine;

/// <summary>
/// Factory class responsible for creating and initializing projectiles from a pool using configuration from the ProjectileDatabase.
/// Manages subscription to the return-to-pool event to ensure proper cleanup.
/// </summary>
public class ProjectileFactory : IProjectileFactory
{
    private readonly ProjectileDatabase _database;
    private readonly IProjectilePoolManager _pool;

    /// <summary>
    /// Constructs a new ProjectileFactory with a reference to the projectile database and pool manager.
    /// </summary>
    /// <param name="database">The ProjectileDatabase used to retrieve projectile configuration data.</param>
    /// <param name="pool">The IProjectilePoolManager responsible for managing pooled projectiles.</param>
    public ProjectileFactory(ProjectileDatabase database, IProjectilePoolManager pool)
    {
        _database = database;
        _pool = pool;
    }

    /// <summary>
    /// Creates and initializes a projectile of the specified type.
    /// </summary>
    /// <param name="type">The type of projectile to create.</param>
    /// <param name="origin">The starting position of the projectile.</param>
    /// <param name="targetPosition">The target position the projectile will head toward.</param>
    /// <returns>The created and initialized projectile instance.</returns>
    public Projectile Create(ProjectileData.ProjectileType type, Vector3 origin, Vector3 targetPosition)
    {
        var projectile = _pool.GetProjectile(type, origin, null);
        projectile.RequestReturnToPool += Return;
        projectile.Initialize(_database.GetConfig(type), targetPosition);
        return projectile;
    }

    /// <summary>
    /// Returns the given projectile to the pool.
    /// </summary>
    /// <param name="projectile">The projectile to return to the pool.</param>
    private void Return(Projectile projectile)
    {
        projectile.RequestReturnToPool -= Return;
        _pool.ReturnProjectile(projectile);
    }
}