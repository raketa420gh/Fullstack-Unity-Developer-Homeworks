using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public sealed class FireComponent : MonoBehaviour
    {
        [SerializeField]
        private Transform _firePoint;
        
        [SerializeField]
        private float _fireCooldown;
        
        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private ParticleSystem _fireVFX;

        [SerializeField]
        private AudioClip _fireSFX;

        private bool _canFire;
        private float _fireTime;

        public void SetFireCondition(bool canFire)
        {
            _canFire = canFire;
        }

        public void Fire()
        {
            float time = Time.time;
            
            /*if (time - _fireTime < _fireCooldown || this.currentHealth <= 0)
                return;*/

            if (_fireSFX)
                _audioSource.PlayOneShot(_fireSFX);

            if (_fireVFX)
                _fireVFX.Play();

            //this.OnFire?.Invoke(this);
            _fireTime = time;
        }
    }
}