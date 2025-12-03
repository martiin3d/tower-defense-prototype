using UnityEngine;

/// <summary>
/// Implements a targeting system that selects the closest enemy to a given position within a specified range.
/// </summary>
public class ClosestTargetingSystem : IClosestTargetingSystem
{
    private readonly IEnemyPoolManager _enemyPool;

    /// <summary>
    /// Constructs the targeting system with access to the active enemy pool.
    /// </summary>
    /// <param name="enemyPool">The enemy pool manager to retrieve active enemies from.</param>
    public ClosestTargetingSystem(IEnemyPoolManager enemyPool)
    {
        _enemyPool = enemyPool;
    }

    /// <summary>
    /// Returns the closest enemy to the specified position within the given range.
    /// </summary>
    /// <param name="cannonPosition">The position to search from.</param>
    /// <param name="range">The maximum distance to search for a enemy.</param>
    /// <returns>The closest valid enemy within range, or null if none are found.</returns>
    public Enemy GetTarget(Vector3 cannonPosition, float range)
    {
        var enemies = _enemyPool.GetActiveEnemies;
        float closestDist = range;
        Enemy closest = null;

        foreach (var enemy in enemies)
        {
            float dist = Vector3.Distance(cannonPosition, enemy.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = enemy;
            }
        }

        return closest;
    }
}