using UnityEngine;

namespace ShootEmUp
{
    public sealed class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        private Transform _container;
        [SerializeField]
        private EnemyPool _enemiesPool;
        [SerializeField]
        private Transform[] _spawnPositions;
        [SerializeField]
        private Transform[] _attackPositions;
        [SerializeField] 
        private Player _player;
        [SerializeField] 
        private BulletManager _bulletManager;

        public void Initialize()
        {
            _enemiesPool.CreatePool();
        }

        public Enemy SpawnEnemyAtRandomPoint()
        {
            Enemy enemy = _enemiesPool.Pool.GetFreeElement();
            enemy.Create(_bulletManager);
            enemy.transform.position = GetRandomSpawnPoint().position;
            enemy.transform.SetParent(_container);
            enemy.SetDestination(GetRandomAttackPoint().position);
            enemy.SetTarget(_player);

            return enemy;
        }

        private Transform GetRandomSpawnPoint()
        {
            int index = Random.Range(0, _spawnPositions.Length);
            return _spawnPositions[index];
        }

        private Transform GetRandomAttackPoint()
        {
            int index = Random.Range(0, _attackPositions.Length);
            return _attackPositions[index];
        }
    }
}