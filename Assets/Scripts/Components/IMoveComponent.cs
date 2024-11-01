using UnityEngine;

namespace ShootEmUp
{
    public interface IMoveComponent
    {
        public float Speed { get; }
        public bool Enabled { get; }

        public void Enable();
        public void Disable();
        public void Move(Vector2 direction);
    }
}