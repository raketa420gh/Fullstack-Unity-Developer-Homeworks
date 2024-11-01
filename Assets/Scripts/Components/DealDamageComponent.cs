using UnityEngine;

namespace ShootEmUp
{
    public class DealDamageComponent : IDealDamageComponent
    {
        public int Damage => _damage;
        
        private readonly int _damage;
        private readonly CharacterType _enemyType;

        public DealDamageComponent(CharacterType enemyType, int damage)
        {
            _enemyType = enemyType;
            _damage = damage;
        }

        public bool DealDamage(Collider collider)
        {
            if (collider.TryGetComponent(out HealthComponent healthComponent))
            {
                healthComponent.TakeDamage(_damage);
                return true;
            }

            return false;
        }
    }
}