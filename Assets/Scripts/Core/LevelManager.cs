using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private int _startCoins;
    [SerializeField] private bool _removeCannonsWhenWaveFinish;
    [SerializeField] private bool _resetCoinsWhenWaveFinish;
    [SerializeField] private ProjectileDatabase _projectileDatabase;
    [SerializeField] private CannonDatabase _cannonDatabase;
    [SerializeField] private EnemyDatabase _enemyDatabase;
    [SerializeField] private List<EnemySpawner> _enemySpawners;
    [SerializeField] private WaveManager _waveManager;
    [SerializeField] private UICannonSelector _uICannonSelector;
    [SerializeField] private CannonPlacer _cannonPlacer;
    [SerializeField] private UIManager _uiManager;

    private IEnemyPoolManager _enemyPoolManager;
    private IProjectilePoolManager _projectilePoolManager;
    private ICannonPoolManager _cannonPoolManager;
    private ICannonsManager _cannonsManager;
    private ICurrencyManager _currencyManager;

    private void Awake()
    {
        _currencyManager = new CurrencyManager(_startCoins);

        InitializeUI();
        InitializeEnemies();
        InitializeProjectiles();
        InitializeCannons();

        _base.OnBaseDestroyed += OnBaseDestroyedEvent;

        _waveManager.Initialize(_enemySpawners);
    }

    private void Start()
    {
        _uiManager.StartReadySetGo(0);
    }

    private void OnEnable()
    {
        _uiManager.OnRetryButton += OnRetryButtonClick;
    }

    private void OnDisable()
    {
        _uiManager.OnRetryButton -= OnRetryButtonClick;
    }

    /// <summary>
    /// Called when the Ready-Set-Go UI sequence finishes to start the next wave.
    /// </summary>
    private void OnReadySetGoFinish()
    {
        _waveManager.StartNextWave();
    }

    /// <summary>
    /// Called when there are no active enemies on the field.
    /// Handles wave progression or game win conditions.
    /// </summary>
    private void OnNoActiveEnemiesEvent()
    {
        if (_waveManager.isCurrentWaveFinished)
        {
            _projectilePoolManager.ReturnAllProjectile();
            _cannonsManager.ClearPreview();

            if (!_waveManager.hasMoreWaves)
            {
                _uiManager.Win();
            }
            else
            {
                if (_removeCannonsWhenWaveFinish)
                {
                    _cannonPoolManager.ReturnAllCannons();
                }

                if (_resetCoinsWhenWaveFinish)
                {
                    _currencyManager.Reset();
                }

                _uiManager.StartReadySetGo(_waveManager.currentWave);
            }
        }
    }

    /// <summary>
    /// Handles retry button click: resets game state and starts again.
    /// </summary>
    private void OnRetryButtonClick()
    {
        _waveManager.Reset();
        _projectilePoolManager.ReturnAllProjectile();
        _cannonPoolManager.ReturnAllCannons();
        _enemyPoolManager.ReturnAllEnemies();
        _currencyManager.Reset();
        _base.ResetValues();
        _cannonPlacer.ClearPreview();

        _uiManager.StartReadySetGo(0);
    }

    /// <summary>
    /// Initializes the enemy database and pool manager, and sets up spawners.
    /// </summary>
    private void InitializeEnemies()
    {
        _enemyDatabase.Initialize();

        _enemyPoolManager = new EnemyPoolManager();
        _enemyPoolManager.OnNoActiveEnemies += OnNoActiveEnemiesEvent;
        _enemyPoolManager.Initialize(_enemyDatabase);

        foreach (var spawner in _enemySpawners)
        {
            spawner.Initialize(_enemyPoolManager, _enemyDatabase, _base.transform, _currencyManager);
        }
    }

    /// <summary>
    /// Initializes the projectile database and pool manager.
    /// </summary>
    private void InitializeProjectiles()
    {
        _projectileDatabase.Initialize();
        _projectilePoolManager = new ProjectilePoolManager();
        _projectilePoolManager.Initialize(_projectileDatabase);
    }

    /// <summary>
    /// Initializes the cannon database, pool manager, and cannons manager.
    /// </summary>
    private void InitializeCannons()
    {
        _cannonDatabase.Initialize();
        _cannonPoolManager = new CannonPoolManager();
        _cannonPoolManager.Initialize(_cannonDatabase);

        _cannonsManager = new CannonsManager();
        _cannonsManager.Initialize(_cannonDatabase, _cannonPoolManager, _cannonPlacer, _uICannonSelector, _currencyManager, _enemyPoolManager, _projectileDatabase, _projectilePoolManager);
    }

    /// <summary>
    /// Initializes the UI Manager.
    /// </summary>
    private void InitializeUI()
    {
        _uiManager.Initialize(_currencyManager);
        _uiManager.OnReadySetGoFinish += OnReadySetGoFinish;
    }

    /// <summary>
    /// Called when the base is destroyed. Stops waves and triggers lose state.
    /// </summary>
    private void OnBaseDestroyedEvent()
    {
        _waveManager.Stop();
        _uiManager.Lose();
        _enemyPoolManager.ReturnAllEnemies();
        _cannonPoolManager.ReturnAllCannons();
    }

    private void OnDestroy()
    {
        _base.OnBaseDestroyed -= OnBaseDestroyedEvent;
        _enemyPoolManager.ReturnAllEnemies();
    }
}
