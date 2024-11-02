using System;

namespace ShootEmUp
{
    public sealed class HealthComponent : IHealthComponent
    {
        public int Health { get; private set; }
        public int MaxHealth { get; private set; }
        public event Action<int> OnStateChanged;

        public HealthComponent(int maxHealth)
        {
            MaxHealth = maxHealth;
            Health = MaxHealth;
        }

        public float GetProgress()
        {
            return (float)Health / MaxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (Health == 0)
                return;

            Health = Math.Max(0, Health - damage);
            OnStateChanged?.Invoke(Health);
        }
    }
}