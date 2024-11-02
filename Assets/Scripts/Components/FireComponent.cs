using UnityEngine;

namespace ShootEmUp
{
    public sealed class FireComponent : IFireComponent
    {
        private readonly CharacterType _enemyType;
        private readonly BulletManager _bulletManager;
        private readonly Transform _firePoint;

        public FireComponent(BulletManager bulletManager, Transform firePoint, CharacterType enemyType)
        {
            _bulletManager = bulletManager;
            _firePoint = firePoint;
            _enemyType = enemyType;
        }
        
        public void Fire(Vector2 direction)
        {
             _bulletManager.SpawnBullet(_enemyType, _firePoint.position, direction);
        }
    }
}