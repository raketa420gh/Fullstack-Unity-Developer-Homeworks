using System;
using UnityEngine;
namespace ShootEmUp
{
    [Serializable]
    public sealed class HealthComponent : IHealthComponent
    {
        public event Action<int> OnStateChanged;

        [SerializeField] 
        private int _health;

        [SerializeField] 
        private int _maxHealth;

        public HealthComponent(int maxHealth)
        {
            _maxHealth = maxHealth;
            _health = _maxHealth;
        }

        public float GetProgress()
        {
            return (float)_health / _maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (_health == 0)
                return;

            _health = Math.Max(0, _health - damage);
            OnStateChanged?.Invoke(_health);
        }
    }
}