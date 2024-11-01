using UnityEngine;

namespace ShootEmUp
{
    public sealed class FireComponent : IFireComponent
    {
        private readonly CharacterType _characterType;
        private readonly BulletManager _bulletManager;
        private readonly Transform _firePoint;

        public FireComponent(BulletManager bulletManager, Transform firePoint, CharacterType characterType)
        {
            _bulletManager = bulletManager;
            _firePoint = firePoint;
            _characterType = characterType;
        }
        
        public void Fire(Vector2 direction)
        {
             _bulletManager.SpawnBullet(_characterType, _firePoint.position, direction);
        }
    }
}