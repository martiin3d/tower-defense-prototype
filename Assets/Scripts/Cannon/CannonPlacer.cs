using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Manages the placement of cannons in the game world.
/// Handles preview visualization and placement cost validation.
/// </summary>
public class CannonPlacer : MonoBehaviour
{
    [SerializeField] private LayerMask placeableMask;

    private Cannon _previewInstance;
    private ICannonPoolManager _cannonPool;
    private ICurrencyManager _currencyManager;
    private IProjectileFactory _projectileFactory;
    private IClosestTargetingSystem _closestTargetingSystem;

    public Action OnPlaceRelease;

#if (!UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS))
    private bool _placing = false;
#endif

    /// <summary>
    /// Initializes the cannon placer with required dependencies.
    /// </summary>
    /// <param name="cannonPool">Pool manager for cannon instances.</param>
    /// <param name="currencyManager">Manager for in-game currency.</param>
    /// <param name="enemyPoolManager">Pool manager for enemies, used for targeting system.</param>
    /// <param name="projectileFactory">Factory to create projectiles.</param>
    public void Initialize(ICannonPoolManager cannonPool, ICurrencyManager currencyManager, IEnemyPoolManager enemyPoolManager, IProjectileFactory projectileFactory)
    {
        _cannonPool = cannonPool;
        _currencyManager = currencyManager;
        _projectileFactory = projectileFactory;
        _closestTargetingSystem = new ClosestTargetingSystem(enemyPoolManager);
    }

    private void Update()
    {
        if (_previewInstance != null)
        {
            if (EventSystem.current.IsPointerOverGameObject()) //avoid placing cannon when UI is interacted with
            {
                return;
            }

            LayerMask raycastMask = placeableMask | LayerMask.GetMask("Obstacle");
#if (!UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS))
            if (Input.touchCount > 0)
            {
                if (!_placing)
                {
                    _placing = true;
                }
                else
                {
                    Touch touch = Input.GetTouch(0);
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    if (Physics.Raycast(ray, out var hit, 100f, raycastMask))
                    {
                        if (((1 << hit.collider.gameObject.layer) & placeableMask) != 0)
                        {
                            Vector3 placePos = hit.point;
                            PlaceCannon(placePos);
                        }
                    }
                }
            }
#else
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 100f, raycastMask))
            {
                if (((1 << hit.collider.gameObject.layer) & placeableMask) != 0)
                {
                    Vector3 placePos = hit.point;
                    _previewInstance.transform.position = placePos;

                    if (Input.GetMouseButtonDown(0))
                    {
                        PlaceCannon(placePos);
                    }
                }
            }
#endif
        }
    }

    /// <summary>
    /// Sets the cannon type to place, creating or updating the preview instance if enough currency is available.
    /// </summary>
    /// <param name="data">Data configuration for the cannon to place.</param>
    public void SetCannonToPlace(CannonData data)
    {
        if (!_currencyManager.HasEnough(data.cost))
        {
            Debug.Log("Not enough currency to place cannon.");
            return;
        }

        if (_previewInstance != null && _previewInstance.type != data.type)
        {
            _cannonPool.ReturnCannon(_previewInstance);
            _previewInstance = null;
        }

        if (_previewInstance == null)
        {
            _previewInstance = _cannonPool.GetCannon(data.type, Vector3.zero, null);
            _previewInstance.Initialize(data, _projectileFactory, _closestTargetingSystem);
            _previewInstance.SetAsPreview();
        }
    }

    /// <summary>
    /// Finalizes the placement of the cannon at the specified position, deducting cost and disabling preview mode.
    /// </summary>
    /// <param name="position">World position to place the cannon.</param>
    private void PlaceCannon(Vector3 position)
    {
#if (!UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS))
        _placing = false;
#endif
        _previewInstance.transform.position = position;
        _currencyManager.Spend(_previewInstance.cost);
        _previewInstance.SetAsNormal();
        _previewInstance = null;
        OnPlaceRelease?.Invoke();
    }

    /// <summary>
    /// Clear the preview cannon instance if it exists.
    /// </summary>
    public void ClearPreview()
    {
        if (_previewInstance != null)
        {
            _cannonPool.ReturnCannon(_previewInstance);
            _previewInstance = null;
            OnPlaceRelease?.Invoke();
        }
    }
}
