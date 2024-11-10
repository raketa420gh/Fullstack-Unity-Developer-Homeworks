using UnityEngine;

namespace ShootEmUp
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Player _player;
        [SerializeField]
        private InputAdapter _inputAdapter;

        private bool _fireRequired;
        private float _moveDirectionX;

        private void OnEnable()
        {
            _inputAdapter.OnMoveLeftKeyPressed += HandleInputMoveLeftKeyEvent;
            _inputAdapter.OnMoveRightKeyPressed += HandleInputMoveRightKeyEvent;
            _inputAdapter.OnFireKeyPressed += HandleInputFireKeyEvent;
        }

        private void OnDisable()
        {
            _inputAdapter.OnMoveLeftKeyPressed -= HandleInputMoveLeftKeyEvent;
            _inputAdapter.OnMoveRightKeyPressed -= HandleInputMoveRightKeyEvent;
            _inputAdapter.OnFireKeyPressed -= HandleInputFireKeyEvent;
        }

        private void FixedUpdate()
        {
            _player.Move(new Vector2(_moveDirectionX, 0));
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