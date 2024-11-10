using System.Collections.Generic;
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
        
        private readonly List<Bullet> _activeBullets = new List<Bullet>();

        private void Awake()
        {
            _activeBullets.Clear();
            _bulletsPool.CreatePool();
        }

        public void SpawnBulletByType(CharacterType characterType, Vector3 position, Vector2 direction)
        {
            BulletData bulletData = _bulletDatabase.FirstOrDefault(data => characterType == data.EnemyType);
            
            Bullet bullet = SpawnBullet(bulletData, position, direction);
            _activeBullets.Add(bullet);
            bullet.OnDead += HandleBulletDeadEvent;
        }

        private Bullet SpawnBullet(BulletData data, Vector3 position, Vector2 direction)
        {
            Bullet bullet = _bulletsPool.GetFromPool();
            bullet.SetBulletData(data);
            bullet.SetPosition(position);
            bullet.SetColor(data.Color);
            bullet.SetLayer(data.PhysicsLayerIndex);
            bullet.SetDirection(direction);
            return bullet;
        }

        private void FixedUpdate()
        {
            foreach (Bullet bullet in _activeBullets.Where(bullet => !_levelBounds.InBounds(bullet.transform.position)))
                bullet.gameObject.SetActive(false);
        }

        private void HandleBulletDeadEvent(Bullet bullet)
        {
            _activeBullets.Remove(bullet);
        }
    }
}