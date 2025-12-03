using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Represents an enemy unit that moves toward the player's base.
/// Handles movement, damage, death logic, and communicates events to external systems.
/// </summary>
public class Enemy : MonoBehaviour
{
    [SerializeField]
    private UILifeBar _uiLifeBar;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private float _deadDurationTime = .9f;

    /// <summary>
    /// Triggered when the enemy dies.
    /// </summary>
    public event Action<Enemy> OnDeath;

    /// <summary>
    /// Triggered when the enemy reaches the player's base.
    /// </summary>
    public event Action<Enemy> OnReachedBase;

    public EnemyData.EnemyType type => _enemyData.type;
    public int hitDamage => _enemyData.hitDamage;
    public bool isAlive => _hp > 0 && !_isDying;
    private EnemyData _enemyData;

    private float _currentSpeed;
    private Transform _baseTransform;
    private float _hp;
    private float _slowDownTimer;
    private bool _isDying;

    /// <summary>
    /// Initializes the enemy with its data, target base transform, and default movement state.
    /// </summary>
    /// <param name="enemyData">The configuration data defining the enemy's stats and behavior.</param>
    /// <param name="baseTransform">The transform of the player's base, used as the enemy's destination.</param>
    public void Initialize(EnemyData enemyData, Transform baseTransform)
    {
        _enemyData = enemyData;
        _isDying = false;
        _animator.SetBool("Dead", _isDying);
        _hp = _enemyData.hp;
        _baseTransform = baseTransform;
        transform.LookAt(_baseTransform);
        SetNormalSpeed();
        _uiLifeBar.UpdateLife(_hp, _enemyData.hp);
    }

    private void Update()
    {
        if (isAlive)
        {
            transform.position = Vector3.MoveTowards(transform.position, _baseTransform.position, _currentSpeed * Time.deltaTime);

            if (_slowDownTimer > 0f)
            {
                _slowDownTimer -= Time.deltaTime;
                if (_slowDownTimer <= 0f)
                {
                    SetNormalSpeed();
                }
            }
        }
    }


    /// <summary>
    /// Reduces the enemy's HP by the given amount and triggers death if HP falls to zero.
    /// Updates the UI life bar accordingly.
    /// </summary>
    /// <param name="amount">The amount of damage to apply.</param>
    public void TakeDamage(float amount)
    {
        _hp -= amount;
        if (_hp <= 0f)
        {
            if (!_isDying)
            {
                _isDying = true;
                StartCoroutine(Die());
            }
        }
        else
        {
            _uiLifeBar.UpdateLife(_hp, _enemyData.hp);
        }
    }

    /// <summary>
    /// Temporarily reduces the enemy's speed by a given factor for a specified duration.
    /// </summary>
    /// <param name="slowFactor">The factor to slow down the enemy's speed (e.g., 0.5 for 50% speed).</param>
    /// <param name="duration">The duration of the slowdown effect in seconds.</param>
    public void ApplySlow(float slowFactor, float duration)
    {
        _currentSpeed = _enemyData.speed * slowFactor;
        _slowDownTimer = duration;
    }

    /// <summary>
    /// Restores the enemy's speed to its normal value and resets the slowdown timer.
    /// </summary>
    private void SetNormalSpeed()
    {
        _slowDownTimer = 0;
        _currentSpeed = _enemyData.speed;
    }

    /// <summary>
    /// Notifies listeners that the enemy has reached the base.
    /// </summary>
    public void HandleBaseArrival()
    {
        OnReachedBase?.Invoke(this);
    }

    /// <summary>
    /// Triggers the enemy's death event.
    /// </summary>
    private IEnumerator Die()
    {
        _uiLifeBar.UpdateLife(0, _enemyData.hp);
        _animator.SetBool("Dead", _isDying);
        yield return new WaitForSeconds(_deadDurationTime);
        OnDeath?.Invoke(this);
    }
}
