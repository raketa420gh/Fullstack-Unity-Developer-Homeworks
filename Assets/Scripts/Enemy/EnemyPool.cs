using UnityEngine;

namespace ShootEmUp
{
    public sealed class EnemyPool : Pool<EnemyBehaviour>
    {
        [SerializeField] 
        private BulletSpawner _bulletSpawner;
        
        public override EnemyBehaviour GetFromPool()
        {
            EnemyBehaviour enemyBehaviour = base.GetFromPool();
            enemyBehaviour.Create(_bulletSpawner);
            return enemyBehaviour;
        }
    }
}