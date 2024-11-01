using UnityEngine;

namespace ShootEmUp
{
    public interface IDealDamageComponent
    {
        public int Damage { get; }
        public bool DealDamage(Collider collider);
    }
}