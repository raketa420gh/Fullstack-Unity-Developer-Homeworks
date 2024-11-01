using System;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class Player : MonoBehaviour
    {
        public bool IsAlive => _isAlive;
        
        public event Action OnCreated;
        public event Action OnDead;

        [SerializeField] 
        private CharacterType _characterType = CharacterType.Player;
        [SerializeField] 
        private int _defaultMaxHealth = 1;
        [SerializeField] 
        private int _defaultDamage = 1;
        [SerializeField] 
        private float _defaultMoveSpeed = 3;
        [SerializeField] 
        private Rigidbody2D _rb;
        [SerializeField] 
        private Transform _firePoint; 
        [SerializeField]
        private BulletManager _bulletManager;

        private bool _isAlive;
        private IHealthComponent _healthComponent;
        private IMoveComponent _moveComponent;
        private IFireComponent _fireComponent;
        private IDealDamageComponent _dealDamageComponent;

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
            _dealDamageComponent = new DealDamageComponent(_characterType, _defaultDamage);
            _moveComponent = new MoveComponentRigidBody(_rb, _defaultMoveSpeed);
            _fireComponent = new FireComponent(_bulletManager, _firePoint, CharacterType.Player);

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