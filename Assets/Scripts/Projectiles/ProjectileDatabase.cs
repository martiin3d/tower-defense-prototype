using System.Collections.Generic;
using UnityEngine;
using static ProjectileData;

[CreateAssetMenu(menuName = "Custom/Tools/Projectile/ProjectileDatabase")]
public class ProjectileDatabase : ScriptableObject
{
    [SerializeField] private List<ProjectileData> _projectileDataList;
    private Dictionary<ProjectileType, ProjectileData> _configMap;

    public IReadOnlyList<ProjectileData> ProjectileDataList => _projectileDataList;

    public IReadOnlyDictionary<ProjectileType, ProjectileData> GetConfigMap => _configMap;

    /// <summary>
    /// Initializes the projectile database.
    /// Must be called before using GetConfig().
    /// </summary>
    public void Initialize()
    {
        _configMap = new Dictionary<ProjectileType, ProjectileData>();
        for (int i = 0; i < _projectileDataList.Count; i++)
        {
            _configMap.Add(_projectileDataList[i].type, _projectileDataList[i]);
        }
    }

    /// <summary>
    /// Returns the configuration for a given projectile type.
    /// </summary>
    /// <param name="type">The type of projectile.</param>
    /// <returns>The corresponding ProjectileData.</returns>
    public ProjectileData GetConfig(ProjectileType type)
    {
        return _configMap[type];
    }
}
