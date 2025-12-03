using System;
using UnityEngine;

/// <summary>
/// Represents a projectile that moves towards a target direction,
/// applies damage and optional slow effect to enemies upon collision,
/// and notifies when it should be returned to the object pool after a timeout or impact.
/// </summary>
public class Projectile : MonoBehaviour
{
    private Vector3 _targetDirection;
    private float _returnToPoolTime = 5f;
    private float _returnTimer;
    private ProjectileData _data;

    public ProjectileData.ProjectileType type { private set; get; }

    /// <summary>
    /// Event invoked when the projectile should be returned to the pool.
    /// </summary>
    public Action<Projectile> RequestReturnToPool;

    /// <summary>
    /// Initializes the projectile with the given data and target position.
    /// Calculates the direction towards the target.
    /// </summary>
    /// <param name="data">Projectile data containing speed, damage, and effects.</param>
    /// <param name="target">The target position to move towards.</param>
    public void Initialize(ProjectileData data, Vector3 target)
    {
        _data = data;
        _targetDirection = (target - transform.position).normalized;
    }

    private void Update()
    {
        _returnTimer += Time.deltaTime;
        transform.position += _targetDirection * _data.speed * Time.deltaTime;

        if (_returnTimer >= _returnToPoolTime)
        {
            _returnTimer = 0;
            RequestReturnToPool?.Invoke(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.TakeDamage(_data.damage);

            if (enemy.isAlive && _data.isFreeze)
            {
                enemy.ApplySlow(_data.slowFactor, _data.slowDuration);
            }

            _returnTimer = 0;
            RequestReturnToPool?.Invoke(this);
        }
    }
}
