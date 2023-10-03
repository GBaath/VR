using UnityEngine;
public interface IDamageable
{
    void TakeDamage(int amount, bool isDead = false);
    void Die(float destroyDelay);
}
