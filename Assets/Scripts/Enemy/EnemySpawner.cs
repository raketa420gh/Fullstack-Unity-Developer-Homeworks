using UnityEngine;
using Random = UnityEngine.Random;

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
        private BulletSpawner _bulletSpawner;

        public void Initialize()
        {
            _enemiesPool.CreatePool();
        }

        public EnemyBehaviour SpawnEnemyAtRandomPoint()
        {
            EnemyBehaviour enemyBehaviour = _enemiesPool.GetFromPool();
            enemyBehaviour.transform.position = GetRandomSpawnPoint().position;
            enemyBehaviour.transform.SetParent(_container);
            enemyBehaviour.SetDestination(GetRandomAttackPoint().position);
            enemyBehaviour.SetTarget(_player);

            return enemyBehaviour;
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