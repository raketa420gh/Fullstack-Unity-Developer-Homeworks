using UnityEngine;

namespace ShootEmUp
{
    public sealed class LevelBackgroundMover : MonoBehaviour
    {
        [SerializeField] 
        private LevelBackgroundParams _levelBackgroundParams;

        private float _startPositionY;
        private float _endPositionY;
        private float _movingSpeedY;
        private float _positionX;
        private float _positionZ;
        private Transform _myTransform;

        private void Awake()
        {
            _startPositionY = _levelBackgroundParams.StartPositionY;
            _endPositionY = _levelBackgroundParams.EndPositionY;
            _movingSpeedY = _levelBackgroundParams.MovingSpeedY;
            _myTransform = transform;
            Vector3 position = _myTransform.position;
            _positionX = position.x;
            _positionZ = position.z;
        }

        private void FixedUpdate()
        {
            if (_myTransform.position.y <= _endPositionY)
                _myTransform.position = new Vector3(_positionX, _startPositionY, _positionZ);

            _myTransform.position -= new Vector3(_positionX, _movingSpeedY * Time.fixedDeltaTime, _positionZ);
        }
    }
}