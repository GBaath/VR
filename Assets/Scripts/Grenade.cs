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

    private void Start()
    {
        aS = GetComponent<AudioSource>();
    }
    public void ArmGrenade()
    {
        aS.clip = pinSound;
        aS.Play();
        isArmed = true;
        Debug.Log("Grenade is armed");
    }

    private void ExplodeGrenade()
    {
        Instantiate(explosionEffect, transform.position, explosionEffect.transform.rotation);
        Debug.Log("BOOM explosion");
        Destroy(gameObject, 2);
        aS.clip = explosion;
        aS.Play();
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
