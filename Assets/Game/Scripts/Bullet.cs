using UnityEngine;

namespace Game
{
    public sealed class Bullet : MonoBehaviour
    {
        private TeamType _team;
        private Vector2 _direction;
        private int _damage;
        private int _speed;

        private void OnEnable()
        {
            gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            gameObject.SetActive(false);
        }
    }
}