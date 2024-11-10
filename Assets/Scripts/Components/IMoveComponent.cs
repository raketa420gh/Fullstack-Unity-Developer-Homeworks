using UnityEngine;

namespace ShootEmUp
{
    public interface IMoveComponent
    {
        public void Enable();
        public void Disable();
        public void Move(Vector2 direction);
    }
}