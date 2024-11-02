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
        private BulletManager _bulletManager;
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
                
                Enemy enemy =_enemySpawner.SpawnEnemyAtRandomPoint();
                _activeEnemies.Add(enemy);
                enemy.OnDead += HandleEnemyDeadEvent;

                /*if (_activeEnemies.Count < 5)
                {
                    enemy.OnFire += OnFire;
                }*/
            }
        }

        private void OnFire(Vector2 position, Vector2 direction)
        {
            //_bulletManager.SpawnBullet(position, Color.red, (int) PhysicsLayer.ENEMY_BULLET, 1, false, direction * 2);
        }

        private void HandleEnemyDeadEvent(Enemy enemy)
        {
            enemy.OnDead -= HandleEnemyDeadEvent;
            enemy.Dispose();
            _activeEnemies.Remove(enemy);
        }
    }
}