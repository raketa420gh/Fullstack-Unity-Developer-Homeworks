using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public sealed class HealthComponent
    {
        public event Action<float> OnHealthChanged;
        
        [SerializeField]
        private int _maxHealth;
        
        private int _currentHealth;

        public void Initialize()
        {
            if (_maxHealth > 0)
                _currentHealth = _maxHealth;
            else
                Debug.LogWarning("Max health can be more than 0.");
        }

        public void Dispose()
        {
            
        }

        public void ChangeHealth(int amount)
        {
            int targetHealth = _currentHealth + amount;
            
            if (targetHealth > _maxHealth)
                targetHealth = _maxHealth;
            if (targetHealth < 0)
                targetHealth = 0;
            
            _currentHealth = targetHealth;
            
            OnHealthChanged?.Invoke(_currentHealth);
        }
    }
}