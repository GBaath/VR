using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    
    public UnityEvent onAlmostDead;
    

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        //currentHealth = Mathf.Max(currentHealth - damage, 0);
        currentHealth -= damage;
        
        if (currentHealth <= 20)
        {
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
