using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInstructionScript : MonoBehaviour
{

    [SerializeField] GameObject crossbow;
    [SerializeField] Animator handanimtor;

    public void TurnOffCrossbow()
    {
        crossbow.SetActive(false);
    }
    public void TurnOnCrossbow()
    {
        crossbow.SetActive(true);
    }

    public void SetGrip()
    {        
        handanimtor.SetBool("Grip", true);

        Debug.Log("GripShouldHappen");
    }
    public void RemoveGrip()
    {
        handanimtor.SetBool("Grip", false);
        Debug.Log("NoGrip");
    }
}