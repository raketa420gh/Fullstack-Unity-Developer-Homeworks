using UnityEngine;

namespace Game
{
    public interface IMoveComponent
    {
        float MoveSpeed { get; }
        void Move(Vector3 direction);
    }
}