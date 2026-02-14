using System;
using UnityEngine;

public class InputService : MonoBehaviour
{
    public event Action OnMoveLeftKeyPressed;
    public event Action OnMoveRightKeyPressed;
    public event Action OnFireKeyPressed;
    
    [SerializeField]
    private KeyCode _leftKey = KeyCode.LeftArrow;
    
    [SerializeField]
    private KeyCode _rightKey = KeyCode.RightArrow;
    
    [SerializeField]
    private KeyCode _fireKey = KeyCode.Space;

    private void Update()
    {
        if (Input.GetKeyDown(_leftKey))
        {
            OnMoveLeftKeyPressed?.Invoke();
        }

        if (Input.GetKeyDown(_rightKey))
        {
            OnMoveRightKeyPressed?.Invoke();
        }

        if (Input.GetKeyDown(_fireKey))
        {
            OnFireKeyPressed?.Invoke();
        }
    }
}
