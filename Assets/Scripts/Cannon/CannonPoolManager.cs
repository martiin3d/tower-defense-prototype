using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages object pools of cannons by cannon type,
/// providing efficient retrieval and return of cannon instances.
/// </summary>
public class CannonPoolManager : ICannonPoolManager
{
    private Dictionary<CannonData.CannonType, ObjectPool<Cannon>> _cannonPool;

    /// <summary>
    /// Initializes the cannon pools using the given cannon database.
    /// </summary>
    /// <param name="cannonDataBase">Database containing cannon configurations and prefabs.</param>
    public void Initialize(CannonDatabase cannonDataBase)
    {
        GameObject parent = new GameObject("__POOL__CANNON");
        _cannonPool = new Dictionary<CannonData.CannonType, ObjectPool<Cannon>>();

        foreach (var cannonData in cannonDataBase.GetConfigMap)
        {
            var pool = new ObjectPool<Cannon>(cannonData.Value.prefab, 10, parent.transform);
            _cannonPool.Add(cannonData.Key, pool);
        }
    }

    /// <summary>
    /// Retrieves a cannon instance from the pool of the specified type.
    /// If no cannon pool exists for the type returns null.
    /// </summary>
    /// <param name="type">The cannon type to get.</param>
    /// <param name="position">Position where the cannon will be placed.</param>
    /// <param name="parent">Optional parent transform for the cannon instance.</param>
    /// <returns>A pooled cannon instance or null if not found.</returns>
    public Cannon GetCannon(CannonData.CannonType type, Vector3 position, Transform parent)
    {
        if (_cannonPool.TryGetValue(type, out var pool))
        {
            Cannon cannon = pool.Get(position, parent);
            return cannon;
        }

        Debug.LogError($"Cannon type {type} not found in pool.");
        return null;
    }

    /// <summary>
    /// Returns a cannon instance to its corresponding pool.
    /// </summary>
    /// <param name="cannon">Cannon instance to return to pool.</param>
    public void ReturnCannon(Cannon cannon)
    {
        if (cannon == null) return;

        if (_cannonPool.TryGetValue(cannon.type, out var pool))
        {
            pool.ReturnToPool(cannon);
        }
        else
        {
            Debug.LogError("Cannon type not found in pool.");
        }
    }

    /// <summary>
    /// Returns all active cannons in all pools back to their respective pools.
    /// </summary>
    public void ReturnAllCannons()
    {
        foreach (var pool in _cannonPool.Values)
        {
            pool.ReturnAllToPool();
        }
    }

    /// <summary>
    /// Clears all cannon pools, destroying all pooled and active cannon GameObjects.
    /// </summary>
    public void ClearPools()
    {
        foreach (var pool in _cannonPool.Values)
        {
            pool.Clear();
        }
        _cannonPool.Clear();
    }
}