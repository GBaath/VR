using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    private Health health;

    private void Awake()
    {
        health = FindObjectOfType<Health>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Collided");
            health.TakeDamage(20);
        }
    }
}
