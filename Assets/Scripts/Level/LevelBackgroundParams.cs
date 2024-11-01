using System;
using UnityEngine;

namespace ShootEmUp
{
    [Serializable]
    public sealed class LevelBackgroundParams
    {
        public float StartPositionY => _startPositionY;
        public float EndPositionY => _endPositionY;
        public float MovingSpeedY => _movingSpeedY;
        
        [SerializeField] 
        private float _startPositionY;
        [SerializeField] 
        private float _endPositionY;
        [SerializeField] 
        private float _movingSpeedY;
    }
}