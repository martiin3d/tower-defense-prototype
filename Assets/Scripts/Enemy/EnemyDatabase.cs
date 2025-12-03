using System.Collections.Generic;
using UnityEngine;
using static EnemyData;

[CreateAssetMenu(menuName = "Custom/Tools/Enemy/EnemyDatabase")]
public class EnemyDatabase : ScriptableObject
{
    [SerializeField] private List<EnemyData> _enemyDataList;
    private Dictionary<EnemyType, EnemyData> _configMap;

    public IReadOnlyList<EnemyData> EnemyDataList => _enemyDataList;

    public IReadOnlyDictionary<EnemyType, EnemyData> GetConfigMap => _configMap;

    public void Initialize()
    {
        _configMap = new Dictionary<EnemyType, EnemyData>();
        for (int i = 0; i < _enemyDataList.Count; i++)
        {
            _configMap.Add(_enemyDataList[i].type, _enemyDataList[i]);
        }
    }

    public EnemyData GetConfig(EnemyType type)
    {
        return _configMap[type];
    }
}