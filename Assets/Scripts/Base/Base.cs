using System;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField]
    private int _maxHp = 10;

    [SerializeField]
    private MeshFilter _meshFilter;

    [SerializeField]
    private Mesh _intactMesh;

    [SerializeField]
    private Mesh _damagedMesh;

    [SerializeField]
    private Mesh _criticalMesh;

    [SerializeField]
    private UILifeBar _uiLifeBar;

    private BaseState _baseState;

    public enum BaseState
    {         
        Intact,
        Damaged,
        Critical,
    }

    private int _hp;

    /// <summary>
    /// Invoked when the player's base is destroyed.
    /// Systems that need to respond (e.g., game over handling) should subscribe to this action.
    /// </summary>
    public Action OnBaseDestroyed;

    private void OnEnable()
    {
        ResetValues();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Enemy"))
        {
            if (other.transform.root.TryGetComponent<Enemy>(out Enemy enemy))
            {
                _hp -= enemy.hitDamage;
                _uiLifeBar.UpdateLife(_hp, _maxHp);
                enemy.HandleBaseArrival();
            }

            if (_hp > _maxHp * 0.2f && _hp <= _maxHp * 0.5f && _baseState != BaseState.Damaged)
            {
                SetState(BaseState.Damaged);
            }
            else if (_hp <= _maxHp * 0.2f && _baseState != BaseState.Critical)
            {
                SetState(BaseState.Critical);
            }

            if (_hp <= 0)
            {
                Debug.Log("Base destroyed!");
                OnBaseDestroyed?.Invoke();
                gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Resets the base's health to maximum, updates the UI life bar, and ensures the base is active.
    /// </summary>
    public void ResetValues()
    {
        _hp = _maxHp;
        _baseState = BaseState.Intact;
        _uiLifeBar.UpdateLife(_hp, _maxHp);
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Updates the base's state to the specified value and applies the corresponding mesh representation.
    /// </summary>
    /// <remarks>Calling this method changes the visual appearance of the object by updating its mesh to match
    /// the specified state.</remarks>
    /// <param name="newState">The new state to set for the object. Determines which mesh will be displayed.</param>
    private void SetState(BaseState newState)
    {
        _baseState = newState;
        switch(_baseState)
        {
            case BaseState.Intact:
                _meshFilter.mesh = _intactMesh;
                break;
            case BaseState.Damaged:
                _meshFilter.mesh = _damagedMesh;
                break;
            case BaseState.Critical:
                _meshFilter.mesh = _criticalMesh;
                break;
        }
    }
}
