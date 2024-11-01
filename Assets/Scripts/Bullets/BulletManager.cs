using System.Linq;
using New;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class BulletManager : MonoBehaviour
    {
        [SerializeField]
        private BulletSpawner _bulletSpawner;
        [SerializeField]
        private LevelBounds _levelBounds;
        [SerializeField] 
        private BulletData[] _bulletDatabase;

        private void Awake()
        {
            _bulletSpawner.Initialize(_levelBounds);
        }

        public void SpawnBullet(CharacterType characterType, Vector3 position, Vector2 direction)
        {
            BulletData bulletData = _bulletDatabase.FirstOrDefault(data => characterType == data.CharacterType);
            
            _bulletSpawner.SpawnBullet(bulletData, position, direction);
        }

        private void DealDamage(Bullet bullet, GameObject other)
        {
            /*int damage = bullet.damage;
            if (damage <= 0)
                return;
            
            if (other.TryGetComponent(out Player player))
            {
                if (bullet.isPlayer != player.isPlayer)
                {
                    if (player.health <= 0)
                        return;

                    player.health = Mathf.Max(0, player.health - damage);
                    player.OnHealthChanged?.Invoke(player, player.health);

                    if (player.health <= 0)
                        player.OnHealthEmpty?.Invoke(player);
                }
            }
            else if (other.TryGetComponent(out Enemy enemy))
            {
                if (bullet.isPlayer != enemy._isPlayer)
                {
                    if (enemy._health > 0)
                    {
                        enemy._health = Mathf.Max(0, enemy._health - damage);
                    }
                }
            }*/
        }
    }
}