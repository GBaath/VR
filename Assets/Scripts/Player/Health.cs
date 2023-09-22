using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 200;
    [SerializeField] private float currentHealth;
    
    [FormerlySerializedAs("damageOverlayImg")] [SerializeField] private Image damageFlashImg;
    [FormerlySerializedAs("damageLoopOverlayImg")] [SerializeField] private Image damageLoopFlashingImg;
    [SerializeField] private float healthPercent;
    private bool isDead = false;
    private float damageCoolTime;
    private float maxDamageCoolTime = 5f;
    private bool isRestoringHealth = false;
    private float timeSinceLastDamage;
    void Awake()
    {
        currentHealth = maxHealth;
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
        currentHealth = Mathf.Min(currentHealth + 1, maxHealth);

        isRestoringHealth = true;
        UpdateHealthPercentage();
        timeSinceLastDamage = 0f;
        //DisableFlashLoop();
    }

    public void TakeDamage(float damage)
    {
        isRestoringHealth = false;
        
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        //damageFlashImg.GetComponent<Animator>().SetBool("DamageOverlay", true);
        damageCoolTime = 0f;
        UpdateHealthPercentage();

        if (healthPercent <= 0.2f && !isDead)
        {
            //damageLoopFlashingImg.GetComponent<Animator>().SetBool("DamageLoopOverlay", true);
            //damageFlashImg.GetComponent<Animator>().SetBool("DamageOverlay", false);
        }

        if (currentHealth == 0)
        {
            isDead = true;
            //DisableFlashLoop();
            Die();
        }
    }

    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateHealthPercentage()
    {
        healthPercent = currentHealth / maxHealth;
    }

    private void DisableFlashLoop()
    {
        damageLoopFlashingImg.GetComponent<Animator>().SetBool("DamageLoopOverlay", false);
    }
}
