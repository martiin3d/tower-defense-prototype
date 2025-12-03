using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<WaveData> _waves;

    private List<EnemySpawner> _spawners;
    private int _currentWave = 0;

    /// <summary>
    /// Gets the index of the current wave.
    /// </summary>
    public int currentWave => _currentWave;

    /// <summary>
    /// Indicates whether the current wave has finished spawning all enemies.
    /// </summary>
    public bool isCurrentWaveFinished { private set; get; }

    /// <summary>
    /// Indicates whether there are more waves remaining to be spawned.
    /// </summary>
    public bool hasMoreWaves => _currentWave < _waves.Count;

    /// <summary>
    /// Initializes the WaveManager with a list of available spawners.
    /// </summary>
    /// <param name="spawnerList">List of enemy spawners.</param>
    public void Initialize(List<EnemySpawner> spawnerList)
    {
        _spawners = spawnerList;
    }
    /// <summary>
    /// Starts the next wave of enemies if available.
    /// </summary>
    public void StartNextWave()
    {
        if (!hasMoreWaves)
            return;

        isCurrentWaveFinished = false;
        var wave = _waves[_currentWave];
        StartCoroutine(SpawnWaveCoroutine(wave));
        _currentWave++;
    }

    /// <summary>
    /// Stops the wave spawning coroutine.
    /// </summary>
    public void Stop()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// Resets the wave manager state.
    /// </summary>
    public void Reset()
    {
        Stop();
        _currentWave = 0;
    }

    /// <summary>
    /// Coroutine that spawns enemies randomly from the current wave's enemy groups,
    /// distributing them randomly among available spawners with delays between spawns.
    /// </summary>
    /// <param name="wave">Wave data containing enemy groups and spawn timing.</param>
    /// <returns>IEnumerator for coroutine execution.</returns>
    private IEnumerator SpawnWaveCoroutine(WaveData wave)
    {
        List<EnemyData.EnemyType> enemiesToSpawn = new List<EnemyData.EnemyType>();

        foreach (EnemySpawnGroup group in wave.enemyGroups)
        {
            for (int i = 0; i < group.amount; i++)
            {
                enemiesToSpawn.Add(group.type);
            }
        }

        while (enemiesToSpawn.Count > 0)
        {
            int index = Random.Range(0, enemiesToSpawn.Count);
            EnemyData.EnemyType enemyType = enemiesToSpawn[index];
            enemiesToSpawn.RemoveAt(index);

            EnemySpawner spawner = _spawners[Random.Range(0, _spawners.Count)];
            spawner.Spawn(enemyType);
            yield return new WaitForSeconds(wave.timeBetweenSpawns);
        }

        isCurrentWaveFinished = true;
    }
}