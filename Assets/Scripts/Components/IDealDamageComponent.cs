using UnityEngine;

namespace ShootEmUp
{
    public interface IDealDamageComponent
    {
        public bool DealDamage(Collider2D collider);
    }
}