using UnityEngine;

public class HealthProperty : MonoBehaviour {
    public float maxHealth = 1, deathTimer = 3;
    float currentHealth;
    [HideInInspector] public bool isDead = false;

    private void Start() {
        if (TryGetComponent(out Enemy enemy) && enemy.EnemyData) {
            maxHealth = enemy.EnemyData.MaxHealth;
        }
        currentHealth = maxHealth;
    }

    public void LoseHealth(int amount, GameObject source = null) {
        currentHealth -= amount;
        TryGetComponent(out IDamageable damageable);
        if (currentHealth <= 0) {
            if (damageable != null) {
                damageable.Die(deathTimer);
            } else {
                Destroy(gameObject, deathTimer);
            }
            return;
        }
        if (amount > 0) {
            damageable.TakeDamage(amount, source);
        }
    }
}