using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPhysicsMove : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    public void ApplyWeaponForce(Vector3 weaponAimVector, float forceFactor)
    {
        rb.AddForce(weaponAimVector.normalized*-1*forceFactor*100);
    }
}
