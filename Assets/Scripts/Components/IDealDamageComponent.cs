using UnityEngine;

namespace ShootEmUp
{
    public interface IDealDamageComponent
    {
        void Set(CharacterType enemyType, int damage);
        bool DealDamage(Collider2D collider);
    }
}