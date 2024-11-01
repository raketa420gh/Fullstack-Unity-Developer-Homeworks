using UnityEngine;

namespace ShootEmUp
{
    public sealed class MoveComponentRigidBody : IMoveComponent
    {
        public float Speed => _speed;
        public bool Enabled => _enabled;

        private readonly Rigidbody2D _rigidbody;
        private readonly float _speed;
        private bool _enabled;

        public MoveComponentRigidBody(Rigidbody2D rigidbody, float speed)
        {
            _rigidbody = rigidbody;
            _speed = speed;
        }
        
        public void Enable()
        {
            _enabled = true;
        }

        public void Disable()
        {
            _enabled = false;
        }
        
        public void Move(Vector2 direction)
        {
            if (!_enabled)
                return;
            
            Vector2 moveStep = direction * (_speed * Time.fixedDeltaTime);
            Vector2 targetPosition = _rigidbody.position + moveStep;
            _rigidbody.position = targetPosition;
        }
    }
}