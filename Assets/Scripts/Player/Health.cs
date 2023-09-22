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
    [FormerlySerializedAs("damageOverlayImg")] [SerializeField] private Animator damageFlashImg;
    [FormerlySerializedAs("damageLoopOverlayImg")] [SerializeField] private Animator damageLoopFlashingImg;
    private float healthPercent;
    private bool isDead = false;
    private float damageCoolTime;
    private float maxDamageCoolTime = 5f;
    private bool isRestoringHealth = false;
    private float timeSinceLastDamage;
    void Awake()
    {
        currentHealth = maxHealth;
         damageFlashImg = GetComponent<Animator>();
         damageLoopFlashingImg = GetComponent<Animator>();

    }
    
    private void Update()
    {

        if (!isRestoringHealth)
        {
            damageCoolTime += Time.deltaTime;
        }

        if (damageCoolTime >= maxDamageCoolTime && !isDead)
        {
            AddToHealth();
        }
    }
    
    public void AddToHealth()
    {
        //currentHealth += 1f * maxCoolTime;
        currentHealth = Mathf.Min(currentHealth + 1, maxHealth);

        isRestoringHealth = true;

        timeSinceLastDamage = 0f;
        DisableFlashLoop();
    }

    public void TakeDamage(float damage)
    {
        isRestoringHealth = false;
        
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        damageFlashImg.SetBool("DamageOverlay", true);
        damageCoolTime = 0f;
        healthPercent = currentHealth / maxHealth;

        if (currentHealth > 0 && healthPercent <= 0.2f && !isDead)
        {
            damageLoopFlashingImg.SetBool("DamageLoopOverlay", true);
            damageFlashImg.SetBool("DamageOverlay", false);
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
        damageLoopFlashingImg.SetBool("DamageLoopOverlay", false);
    }
}
