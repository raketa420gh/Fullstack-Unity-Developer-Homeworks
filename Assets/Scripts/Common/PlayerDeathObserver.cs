using UnityEngine;

namespace ShootEmUp
{
    public class PlayerDeathObserver : MonoBehaviour
    {
        [SerializeField] 
        private Player _player;

        private void OnEnable()
        {
            _player.OnDead += HandlePlayerDeathEvent;
        }

        private void OnDisable()
        {
            _player.OnDead -= HandlePlayerDeathEvent;
        }

        private void HandlePlayerDeathEvent(Ship ship)
        {
            Time.timeScale = 0;
        }
    }
}