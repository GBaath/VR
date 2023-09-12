using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    private Health player;

    private void Awake()
    {
        player = FindObjectOfType<Health>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided");
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collided player");
            player.GetComponent<Health>().TakeDamage(20);
        }
    }
}
