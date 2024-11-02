using UnityEngine;

namespace ShootEmUp
{
    public sealed class EnemyPool : MonoBehaviour
    {
        public Pool<Enemy> Pool => _pool;
        
        [SerializeField] 
        private int _poolCount = 25;
        [SerializeField] 
        private bool _isAutoExpand = true;
        [SerializeField] 
        private Enemy _prefab;
        [SerializeField] 
        private Transform _container;
        
        private Pool<Enemy> _pool;

        public void CreatePool()
        {
            _pool = new Pool<Enemy>(_prefab, _poolCount, _container)
            {
                AutoExpand = _isAutoExpand
            };
        }
    }
}