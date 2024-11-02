using System.Linq;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class BulletManager : MonoBehaviour
    {
        [SerializeField]
        private BulletSpawner _bulletSpawner;
        [SerializeField]
        private LevelBounds _levelBounds;
        [SerializeField] 
        private BulletData[] _bulletDatabase;

        private void Awake()
        {
            _bulletSpawner.Initialize(_levelBounds);
        }

        public void SpawnBullet(CharacterType characterType, Vector3 position, Vector2 direction)
        {
            BulletData bulletData = _bulletDatabase.FirstOrDefault(data => characterType == data.EnemyType);
            
            _bulletSpawner.SpawnBullet(bulletData, position, direction);
        }
    }
}