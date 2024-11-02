namespace ShootEmUp
{
    public interface IDamageable
    {
        IHealthComponent Health { get; }
        CharacterType EnemyType { get; }
    }
}