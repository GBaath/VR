using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    
    public UnityEvent onAlmostDead;
    private DamageIndicator damageVignetteImg;

    void Awake()
    {
        currentHealth = maxHealth;
        damageVignetteImg = FindObjectOfType<DamageIndicator>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);

        if (currentHealth <= 20)
        {
            damageVignetteImg.flashSpeed = 1f;
            damageVignetteImg.Flash();
            onAlmostDead?.Invoke();
        }

        if (currentHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // game over 
    }
}
