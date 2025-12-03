using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private IEnemyPoolManager _enemyPoolManager;
    private ICurrencyManager _currencyManager;
    private EnemyDatabase _enemyDatabase;
    private Transform _baseTransform;

    /// <summary>
    /// Initializes the spawner with required dependencies.
    /// </summary>
    /// <param name="enemyPoolManager">Reference to the enemy pool manager.</param>
    /// <param name="enemyDatabase">Reference to the enemy database for configuration data.</param>
    /// <param name="baseTransform">Transform of the base that enemies move towards.</param>
    /// <param name="currencyManager">Reference to the currency manager for reward handling.</param>
    public void Initialize(IEnemyPoolManager enemyPoolManager, EnemyDatabase enemyDatabase, Transform baseTransform, ICurrencyManager currencyManager)
    {
        _enemyPoolManager = enemyPoolManager;
        _enemyDatabase = enemyDatabase;
        _baseTransform = baseTransform;
        _currencyManager = currencyManager;
    }

    /// <summary>
    /// Spawns a enemy of the specified type and initializes it.
    /// </summary>
    /// <param name="type">The type of enemy to spawn.</param>
    public void Spawn(EnemyData.EnemyType type)
    {
        EnemyData config = _enemyDatabase.GetConfig(type);
        Enemy enemy = _enemyPoolManager.GetEnemy(type, transform.position, null);
        enemy.OnDeath += OnEnemyDeath;
        enemy.OnReachedBase += OnEnemyReachedBase;
        enemy.Initialize(config, _baseTransform);
    }

    /// <summary>
    /// Event handler called when a enemy reaches the base.
    /// </summary>
    /// <param name="enemy">The enemy that reached the base.</param>
    private void OnEnemyReachedBase(Enemy enemy)
    {
        enemy.OnDeath -= OnEnemyDeath;
        enemy.OnReachedBase -= OnEnemyReachedBase;
        _enemyPoolManager.ReturnEnemy(enemy);
    }

    /// <summary>
    /// Event handler called when a enemy dies.
    /// Adds reward to currency, and returns enemy to pool.
    /// </summary>
    /// <param name="enemy">The enemy that died.</param>
    private void OnEnemyDeath(Enemy enemy)
    {
        enemy.OnDeath -= OnEnemyDeath;
        enemy.OnReachedBase -= OnEnemyReachedBase;

        EnemyData config = _enemyDatabase.GetConfig(enemy.type);
        _currencyManager.Add(config.reward);

        _enemyPoolManager.ReturnEnemy(enemy);
    }
}
