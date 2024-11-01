using System;

namespace ShootEmUp
{
    public interface IHealthComponent
    {
        public event Action<int> OnStateChanged;
        public int Health { get; }
        public int MaxHealth { get; }
        public float GetProgress();
        public void TakeDamage(int damage);
    }
}