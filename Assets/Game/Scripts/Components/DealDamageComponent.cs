using UnityEngine;

namespace Game
{
    public sealed class DealDamageComponent : MonoBehaviour
    {
        [SerializeField]
        private int _damage;
        
        public void DealDamage(Collision2D collision)
        {
            if (_damage == 0)
                return;
            
            HealthComponent healthComponent = collision.gameObject.GetComponent<HealthComponent>();
            
            if (healthComponent == null)
                return;
            
            healthComponent.ChangeHealth(-_damage);
        }
    }
}