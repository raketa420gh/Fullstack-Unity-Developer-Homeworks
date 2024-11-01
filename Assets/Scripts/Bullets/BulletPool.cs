using UnityEngine;

namespace ShootEmUp
{
    public sealed class BulletPool : MonoBehaviour
    {
        [SerializeField] 
        private int _poolCount = 25;
        [SerializeField] 
        private bool _isAutoExpand = true;
        [SerializeField] 
        private Bullet _prefab;
        [SerializeField] 
        private Transform _container;

        private Pool<Bullet> _pool;

        public Pool<Bullet> Pool => _pool;

        public void CreatePool()
        {
            _pool = new Pool<Bullet>(_prefab, _poolCount, _container)
            {
                AutoExpand = _isAutoExpand
            };
        }
    }
}