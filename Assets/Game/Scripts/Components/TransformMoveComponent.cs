using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public sealed class TransformMoveComponent : IMoveComponent
    {
        public float MoveSpeed => _speed;

        [SerializeField]
        private Transform _transform;

        [SerializeField]
        private float _speed;

        public void Move(Vector3 direction)
        {
            Vector3 moveStep = _transform.forward * (_speed * Time.fixedDeltaTime);
            _transform.position += moveStep;
        }
    }
}