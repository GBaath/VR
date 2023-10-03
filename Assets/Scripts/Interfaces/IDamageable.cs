using UnityEngine;
public interface IDamageable
{
    //bool IsDead { get; set; }
    void TakeDamage(int amount, GameObject source = null);
    void Die(float destroyDelay);
}
