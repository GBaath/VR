using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerWeaponScript : MonoBehaviour
{

    [SerializeField] ParticleSystem flameFX;
    FlamethrowerRadius radiusScript;

    [SerializeField] int BurningDPS = 5;
    [SerializeField] float BurnDuration = 3f;
    private void Start()
    {
        radiusScript = GetComponent<FlamethrowerRadius>();
    }

}
