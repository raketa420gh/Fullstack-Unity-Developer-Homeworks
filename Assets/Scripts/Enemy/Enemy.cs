using UnityEngine;

namespace ShootEmUp
{
    public sealed class Enemy : Ship
    {
        [SerializeField] private float _countdown;
        [SerializeField] private float _reachDistance = 0.25f;
        
        private Player _target;
        private Vector2 _destination;
        private float _currentTime;
        private bool _isPointReached;

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
                Vector2 direction = _destination - (Vector2)transform.position;
                if (GetIsReached(direction))
                    return;

                _moveComponent.Move(direction);
            }
        }

        private Vector2 GetTargetDirection()
        {
            Vector2 startPosition = _fireComponent.FirePoint.position;
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
    }
}