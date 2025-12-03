using UnityEngine;
using static ProjectileData;

[CreateAssetMenu(menuName = "Custom/Tools/Cannon/CannonData")]
public class CannonData : ScriptableObject
{
    public enum CannonType
    {
        Normal,
        Freeze,
    }

    public Cannon prefab;
    [SerializeField] private CannonType _type;
    [SerializeField] private ProjectileType _projectileType;
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _cost;
    [SerializeField] private float _fireRate;
    [SerializeField] private float _range;

    public CannonType type => _type;
    public ProjectileType projectileType => _projectileType;
    public Sprite icon => _icon;
    public int cost => _cost;
    public float fireRate => _fireRate;
    public float range => _range;
}
