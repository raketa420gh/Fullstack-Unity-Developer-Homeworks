using UnityEngine;

namespace Game
{
    public sealed class Bullet : MonoBehaviour
    {
        [SerializeField]
        private TransformMoveComponent _moveComponent;
        
        [SerializeField]
        private TeamComponent _teamComponent;
        
        [SerializeField]
        private DealDamageComponent _dealDamageComponent;
        
        private Vector3 _direction;

        private void OnEnable()
        {
            Enable();
        }

        private void Update()
        {
            if (_moveComponent == null)
                return;
            
            _moveComponent.Move(_direction);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            _dealDamageComponent.DealDamage(other);
            Disable();
        }

        private void OnDisable()
        {
            Disable();
        }

        public void SetDirection(Vector3 direction)
        {
            _direction = direction;
        }

        private void Enable()
        {
            gameObject.SetActive(true);
        }

        private void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}