using UnityEngine;

namespace ShootEmUp
{
    [CreateAssetMenu(fileName = "BulletData", menuName = "Bullets/BulletData")]
    public sealed class BulletData : ScriptableObject
    {
        public CharacterType EnemyType => _enemyType;
        public Color Color => _color;
        public int PhysicsLayerIndex => _physicsLayerIndex;
        public int Damage => _damage;

        [SerializeField] 
        private CharacterType _enemyType = CharacterType.Enemy;
        [SerializeField] 
        private Color _color;
        [SerializeField] 
        private int _physicsLayerIndex;
        [SerializeField] 
        private int _damage;
    }
}