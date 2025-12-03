using UnityEngine;

public interface IProjectilePoolManager
{
    void Initialize(ProjectileDatabase projectileDataBase);
    Projectile GetProjectile(ProjectileData.ProjectileType type, Vector3 position, Transform parent);
    void ReturnAllProjectile();
    void ReturnProjectile(Projectile projectile);
    void ClearPools();
}