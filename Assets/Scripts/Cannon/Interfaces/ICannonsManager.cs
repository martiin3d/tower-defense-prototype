/// <summary>
/// Manages the initialization and coordination of cannon placement, selection, and projectile creation systems.
/// Handles user cannon selection and delegates cannon placement logic.
/// </summary>
public interface ICannonsManager
{
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
    void Initialize(CannonDatabase database, ICannonPoolManager pool, CannonPlacer placer, UICannonSelector selector, ICurrencyManager currencyManager, IEnemyPoolManager enemyPoolManager, ProjectileDatabase projectileDatabase, IProjectilePoolManager projectilePoolManager);

    /// <summary>
    /// Clear the preview cannon instance if it exists.
    /// </summary>
    void ClearPreview();
}