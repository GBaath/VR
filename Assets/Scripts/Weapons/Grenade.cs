using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Grenade : MonoBehaviour
{

    bool isArmed = false;
    public float fuseTime;
    AudioSource aS;
    [SerializeField] UnityEngine.AudioClip explosion;
    [SerializeField] UnityEngine.AudioClip pinSound;
    [SerializeField] ParticleSystem explosionEffect;
    [SerializeField] Collider pinGrab;


    //Damage
    AoeDmg dmgScript;
    public float dmgRadius;
    public int damageAmount;

    private void Start()
    {
        dmgScript = GetComponent<AoeDmg>();
        aS = GetComponent<AudioSource>();
    }
    public void ArmGrenade()
    {
        aS.clip = pinSound;
        aS.Play();
        isArmed = true;
    }

    private void ExplodeGrenade()
    {
        Instantiate(explosionEffect, transform.position, explosionEffect.transform.rotation);
        Destroy(gameObject, 2);
        aS.clip = explosion;
        aS.Play();
        dmgScript.DealAoEDamage(transform.position, damageAmount, dmgRadius);
    }
    public void StartFuse()
    {
        if (isArmed)
        {
            Invoke(nameof(ExplodeGrenade), fuseTime);
        }
    }
    public void ActivatePin()
    {
        pinGrab.enabled = true;
    }
    public void DeActivatePin()
    {
        pinGrab.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            ExplodeGrenade();
        }
    }




}
