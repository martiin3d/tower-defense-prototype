using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Tools/Projectile/ProjectileData")]
public class ProjectileData : ScriptableObject
{
    public enum ProjectileType
    {
        Normal,
        Freeze,
    }

    public Projectile prefab;
    [SerializeField] private ProjectileType _type;
    [SerializeField] private float _slowFactor;
    [SerializeField] private float _slowDuration;
    [SerializeField] private float _damage;
    [SerializeField] private float _speed;

    public ProjectileType type => _type;
    public bool isFreeze => _slowFactor > 0 && _slowDuration > 0;
    public float slowFactor => _slowFactor;
    public float slowDuration => _slowDuration;
    public float damage => _damage;
    public float speed => _speed;

    public MonoBehaviour Prefab => prefab;
}