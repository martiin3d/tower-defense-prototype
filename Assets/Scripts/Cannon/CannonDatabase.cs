using System.Collections.Generic;
using UnityEngine;
using static CannonData;

[CreateAssetMenu(menuName = "Custom/Tools/Cannon/CannonDatabase")]
public class CannonDatabase : ScriptableObject
{
    [SerializeField] private List<CannonData> _cannonDataList;
    private Dictionary<CannonType, CannonData> _configMap;

    public IReadOnlyList<CannonData> CannonDataList => _cannonDataList;

    public IReadOnlyDictionary<CannonType, CannonData> GetConfigMap => _configMap;

    public void Initialize()
    {
        _configMap = new Dictionary<CannonType, CannonData>();
        for (int i = 0; i < _cannonDataList.Count; i++)
        {
            _configMap.Add(_cannonDataList[i].type, _cannonDataList[i]);
        }
    }

    public CannonData GetConfig(CannonType type)
    {
        return _configMap[type];
    }
}
