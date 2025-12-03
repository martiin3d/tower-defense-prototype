using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages object pooling for enemies, allowing efficient reuse of enemy instances.
/// Initializes pools based on enemy configuration data and handles retrieval and return of enemies.
/// </summary>
public class EnemyPoolManager : IEnemyPoolManager
{
    private Dictionary<EnemyData.EnemyType, ObjectPool<Enemy>> _enemyPool;
    private List<Enemy> _activeEnemies = new List<Enemy>();

    /// <summary>
    /// Gets the list of currently active enemies in the game.
    /// This list is read-only to prevent external modifications.
    /// </summary>
    public IReadOnlyList<Enemy> GetActiveEnemies => _activeEnemies.AsReadOnly();

    /// <summary>
    /// Event invoked when there are no active enemies remaining in the pool.
    /// </summary>
    public Action OnNoActiveEnemies { get; set; }

    /// <summary>
    /// Initializes the enemy pools for each enemy type using the given enemy database.
    /// </summary>
    /// <param name="enemyDataBase">Database containing enemy configurations and prefabs.</param>
    public void Initialize(EnemyDatabase enemyDataBase)
    {
        GameObject parent = new GameObject("__POOL__ENEMY");
        _enemyPool = new Dictionary<EnemyData.EnemyType, ObjectPool<Enemy>>();

        foreach (var enemyData in enemyDataBase.GetConfigMap)
        {
            var pool = new ObjectPool<Enemy>(enemyData.Value.prefab, 10, parent.transform);
            _enemyPool.Add(enemyData.Key, pool);
        }
    }

    /// <summary>
    /// Retrieves a enemy of the specified type from the pool and activates it at the given position and parent.
    /// </summary>
    /// <param name="type">The type of enemy to get.</param>
    /// <param name="position">Spawn position for the enemy.</param>
    /// <param name="parent">Parent transform for the enemy.</param>
    /// <returns>The activated enemy instance or null if the type is not found.</returns>
    public Enemy GetEnemy(EnemyData.EnemyType type, Vector3 position, Transform parent)
    {
        if (_enemyPool.TryGetValue(type, out var pool))
        {
            Enemy enemy = pool.Get(position, parent);
            _activeEnemies.Add(enemy);
            return enemy;
        }

        Debug.LogError($"Enemy type {type} not found in pool.");
        return null;
    }

    /// <summary>
    /// Returns a enemy to its pool, removes it from the active enemies list,
    /// and invokes OnNoActiveEnemies if no active enemies remain.
    /// </summary>
    /// <param name="enemy">The enemy to return to the pool.</param>
    public void ReturnEnemy(Enemy enemy)
    {
        if (enemy == null) return;

        if (_enemyPool.TryGetValue(enemy.type, out var pool))
        {
            _activeEnemies.Remove(enemy);
            if (_activeEnemies.Count == 0)
            {
                OnNoActiveEnemies?.Invoke();
            }
            pool.ReturnToPool(enemy);
        }
        else
        {
            Debug.LogError("Enemy type not found in pool.");
        }
    }

    /// <summary>
    /// Returns all active enemies in all pools back to their respective pools and clears the active enemies list.
    /// </summary>
    public void ReturnAllEnemies()
    {
        foreach (var pool in _enemyPool.Values)
        {
            pool.ReturnAllToPool();
        }
        _activeEnemies.Clear();
    }

    /// <summary>
    /// Clears all enemy pools and active enemies list, releasing references and cleaning up memory.
    /// </summary>
    public void ClearPools()
    {
        foreach (var pool in _enemyPool.Values)
        {
            pool.Clear();
        }
        _enemyPool.Clear();
        _activeEnemies.Clear();
    }
}