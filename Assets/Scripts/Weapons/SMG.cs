using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SMG : MonoBehaviour
{
    public ShootingWeaponSO weaponStats;
    public HapticInteractable hapticScript;
    public Transform firingPoint;
    public int currentAmmo;
    private AudioSource aS;
    bool isFiring;
    private float lastFiredTime = 0f; // time the last bullet was fired
    private bool outOfAmmo = false;
    private XRGrabInteractable grabbable;


    private void Start()
    {
        hapticScript = GetComponent<HapticInteractable>();
        //Add grab Listeners
        grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(StartFiring);
        grabbable.deactivated.AddListener(StopFiring);
        grabbable.selectExited.AddListener(ObjectDropped);
        //SetStats
        hapticScript.duration = weaponStats.hapticDuration;
        currentAmmo = weaponStats.ammoCount;
        //Audio
        aS = GetComponent<AudioSource>();
        aS.clip = weaponStats.firingSound;
    }
    private void Update()
    {
        if (isFiring && Time.time - lastFiredTime >= weaponStats.reloadTime)
        {
            FireBullet();
            lastFiredTime = Time.time; // update the last fired time
        }
    }
    public void StartFiring(ActivateEventArgs arg)
    {
        if (outOfAmmo)
        {
            OutOfAmmoSound();
        }
        isFiring = true;
    }

    public void StopFiring(DeactivateEventArgs arg)
    {
        isFiring = false;
    }
    public void ObjectDropped(SelectExitEventArgs arg)
    {
        isFiring = false; // Stop firing when object is dropped
    }
    private void OutOfAmmoSound()
    {
        aS.clip = weaponStats.noAmmoSound;
        aS.Play();
    }
    public void FireBullet()
    {
        if (currentAmmo <= 0)
        {
            if (outOfAmmo == false)
            {
                OutOfAmmoSound();
                outOfAmmo = true;
            }
        }
        else
        {
            aS.clip = weaponStats.firingSound;
            aS.Play();
            GameObject spawnedBullet = Instantiate(weaponStats.projectilePrefab);
            spawnedBullet.transform.position = firingPoint.position;
            spawnedBullet.GetComponent<Rigidbody>().velocity = firingPoint.forward * weaponStats.bulletSpeed;
            Destroy(spawnedBullet, 5);
            currentAmmo -= 1;
        }
        if (hapticScript != null)
        {
            hapticScript.TriggerHapticPublic();
        }

    }
}
