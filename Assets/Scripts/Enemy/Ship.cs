using System;
using UnityEngine;

namespace ShootEmUp
{
    public class Ship : MonoBehaviour, IDamageable
    {
        public event Action<Ship> OnCreated;
        public event Action<Ship> OnDead;
        public CharacterType EnemyType => _enemyType;
        public bool IsAlive => _isAlive;
        public Vector2 FirePointPosition => _fireComponent.FirePoint.position;

        [SerializeField] 
        private CharacterType _characterType = CharacterType.Enemy;
        
        [SerializeField] 
        private CharacterType _enemyType = CharacterType.Player;
        
        [SerializeField] 
        private float _defaultMoveSpeed = 3;
        
        [SerializeField] 
        private Rigidbody2D _rigidbody;

        [SerializeField]
        private HealthComponent _healthComponent;

        [SerializeField]
        private FireComponent _fireComponent;

        private IMoveComponent _moveComponent;
        private BulletSpawner _bulletSpawner;
        private bool _isAlive;

        public void Create(BulletSpawner bulletSpawner)
        {
            _bulletSpawner = bulletSpawner;
            _fireComponent.Set(_bulletSpawner);
            _moveComponent = new MoveComponentRigidBody(_rigidbody, _defaultMoveSpeed);
            
            _healthComponent.OnStateChanged += OnHealthStateChanged;
            _moveComponent.Enable();

            _isAlive = true;
            OnCreated?.Invoke(this);
        }

        public void Dispose()
        {
            _healthComponent.OnStateChanged -= OnHealthStateChanged;
            _moveComponent.Disable();
            gameObject.SetActive(false);
        }

        public void Fire(Vector2 direction)
        {
            _fireComponent.Fire(direction);
        }

        public void TakeDamage(int damage)
        {
            _healthComponent.TakeDamage(damage);
        }

        public void Move(Vector2 direction)
        {
            _moveComponent.Move(direction);
        }

        private void OnHealthStateChanged(int currentHealth)
        {
            if (currentHealth <= 0)
            {
                _isAlive = false;
                OnDead?.Invoke(this);
            }
        }
    }
}