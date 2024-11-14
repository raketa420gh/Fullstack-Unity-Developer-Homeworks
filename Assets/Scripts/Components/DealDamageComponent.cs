using UnityEngine;

namespace ShootEmUp
{
    public class DealDamageComponent : IDealDamageComponent
    {
        private CharacterType _enemyType;
        private int _damage;

        public DealDamageComponent(CharacterType enemyType, int damage)
        {
            _enemyType = enemyType;
            _damage = damage;
        }

        public void Set(CharacterType enemyType, int damage)
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