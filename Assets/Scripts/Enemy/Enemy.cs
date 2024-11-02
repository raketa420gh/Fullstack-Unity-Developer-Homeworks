using System;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class Enemy : MonoBehaviour, IDamageable
    {
        public IHealthComponent Health => _healthComponent;
        public CharacterType EnemyType => _enemyType;
        public bool IsAlive => _isAlive;
        public event Action<Enemy> OnCreated;
        public event Action<Enemy> OnDead;

        [SerializeField] 
        private CharacterType _characterType = CharacterType.Enemy;
        [SerializeField] 
        private CharacterType _enemyType = CharacterType.Player;
        [SerializeField] 
        private int _defaultMaxHealth = 1;
        [SerializeField] 
        private float _defaultMoveSpeed = 3;
        [SerializeField]
        private Rigidbody2D _rigidbody;
        [SerializeField]
        private Transform _firePoint;
        [SerializeField]
        private float _countdown;
        [SerializeField] 
        private float _reachDistance = 0.25f;
        
        private BulletManager _bulletManager;
        private Player _target;
        private Vector2 _destination;
        private float _currentTime;
        private bool _isPointReached;
        private bool _isAlive;
        private IHealthComponent _healthComponent;
        private IMoveComponent _moveComponent;
        private IFireComponent _fireComponent;

        public void Create(BulletManager bulletManager)
        {
            _bulletManager = bulletManager;
            _healthComponent = new HealthComponent(_defaultMaxHealth);
            _moveComponent = new MoveComponentRigidBody(_rigidbody, _defaultMoveSpeed);
            _fireComponent = new FireComponent(_bulletManager, _firePoint, CharacterType.Player);
            
            _healthComponent.OnStateChanged += OnHealthStateChanged;
            _moveComponent.Enable();
            
            _isAlive = true;
            OnCreated?.Invoke(this);
        }

        public void Dispose()
        {
            _healthComponent.OnStateChanged -= OnHealthStateChanged;
            gameObject.SetActive(false);
        }

        public void Reset()
        {
            _currentTime = _countdown;
        }
        
        public void SetDestination(Vector2 endPoint)
        {
            _destination = endPoint;
            _isPointReached = false;
        }

        public void SetTarget(Player target)
        {
            _target = target;
        }

        public void Fire(Vector2 direction)
        {
            _fireComponent.Fire(direction);
        }

        private void FixedUpdate()
        {
            if (!_isAlive)
                return;
            
            if (_isPointReached)
            {
                _currentTime -= Time.fixedDeltaTime;
                
                if (_currentTime <= 0)
                {
                    Vector2 direction = GetTargetDirection();
                    Fire(direction);
                    
                    _currentTime += _countdown;
                }
            }
            else
            {
                Vector2 direction = _destination - (Vector2) transform.position;
                if (GetIsReached(direction)) 
                    return;
                
                _moveComponent.Move(direction);
            }
        }

        private Vector2 GetTargetDirection()
        {
            Vector2 startPosition = _firePoint.position;
            Vector2 vector = (Vector2)_target.transform.position - startPosition;
            Vector2 direction = vector.normalized;
            return direction;
        }

        private bool GetIsReached(Vector2 direction)
        {
            if (direction.magnitude <= _reachDistance)
            {
                _isPointReached = true;
                return true;
            }

            return false;
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