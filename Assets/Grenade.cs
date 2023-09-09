using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    bool isArmed = false;
    public float fuseTime;
    AudioSource aS;
    [SerializeField] UnityEngine.AudioClip explosion;
    [SerializeField] UnityEngine.AudioClip pinSound;
    [SerializeField] ParticleSystem explosionEffect;

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
}
