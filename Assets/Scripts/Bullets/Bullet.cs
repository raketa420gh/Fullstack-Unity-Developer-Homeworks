using System.Collections;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class Bullet : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D _rigidbody;
        [SerializeField] 
        private SpriteRenderer _spriteRenderer;
        [SerializeField]
        private float _speed;
        [SerializeField]
        private float _lifetime;
        
        private Coroutine _coroutine;
        private IDealDamageComponent _dealDamageComponent;

        private void OnEnable()
        {
            Activate();
        }

        private void OnDisable()
        {
            Deactivate();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            _dealDamageComponent.DealDamage(other.collider);
            Deactivate();
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetDirection(Vector3 direction)
        {
            _rigidbody.velocity = direction * _speed;
        }

        public void SetColor(Color color)
        {
            _spriteRenderer.color = color;
        }

        public void SetLayer(int layer)
        {
            gameObject.layer = layer;
        }

        public void SetDamageDealer(IDealDamageComponent dealDamageComponent)
        {
            _dealDamageComponent = dealDamageComponent;
        }

        private void Activate()
        {
            gameObject.SetActive(true);
            _coroutine = StartCoroutine(LifetimeRoutine());
        }

        private void Deactivate()
        {
            gameObject.SetActive(false);
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }

        private IEnumerator LifetimeRoutine()
        {
            yield return new WaitForSeconds(_lifetime);
            _coroutine = null;
            Deactivate();
        }
    }
}