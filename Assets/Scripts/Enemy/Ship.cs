using System;
using UnityEngine;

namespace ShootEmUp
{
    public class Ship : MonoBehaviour, IDamageable
    {
        public CharacterType EnemyType => _enemyType;
        public bool IsAlive => _isAlive;
        public event Action<Ship> OnCreated;
        public event Action<Ship> OnDead;

        [SerializeField] private CharacterType _characterType = CharacterType.Enemy;
        [SerializeField] private CharacterType _enemyType = CharacterType.Player;
        [SerializeField] private int _defaultMaxHealth = 1;
        [SerializeField] private float _defaultMoveSpeed = 3;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Transform _firePoint;

        private BulletSpawner _bulletSpawner;
        private IHealthComponent _healthComponent;
        protected IMoveComponent _moveComponent;
        protected IFireComponent _fireComponent;
        protected bool _isAlive;

        public void Create(BulletSpawner bulletSpawner)
        {
            _bulletSpawner = bulletSpawner;
            _healthComponent = new HealthComponent(_defaultMaxHealth);
            _moveComponent = new MoveComponentRigidBody(_rigidbody, _defaultMoveSpeed);
            _fireComponent = new FireComponent(_bulletSpawner, _firePoint, _enemyType);

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