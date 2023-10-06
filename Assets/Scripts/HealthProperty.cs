using UnityEngine;

public class HealthProperty : MonoBehaviour {
    public float maxHealth = 1, deathTimer = 3;
    float healthLost;
    float currentHealth;
    public float CurrentHealth {
        get { return maxHealth - healthLost; }
        set { currentHealth = value; }
    }
    [HideInInspector] public bool isDead = false;

    private void Start() {
        if (TryGetComponent(out Enemy enemy) && enemy.EnemyData) {
            maxHealth = enemy.EnemyData.MaxHealth;
        }
        currentHealth = maxHealth;
    }

    public void LoseHealth(float amount, GameObject source = null) {
        currentHealth -= amount;
        healthLost += amount;
        bool isLastBlow = currentHealth <= 0;
        if (!TryGetComponent(out IDamageable damageable)) {
            return;
        }
        if (amount > 0 && !isDead) {
            damageable.TakeDamage(amount, isLastBlow);
        }
        if (isLastBlow) { isDead = true; }
        if (isDead) {
            if (damageable != null) {
                damageable.Die(deathTimer);
            } else {
                Destroy(gameObject, deathTimer);
            }
        }
    }
}