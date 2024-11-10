using UnityEngine;

namespace ShootEmUp
{
    public interface IFireComponent
    {
        public Transform FirePoint { get; }
        public void Fire(Vector2 direction);
    }
}