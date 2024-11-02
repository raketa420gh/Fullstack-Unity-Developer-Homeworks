using UnityEngine;

namespace ShootEmUp
{
    public interface IDealDamageComponent
    {
        public int Damage { get; }
        public CharacterType EnemyType { get; }
        public bool DealDamage(Collider collider);
    }
}