using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages object pooling for projectiles, allowing efficient reuse of projectile instances.
/// Initializes pools based on projectile configuration data and handles retrieval and return of projectiles.
/// </summary>
public class ProjectilePoolManager : IProjectilePoolManager
{
    private Dictionary<ProjectileData.ProjectileType, ObjectPool<Projectile>> _projectilePool;

    /// <summary>
    /// Initializes the projectile pools using the provided ProjectileDatabase.
    /// Each projectile type receives its own dedicated pool.
    /// </summary>
    /// <param name="projectileDataBase">The database containing projectile configurations and prefabs.</param>
    public void Initialize(ProjectileDatabase projectileDataBase)
    {
        GameObject parent = new GameObject("__POOL__PROJECTILE");
        _projectilePool = new Dictionary<ProjectileData.ProjectileType, ObjectPool<Projectile>>();

        foreach (var projectileData in projectileDataBase.GetConfigMap)
        {
            var pool = new ObjectPool<Projectile>(projectileData.Value.prefab, 50, parent.transform);
            _projectilePool.Add(projectileData.Key, pool);
        }
    }

    /// <summary>
    /// Retrieves a projectile of the specified type from the pool and positions it at the given location.
    /// </summary>
    /// <param name="type">The type of projectile to retrieve.</param>
    /// <param name="position">The position to spawn the projectile at.</param>
    /// <param name="parent">The optional parent transform for the projectile.</param>
    /// <returns>The pooled projectile instance, or null if the type is not found.</returns>
    public Projectile GetProjectile(ProjectileData.ProjectileType type, Vector3 position, Transform parent)
    {
        if (_projectilePool.TryGetValue(type, out var pool))
        {
            Projectile projectile = pool.Get(position, parent);
            return projectile;
        }

        Debug.LogError($"Projectile type {type} not found in pool.");
        return null;
    }

    /// <summary>
    /// Returns the given projectile to its corresponding pool.
    /// </summary>
    /// <param name="projectile">The projectile instance to return to the pool.</param>
    public void ReturnProjectile(Projectile projectile)
    {
        if (projectile == null) return;

        if (_projectilePool.TryGetValue(projectile.type, out var pool))
        {
            pool.ReturnToPool(projectile);
        }
        else
        {
            Debug.LogError("Projectile type not found in pool.");
        }
    }

    /// <summary>
    /// Returns all active projectiles from all pools to be reused.
    /// </summary>
    public void ReturnAllProjectile()
    {
        foreach (var pool in _projectilePool.Values)
        {
            pool.ReturnAllToPool();
        }
    }

    /// <summary>
    /// Clears all projectile pools and releases resources.
    /// </summary>
    public void ClearPools()
    {
        foreach (var pool in _projectilePool.Values)
        {
            pool.Clear();
        }
        _projectilePool.Clear();
    }
}