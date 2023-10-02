using UnityEngine;

public class HealthProperty : MonoBehaviour {
    public float maxHealth = 1, deathTimer = 3;
    float currentHealth;

    private void Start() {
        if (TryGetComponent(out Enemy enemy) && enemy.EnemyData) {
            maxHealth = enemy.EnemyData.MaxHealth;
        }
        currentHealth = maxHealth;
    }

    public void LoseHealth(int amount) {
        currentHealth -= amount;
        if (TryGetComponent(out IDamageable damageable)) { damageable.TakeDamage(amount); }
        if (currentHealth <= 0) {
            if (damageable != null) { damageable.Die(deathTimer); } else { Destroy(gameObject, deathTimer); }
        }
    }
}