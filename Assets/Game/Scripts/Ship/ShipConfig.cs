using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Game/ShipConfig", order = 0)]
    public sealed class ShipConfig : ScriptableObject
    {
        public int MaxHealth => _maxHealth;
        public int MoveSpeed => _moveSpeed;
        public float FireCooldown => _fireCooldown;
        
        [SerializeField]
        private int _maxHealth = 5;
        
        [SerializeField]
        private int _moveSpeed = 5;
        
        [SerializeField]
        private float _fireCooldown = 0.25f;
    }
}