using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 200;
    [SerializeField] private float currentHealth;
    
    public UnityEvent onAlmostDead;
    [FormerlySerializedAs("damageOverlayImg")] [SerializeField] private Image damageFlashImg;
    [FormerlySerializedAs("damageLoopOverlayImg")] [SerializeField] private Image damageLoopFlashingImg;
    private float healthPercent;
    private bool isDead = false;
    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        damageFlashImg.GetComponent<Animator>().SetTrigger("DamageFlash");

        healthPercent = currentHealth / maxHealth;

        if (currentHealth > 0 && healthPercent <= 0.2f && !isDead)
        {
            damageLoopFlashingImg.GetComponent<Animator>().SetTrigger("DamageLoopFlash");
            //onAlmostDead?.Invoke();
        }

        if (currentHealth == 0)
        {
            isDead = true;
            DisableFlashLoop();
            Die();
        }
    }

    private void Die()
    {
        // game over 
    }

    private void DisableFlashLoop()
    {
        damageLoopFlashingImg.GetComponent<Animator>().enabled = false;
    }
}
