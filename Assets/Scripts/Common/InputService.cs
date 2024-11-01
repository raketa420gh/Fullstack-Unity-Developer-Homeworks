﻿using System;
using UnityEngine;

namespace ShootEmUp
{
    public class InputService : MonoBehaviour
    {
        public event Action OnMoveLeftKeyPressed;
        public event Action OnMoveRightKeyPressed;
        public event Action OnFireKeyPressed;

        [SerializeField] 
        private KeyCode _moveLeftKey = KeyCode.LeftArrow;
        [SerializeField] 
        private KeyCode _moveRightKey = KeyCode.RightArrow;
        [SerializeField] 
        private KeyCode _fireKey = KeyCode.Space;
        
        private void Update()
        {
            if (Input.GetKeyDown(_fireKey))
            {
                Debug.Log($"{_fireKey}");
                OnFireKeyPressed?.Invoke();
            }

            if (Input.GetKey(_moveLeftKey))
            {
                Debug.Log($"{_moveLeftKey}");
                OnMoveLeftKeyPressed?.Invoke();
            }
            else if (Input.GetKey(_moveRightKey))
            {
                Debug.Log($"{_moveRightKey}");
                OnMoveRightKeyPressed?.Invoke();
            }
        }
    }
}