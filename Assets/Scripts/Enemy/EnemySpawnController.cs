using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class EnemySpawnController : MonoBehaviour
    {
        [SerializeField] 
        private EnemySpawner _enemySpawner;

        [SerializeField] 
        private int _countOfEnemiesSameTime = 5;

        [SerializeField] 
        private bool _isSpawningEnabled = true;

        [SerializeField] 
        private int _minDelay = 1;
        
        [SerializeField] 
        private int _maxDelay = 2;

        private readonly List<EnemyBehaviour> _activeEnemies = new List<EnemyBehaviour>();

        private void Awake()
        {
            _activeEnemies.Clear();
            _enemySpawner.Initialize();
        }

        private IEnumerator Start()
        {
            while (_isSpawningEnabled)
            {
                yield return new WaitForSeconds(Random.Range(_minDelay, _maxDelay));

                if (_activeEnemies.Count <= _countOfEnemiesSameTime)
                {
                    EnemyBehaviour enemyBehaviour = _enemySpawner.SpawnEnemyAtRandomPoint();
                    _activeEnemies.Add(enemyBehaviour);
                    enemyBehaviour.OnDead += HandleEnemyDeadEvent;
                }
            }
        }

        private void HandleEnemyDeadEvent(EnemyBehaviour enemy)
        {
            if (!enemy)
                return;

            enemy.OnDead -= HandleEnemyDeadEvent;
            enemy.Dispose();
            _activeEnemies.Remove(enemy);
        }
    }
}