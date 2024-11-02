using UnityEngine;

namespace ShootEmUp
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Player _player;
        [SerializeField]
        private InputService _inputService;

        private bool _fireRequired;
        private float _moveDirectionX;

        private void OnEnable()
        {
            _player.OnDead += HandlePlayerDeadEvent;
            _inputService.OnMoveLeftKeyPressed += HandleInputMoveLeftKeyEvent;
            _inputService.OnMoveRightKeyPressed += HandleInputMoveRightKeyEvent;
            _inputService.OnFireKeyPressed += HandleInputFireKeyEvent;
        }

        private void OnDisable()
        {
            _player.OnDead -= HandlePlayerDeadEvent;
            _inputService.OnMoveLeftKeyPressed -= HandleInputMoveLeftKeyEvent;
            _inputService.OnMoveRightKeyPressed -= HandleInputMoveRightKeyEvent;
            _inputService.OnFireKeyPressed -= HandleInputFireKeyEvent;
        }

        private void FixedUpdate()
        {
            _player.Move(new Vector2(_moveDirectionX, 0));
        }

        private void HandlePlayerDeadEvent()
        {
            Time.timeScale = 0;
        }

        private void HandleInputMoveLeftKeyEvent()
        {
            _player.Move(Vector2.left);
        }

        private void HandleInputMoveRightKeyEvent()
        {
            _player.Move(Vector2.right);
        }

        private void HandleInputFireKeyEvent()
        {
            _player.Fire(Vector2.up);
        }
    }
}