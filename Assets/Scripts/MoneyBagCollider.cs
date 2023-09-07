using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBagCollider : MonoBehaviour
{

    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private MeshRenderer meshToChange;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Treasure"))
        {
            meshToChange.material = highlightMaterial;
            
            Destroy(other);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Treasure"))
        {
            meshToChange.material = defaultMaterial;
        }
    }
}
