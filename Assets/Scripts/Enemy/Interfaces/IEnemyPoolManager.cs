using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages object pooling for enemies, allowing efficient reuse of enemy instances.
/// Initializes pools based on enemy configuration data and handles retrieval and return of enemies.
/// </summary>
public interface IEnemyPoolManager
{
    /// <summary>
    /// Gets the list of currently active enemies in the game.
    /// This list is read-only to prevent external modifications.
    /// </summary>
    IReadOnlyList<Enemy> GetActiveEnemies { get; }

    /// <summary>
    /// Event invoked when there are no active enemies remaining in the pool.
    /// </summary>
    Action OnNoActiveEnemies { get; set; }

    /// <summary>
    /// Initializes the enemy pools for each enemy type using the given enemy database.
    /// </summary>
    /// <param name="enemyDataBase">Database containing enemy configurations and prefabs.</param>
    void Initialize(EnemyDatabase enemyDatabase);

    /// <summary>
    /// Retrieves a enemy of the specified type from the pool and activates it at the given position and parent.
    /// </summary>
    /// <param name="type">The type of enemy to get.</param>
    /// <param name="position">Spawn position for the enemy.</param>
    /// <param name="parent">Parent transform for the enemy.</param>
    /// <returns>The activated enemy instance or null if the type is not found.</returns>
    Enemy GetEnemy(EnemyData.EnemyType type, Vector3 position, Transform transform);

    /// <summary>
    /// Returns a enemy to its pool, removes it from the active enemies list,
    /// and invokes OnNoActiveEnemies if no active enemies remain.
    /// </summary>
    /// <param name="enemy">The enemy to return to the pool.</param>
    void ReturnEnemy(Enemy enemy);

    /// <summary>
    /// Returns all active enemies in all pools back to their respective pools and clears the active enemies list.
    /// </summary>
    void ReturnAllEnemies();
}