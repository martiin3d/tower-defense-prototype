using System;

/// <summary>
/// Manages the initialization and coordination of cannon placement, selection, and projectile creation systems.
/// Handles user cannon selection and delegates cannon placement logic.
/// </summary>
public class CannonsManager : ICannonsManager
{
    private CannonPlacer _placer;
    private UICannonSelector _selector;
    private IProjectileFactory _projectileFactory;
    private ICannonPoolManager _pool;
    private CannonDatabase _database;

    /// <summary>
    /// Initializes the CannonsManager with all required dependencies, including databases, pool managers, placer, selector, and factories.
    /// Sets up cannon selection and placement systems, and prepares projectile creation.
    /// </summary>
    /// <param name="database">The database containing cannon configurations.</param>
    /// <param name="pool">The cannon pool manager for handling cannon instances.</param>
    /// <param name="placer">The cannon placer responsible for placing cannons in the scene.</param>
    /// <param name="selector">The UI selector for cannon selection by the user.</param>
    /// <param name="currencyManager">The currency manager for handling player currency.</param>
    /// <param name="enemyPoolManager">The pool manager for enemy instances.</param>
    /// <param name="projectileDatabase">The database containing projectile configurations.</param>
    /// <param name="projectilePoolManager">The pool manager for projectile instances.</param>
    public void Initialize(CannonDatabase database, ICannonPoolManager pool, CannonPlacer placer, UICannonSelector selector, ICurrencyManager currencyManager, IEnemyPoolManager enemyPoolManager, ProjectileDatabase projectileDatabase, IProjectilePoolManager projectilePoolManager)
    {
        _database = database;
        _pool = pool;
        _placer = placer;
        _selector = selector;

        _selector.Initialize(_database, currencyManager);
        _selector.OnCannonSelected += OnCannonSelected;

        _projectileFactory = new ProjectileFactory(projectileDatabase, projectilePoolManager);
        _placer.Initialize(_pool, currencyManager, enemyPoolManager, _projectileFactory);
        _placer.OnPlaceRelease += OnCannonPlaceRelease;
    }

    private void OnCannonPlaceRelease()
    {
        _selector.ShowPlacingText(false);
    }

    private void OnCannonSelected(CannonData.CannonType type)
    {
        _selector.SetPlacingText(type);
        _selector.ShowPlacingText(true);
        _placer.SetCannonToPlace(_database.GetConfig(type));
    }

    /// <summary>
    /// Clear the preview cannon instance if it exists.
    /// </summary>
    public void ClearPreview()
    {
        _placer.ClearPreview();
    }
}