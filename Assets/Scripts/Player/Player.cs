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
    }
}