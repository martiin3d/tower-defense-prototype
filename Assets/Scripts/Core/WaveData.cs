using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Tools/Wave")]
public class WaveData : ScriptableObject
{
    public List<EnemySpawnGroup> enemyGroups;
    public float timeBetweenSpawns = 0.5f;
}
