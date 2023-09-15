using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthProperty : MonoBehaviour
{
    public int maxHealth = 1, deathTimer = 3;
    int currentHealth;

    private void Start()
    {
        if (TryGetComponent(out Enemy enemy))
        {
            maxHealth = enemy.enemyData.maxHealth;
        }
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (TryGetComponent(out Enemy enemy))
        {
            enemy.GetComponent<AudioSource>().clip = GameManager.instance.audioManager.enemyHit;
            enemy.GetComponent<AudioSource>().Play();
            //Debug.Log(enemy.GetComponent<AudioSource>().clip);
            enemy.GetComponent<AudioSource>().clip = null;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (TryGetComponent(out Enemy enemy))
        {
            enemy.RagdollSetActive(true);
            enemy.KillThisEnemy();
        }
        else if (TryGetComponent(out ExplosiveBarrel eb))
        {
            eb.Explode();
        }
        else
        {
            Destroy(gameObject, deathTimer);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out ProjectileDamage pd))
        {
            TakeDamage(pd.damage);
        }
    }
}