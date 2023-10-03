using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 200;
    [SerializeField] private float currentHealth;
    [SerializeField] private float healthPercent;
    [SerializeField] DisableCanvas highScoreCanvas;


    [FormerlySerializedAs("damageOverlayImg")] [SerializeField] private Animator damageFlashImg;
    [FormerlySerializedAs("damageLoopOverlayImg")] [SerializeField] private Animator damageLoopFlashingImg;
    
    public event Action onDeath;
    public event Action onNearDeath ;
    public event Action onResetHealth;
    public event Action onhitDamage;

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
        DisableFlashLoop();
        //onResetHealth();
    }

    private void UpdateHealthPercentage()
    {
        healthPercent = currentHealth / maxHealth;
    }

    private void DisableFlashLoop()
    {
        damageLoopFlashingImg.SetBool("DamageLoopOverlay", false);
    }

    public void TakeDamage(int amount, bool isDead = false)
    {
        //onhitDamage();
        isRestoringHealth = false;
        
        currentHealth = Mathf.Max(currentHealth - amount, 0);
        damageFlashImg.SetTrigger("DamageFlash");
        damageCoolTime = 0f;
        UpdateHealthPercentage();

        if (healthPercent <= 0.2f && !isDead)
        {
            damageLoopFlashingImg.SetBool("DamageLoopOverlay", true);
            //onNearDeath();
        }

        if (currentHealth == 0)
        {
            isDead = true;
            DisableFlashLoop();
            Die(0.2f);
            //onResetHealth();
        }
    }

    public void Die(float destroyDelay)
    {
        //onDeath();
        ScoreSaver.Instance.OnDeath();
        Invoke(nameof(ResetScene), destroyDelay);
    }

    private void ResetScene()
    {
        Debug.Log("WTF???");
        highScoreCanvas.Enable();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
