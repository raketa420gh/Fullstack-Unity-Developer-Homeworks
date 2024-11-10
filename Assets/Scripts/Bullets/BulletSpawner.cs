using System;
using System.Linq;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class BulletSpawner : MonoBehaviour
    {
        [SerializeField]
        private BulletPool _bulletsPool;
        [SerializeField]
        private LevelBounds _levelBounds;
        [SerializeField] 
        private BulletData[] _bulletDatabase;

        private void Awake()
        {
            _bulletsPool.CreatePool();
        }

        public void SpawnBulletByType(CharacterType characterType, Vector3 position, Vector2 direction)
        {
            BulletData bulletData = _bulletDatabase.FirstOrDefault(data => characterType == data.EnemyType);
            
            SpawnBullet(bulletData, position, direction);
        }

        private void SpawnBullet(BulletData data, Vector3 position, Vector2 direction)
        {
            Bullet bullet = _bulletsPool.Pool.GetFreeElement();
            bullet.SetBulletData(data);
            bullet.SetPosition(position);
            bullet.SetColor(data.Color);
            bullet.SetLayer(data.PhysicsLayerIndex);
            bullet.SetDirection(direction);
        }

        private void FixedUpdate()
        {
            foreach (Bullet bullet in _bulletsPool.Pool.PooledObjects.Where(bullet => !_levelBounds.InBounds(bullet.transform.position)))
                bullet.gameObject.SetActive(false);
        }
    }
}