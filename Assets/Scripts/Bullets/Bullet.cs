using System;
using System.Collections;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class Bullet : MonoBehaviour
    {
        public event Action<Bullet> OnDestroy;
  
        [SerializeField]
        private Rigidbody2D _rigidbody;
        [SerializeField] 
        private SpriteRenderer _spriteRenderer;
        [SerializeField]
        private float _speed;
        [SerializeField]
        private float _lifetime;
        
        private int _damage;
        private Coroutine _coroutine;

        private void OnEnable()
        {
            Activate();
        }

        private void OnDisable()
        {
            Deactivate();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.collider.TryGetComponent(out IHealthComponent healthComponent))
                return;
            
            if (healthComponent == null)
                return;
            
            healthComponent.TakeDamage(_damage);

            OnDestroy?.Invoke(this);
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

        public void SetDamage(int damage)
        {
            _damage = damage;
        }

        private void Activate()
        {
            gameObject.SetActive(true);
            _coroutine = StartCoroutine(this.LifetimeRoutine());
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
            OnDestroy?.Invoke(this);
        }
    }
}