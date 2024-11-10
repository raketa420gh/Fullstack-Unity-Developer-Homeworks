using UnityEngine;

namespace ShootEmUp
{
    public sealed class Player : Ship
    {
        [SerializeField] 
        private BulletSpawner _bulletSpawner;
        
        private void Awake()
        {
            Create(_bulletSpawner);
        }

        public void Move(Vector2 direction)
        {
            _moveComponent.Move(direction);
        }
    }
}