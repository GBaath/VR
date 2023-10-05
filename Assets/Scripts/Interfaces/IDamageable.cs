using UnityEngine;
public interface IDamageable
{
    void TakeDamage(float amount, bool isDead = false);
    void Die(float destroyDelay);
}
