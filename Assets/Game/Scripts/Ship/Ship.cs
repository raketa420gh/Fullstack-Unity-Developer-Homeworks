using UnityEngine;

namespace Game
{
    public class Ship : MonoBehaviour
    {
        [SerializeField]
        private HealthComponent _healthComponent;

        [SerializeField]
        private FireComponent _fireComponent;

        [SerializeField]
        private RigidbodyMoveComponent _moveComponent;

        private void OnEnable()
        {
            _healthComponent.OnHealthZero += HandleHealthZeroEvent;
        }

        private void OnDisable()
        {
            _healthComponent.OnHealthZero -= HandleHealthZeroEvent;
        }

        public void Move(Vector3 direction)
        {
            _moveComponent?.Move(direction);
        }

        public void Fire()
        {
            _fireComponent?.Fire();
        }
        
        private void Disable()
        {
            gameObject.SetActive(false);
        }

        private void HandleHealthZeroEvent()
        {
            Disable();
        }
    }
}