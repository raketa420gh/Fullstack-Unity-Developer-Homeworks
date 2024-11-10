using System.Collections;
using System.Collections.Generic;
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
        private BulletSpawner _bulletSpawner;
        [SerializeField] 
        private int _countOfEnemiesSameTime = 5;
        [SerializeField] 
        private bool _isSpawningEnabled = true;

        private readonly List<Enemy> _activeEnemies = new List<Enemy>();

        private void Awake()
        {
            _activeEnemies.Clear();
            _enemiesPool.CreatePool();
        }

        private IEnumerator Start()
        {
            while (_isSpawningEnabled)
            {
                yield return new WaitForSeconds(Random.Range(1, 2));

                if (_activeEnemies.Count <= _countOfEnemiesSameTime)
                {
                    Enemy enemy = SpawnEnemyAtRandomPoint();
                    _activeEnemies.Add(enemy);
                    enemy.OnDead += HandleEnemyDeadEvent;
                }
            }
        }

        private Enemy SpawnEnemyAtRandomPoint()
        {
            Enemy enemy = _enemiesPool.GetFromPool();
            enemy.Create(_bulletSpawner);
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
        
        private void HandleEnemyDeadEvent(Ship ship)
        {
            if (ship is not Enemy enemy)
                return;

            enemy.OnDead -= HandleEnemyDeadEvent;
            enemy.Dispose();
            _activeEnemies.Remove(enemy);
        }
    }
}