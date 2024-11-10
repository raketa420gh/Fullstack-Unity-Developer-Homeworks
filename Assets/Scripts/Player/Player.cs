using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ShootEmUp
{
    public sealed class Player : MonoBehaviour, IDamageable
    {
        public bool IsAlive => _isAlive;
        public IHealthComponent Health => _healthComponent;
        public CharacterType EnemyType => _enemyType;
        public event Action OnCreated;
        public event Action OnDead;

        [SerializeField] 
        private CharacterType _characterType = CharacterType.Player;
        [SerializeField] 
        private CharacterType _enemyType = CharacterType.Enemy;
        [SerializeField] 
        private int _defaultMaxHealth = 1;
        [SerializeField] 
        private float _defaultMoveSpeed = 3;
        [SerializeField] 
        private Rigidbody2D _rb;
        [SerializeField] 
        private Transform _firePoint; 
        [SerializeField]
        private BulletSpawner _bulletSpawner;

        private bool _isAlive;
        private IHealthComponent _healthComponent;
        private IMoveComponent _moveComponent;
        private IFireComponent _fireComponent;

        private void Awake()
        {
            Create();
        }

        private void OnEnable()
        {
            _healthComponent.OnStateChanged += OnHealthStateChanged;
            _moveComponent.Enable();
        }

        private void OnDisable()
        {
            _healthComponent.OnStateChanged += OnHealthStateChanged;
            _moveComponent.Disable();
        }

        public void Create()
        {
            _healthComponent = new HealthComponent(_defaultMaxHealth);
            _moveComponent = new MoveComponentRigidBody(_rb, _defaultMoveSpeed);
            _fireComponent = new FireComponent(_bulletSpawner, _firePoint, _enemyType);

            _isAlive = true;
            
            OnCreated?.Invoke();
        }

        public void Move(Vector2 direction)
        {
            _moveComponent.Move(direction);
        }

        public void Fire(Vector2 direction)
        {
            _fireComponent.Fire(direction);
        }

        private void OnHealthStateChanged(int currentHealth)
        {
            if (currentHealth <= 0)
            {
                _isAlive = false;
                OnDead?.Invoke();
            }
        }
    }
}