using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Tools/Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    public enum EnemyType
    {
        Normal,
        Big,
    }

    public Enemy prefab;
    [SerializeField] private EnemyType _type;
    [SerializeField] private int _hp;
    [SerializeField] private float _speed;
    [SerializeField] private int _reward;
    [SerializeField] private int _hitDamage;

    public EnemyType type => _type;
    public int hp => _hp;
    public float speed => _speed;
    public int reward => _reward;
    public int hitDamage => _hitDamage;
}