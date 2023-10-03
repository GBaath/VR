using UnityEngine;
public interface IDamageable
{
    void TakeDamage(int amount, GameObject source = null);
    void Die(float destroyDelay);
}
