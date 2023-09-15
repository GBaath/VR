using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeDmg : MonoBehaviour
{
    public LayerMask targetLayer;
    public void DealAoEDamage(Vector3 center,int damageAmount,float damageRadius)
    {      
        Collider[] hitColliders = Physics.OverlapSphere(center, damageRadius,targetLayer);
        foreach (var hitCollider in hitColliders)
        {
           
            if (hitCollider.TryGetComponent(out HealthProperty health))
            {
                Debug.Log("DamageDealt");
                health.TakeDamage(damageAmount);                
            }
        }
    }
}
