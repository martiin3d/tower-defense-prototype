using UnityEngine;

/// <summary>
/// Manages object pools of cannons by cannon type,
/// providing efficient retrieval and return of cannon instances.
/// </summary>
public interface ICannonPoolManager
{
    /// <summary>
    /// Initializes the cannon pools using the given cannon database.
    /// </summary>
    /// <param name="cannonDataBase">Database containing cannon configurations and prefabs.</param>
    void Initialize(CannonDatabase cannonDataBase);

    /// <summary>
    /// Retrieves a cannon instance from the pool of the specified type.
    /// If no cannon pool exists for the type returns null.
    /// </summary>
    /// <param name="type">The cannon type to get.</param>
    /// <param name="position">Position where the cannon will be placed.</param>
    /// <param name="parent">Optional parent transform for the cannon instance.</param>
    /// <returns>A pooled cannon instance or null if not found.</returns>
    Cannon GetCannon(CannonData.CannonType type, Vector3 position, Transform parent);

    /// <summary>
    /// Returns a cannon instance to its corresponding pool.
    /// </summary>
    /// <param name="cannon">cannon instance to return to pool.</param>
    void ReturnCannon(Cannon cannon);

    /// <summary>
    /// Returns all active cannons in all pools back to their respective pools.
    /// </summary>
    void ReturnAllCannons();

    /// <summary>
    /// Clears all cannon pools, destroying all pooled and active cannon GameObjects.
    /// </summary>
    void ClearPools();
}