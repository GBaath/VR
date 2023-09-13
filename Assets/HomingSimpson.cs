using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingSimpson : MonoBehaviour
{
    [SerializeField] bool trigger;
    [SerializeReference] GameObject homingBullet;

    private void Update()
    {
        if (trigger)
        {
            trigger = false;
            Instantiate(homingBullet, transform.position, Quaternion.identity);
        }
    }
}
