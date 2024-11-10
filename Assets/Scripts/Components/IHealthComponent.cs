using System;

namespace ShootEmUp
{
    public interface IHealthComponent
    {
        public event Action<int> OnStateChanged;
        public void TakeDamage(int damage);
    }
}