using UnityEngine;

public class HealthProperty : MonoBehaviour {
    public int maxHealth = 1, deathTimer = 3;
    int currentHealth;

    private void Start() {
        if (TryGetComponent(out Enemy enemy)) {
            if (!enemy.wasSpawned) {
                maxHealth = enemy.EnemyData.MaxHealth;
            }
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