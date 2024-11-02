using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ShootEmUp
{
    public sealed class EnemyManager : MonoBehaviour
    {
        [SerializeField] 
        private EnemySpawner _enemySpawner;
        [SerializeField] 
        private int _countOfEnemiesSameTime = 5;
        [SerializeField] 
        private bool _isSpawningEnabled = true;

        private List<Enemy> _activeEnemies = new List<Enemy>();

        private void Awake()
        {
            _activeEnemies.Clear();
            _enemySpawner.Initialize();
        }

        private IEnumerator Start()
        {
            while (_isSpawningEnabled)
            {
                yield return new WaitForSeconds(Random.Range(1, 2));

                if (_activeEnemies.Count <= _countOfEnemiesSameTime)
                {
                    Enemy enemy =_enemySpawner.SpawnEnemyAtRandomPoint();
                    _activeEnemies.Add(enemy);
                    enemy.OnDead += HandleEnemyDeadEvent;
                }
            }
        }

        private void HandleEnemyDeadEvent(Enemy enemy)
        {
            enemy.OnDead -= HandleEnemyDeadEvent;
            enemy.Dispose();
            _activeEnemies.Remove(enemy);
        }
    }
}