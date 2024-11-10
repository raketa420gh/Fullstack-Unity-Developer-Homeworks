using UnityEngine;

namespace ShootEmUp
{
    public sealed class FireComponent : IFireComponent
    {
        private readonly CharacterType _enemyType;
        private readonly BulletSpawner _bulletSpawner;
        private readonly Transform _firePoint;

        public FireComponent(BulletSpawner bulletSpawner, Transform firePoint, CharacterType enemyType)
        {
            _bulletSpawner = bulletSpawner;
            _firePoint = firePoint;
            _enemyType = enemyType;
        }
        
        public void Fire(Vector2 direction)
        {
             _bulletSpawner.SpawnBulletByType(_enemyType, _firePoint.position, direction);
        }
    }
}