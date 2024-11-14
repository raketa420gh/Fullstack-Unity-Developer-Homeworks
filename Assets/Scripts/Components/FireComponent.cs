using System;
using UnityEngine;

namespace ShootEmUp
{
    [Serializable]
    public sealed class FireComponent : IFireComponent
    {
        public Transform FirePoint => _firePoint;
        
        [SerializeField]
        private CharacterType _enemyType;

        [SerializeField]
        private Transform _firePoint;

        private BulletSpawner _bulletSpawner;

        public void Set(BulletSpawner bulletSpawner)
        {
            _bulletSpawner = bulletSpawner;
        }

        public void Fire(Vector2 direction)
        {
             _bulletSpawner.Spawn(_enemyType, _firePoint.position, direction);
        }
    }
}