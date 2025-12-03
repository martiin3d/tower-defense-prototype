using UnityEngine;

/// <summary>
/// Represents a cannon that targets enemies and fires projectiles at them.
/// Supports preview mode to visually distinguish placement before activation.
/// </summary>
public class Cannon : MonoBehaviour
{
    [SerializeField] private GameObject _original;
    [SerializeField] private GameObject _preview;
    [SerializeField] private Transform _projectileSpawnPoint;

    private CannonData _data;
    private IProjectileFactory _projectileFactory;
    private IClosestTargetingSystem _closestTargetingSystem;
    private float _fireTimer;
    private bool _isPreview;

    public CannonData.CannonType type => _data.type;
    public int cost => _data.cost;
    private Enemy _target;

    /// <summary>
    /// Initializes the cannon with its configuration, projectile factory, and targeting system.
    /// </summary>
    /// <param name="data">Cannon configuration data.</param>
    /// <param name="projectileFactory">Factory used to create projectiles.</param>
    /// <param name="closestTargetingSystem">System to acquire the closest target enemy.</param>
    public void Initialize(CannonData data, IProjectileFactory projectileFactory, IClosestTargetingSystem closestTargetingSystem)
    {
        _isPreview = true;
        _data = data;
        _projectileFactory = projectileFactory;
        _closestTargetingSystem = closestTargetingSystem;
    }

    private void Update()
    {
        if (!_isPreview)
        {
            _fireTimer += Time.deltaTime;

            if (_target == null || !_target.isAlive || !_target.gameObject.activeInHierarchy)
            {
                _target = _closestTargetingSystem.GetTarget(transform.position, _data.range);
            }

            if (_target != null)
            {
                Vector3 direction = _target.transform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

                if (_fireTimer >= 1f / _data.fireRate)
                {
                    _projectileFactory.Create(_data.projectileType, _projectileSpawnPoint.position, _target.transform.position);
                    _fireTimer = 0f;
                }
            }
        }
    }

    /// <summary>
    /// Sets the cannon to preview mode.
    /// </summary>
    public void SetAsPreview()
    {
        _isPreview = true;
        _original.SetActive(false);
        _preview.SetActive(true);
    }

    /// <summary>
    /// Sets the cannon back to normal mode.
    /// </summary>
    public void SetAsNormal()
    {
        _original.SetActive(true);
        _preview.SetActive(false);
        _isPreview = false;
    }
}