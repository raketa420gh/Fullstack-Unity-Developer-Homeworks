using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public sealed class RigidbodyMoveComponent : IMoveComponent
    {
        public float MoveSpeed => _speed;

        [SerializeField]
        private Rigidbody2D _rigidbody2d;

        [SerializeField]
        private float _speed;

        public void Move(Vector3 direction)
        {
            if (direction == Vector3.zero)
                return;
            
            Vector2 newPosition = _rigidbody2d.position + (Vector2)direction * (_speed * Time.fixedDeltaTime);
            _rigidbody2d.MovePosition(newPosition);
        }
    }
}