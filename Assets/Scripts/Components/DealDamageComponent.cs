using UnityEngine;

namespace ShootEmUp
{
    public class DealDamageComponent : IDealDamageComponent
    {
        public int Damage => _damage;
        public CharacterType EnemyType => _enemyType;

        private readonly int _damage;
        private readonly CharacterType _enemyType;

        public DealDamageComponent(CharacterType enemyType, int damage)
        {
            _enemyType = enemyType;
            _damage = damage;
        }

        public bool DealDamage(Collider2D collider)
        {
            if (collider.TryGetComponent(out IDamageable damageable))
            {
                if (damageable == null)
                    return false;
                
                if (damageable.EnemyType == _enemyType)
                    return false;
                
                damageable.TakeDamage(_damage);
                return true;
            }

            return false;
        }
    }
}