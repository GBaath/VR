using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBooster : MonoBehaviour
{
    public float force = 50;

    private void FixedUpdate()
    {
        var tmp = GameManager.instance.playerPhysicsBase.transform.GetChild(0).localEulerAngles;
        transform.localEulerAngles = new(tmp.x - 90, tmp.y + 180, tmp.z);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerPhysicsBase.GetComponent<WeaponPhysicsMove>().ApplyWeaponForce(GameManager.instance.playerPhysicsBase.transform.GetChild(0).forward*-1, force);
        }
    }
}
