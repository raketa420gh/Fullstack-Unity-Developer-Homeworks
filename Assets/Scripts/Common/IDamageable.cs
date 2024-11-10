namespace ShootEmUp
{
    public interface IDamageable
    {
        void TakeDamage(int damage);
        CharacterType EnemyType { get; }
    }
}