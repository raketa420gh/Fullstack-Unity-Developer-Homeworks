﻿using System.Linq;
using ShootEmUp;
using UnityEngine;

namespace New
{
    public sealed class BulletSpawner : MonoBehaviour
    {
        [SerializeField]
        private BulletPool _bulletsPool;
        
        private LevelBounds _levelBounds;

        public void Initialize(LevelBounds levelBounds)
        {
            _levelBounds = levelBounds;
            _bulletsPool.CreatePool();
        }

        public void SpawnBullet(BulletData data, Vector3 position, Vector2 direction)
        {
            Bullet bullet = _bulletsPool.Pool.GetFreeElement();
            bullet.SetPosition(position);
            bullet.SetColor(data.Color);
            bullet.SetLayer(data.PhysicsLayerIndex);
            bullet.SetDamage(data.Damage);
            bullet.SetDirection(direction);
        }

        private void FixedUpdate()
        {
            foreach (Bullet bullet in _bulletsPool.Pool.PooledObjects.Where(bullet => !_levelBounds.InBounds(bullet.transform.position)))
                bullet.gameObject.SetActive(false);
        }
    }
}