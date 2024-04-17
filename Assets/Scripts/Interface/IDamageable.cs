using System;

public interface IDamageable
{
    int DamageCount { get; set; }
    Action OnDestroyed { get; set; }

    void TakeDamage(int amount);
}
