using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShootingWeapon : MonoBehaviour
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
    private Vector3 originalSpread;
    [SerializeField] ParticleSystem cartrideParticles;
    [SerializeField] ParticleSystem fireParticles;

    [SerializeField] Animator fireAnimator;
    [SerializeField] WeaponType currentWeaponType;

    public enum WeaponType
    {
        SMG,
        Pistol,
        Shotgun

    }
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

        switch (currentWeaponType)
        {
            case WeaponType.SMG:

                if (isFiring && Time.time - lastFiredTime >= weaponStats.reloadTime)
                {
                    FireBullet();
                    lastFiredTime = Time.time; // update the last fired time
                }
                break;
        }

    }
    public void StartFiring(ActivateEventArgs arg)
    {
        if (outOfAmmo)
        {
            OutOfAmmoSound();
            return;
        }
        if (fireAnimator != null)
        {
            fireAnimator.SetBool("Fire", true);
            
        }

        switch (currentWeaponType)
        {
            case WeaponType.SMG:
                isFiring = true;
                break;
            case WeaponType.Pistol:
                FireBullet();
                break;
            case WeaponType.Shotgun:
                for (int i = 0; i < weaponStats.shotgunSlugs; i++)
                {
                    FireBullet();
                }
                currentAmmo -= 1;
                break;
        }
        StartParticleEffect();

    }

    public void StopFiring(DeactivateEventArgs arg)
    {
        if (fireAnimator != null)
        {
            switch (currentWeaponType)
            {
                case WeaponType.SMG:
                    fireAnimator.SetBool("Fire", false);
                    break;
            }


        }
        switch (currentWeaponType)
        {
            case WeaponType.SMG:
                isFiring = false;
                break;
        }
        StopParticleEffect();
    }
    public void ObjectDropped(SelectExitEventArgs arg)
    {
        isFiring = false; // Stop firing when object is dropped
        StopShootingAnimation();
        StopParticleEffect();
    }
    private void OutOfAmmoSound()
    {
        aS.pitch = 1;
        aS.clip = weaponStats.noAmmoSound;
        aS.Play();
    }

    private void FireSound()
    {
        aS.pitch = Random.Range(weaponStats.minPitch, weaponStats.maxPitch);
        aS.clip = weaponStats.firingSound;
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
            BulletSpread();
            FireSound();
            GameObject spawnedBullet = Instantiate(weaponStats.projectilePrefab);
            spawnedBullet.transform.position = firingPoint.position;
            spawnedBullet.GetComponent<Rigidbody>().velocity = firingPoint.forward * weaponStats.bulletSpeed;
            Destroy(spawnedBullet, 5);
            switch (currentWeaponType)
            {
                case WeaponType.SMG:
                case WeaponType.Pistol:
                    currentAmmo -= 1;
                    break;
            }

            ResetBulletSpread();
            hapticScript.TriggerHapticPublic();
        }

    }
    private void BulletSpread()
    {
        //Get start rotation
        originalSpread = firingPoint.eulerAngles;
        //Set spread
        float spreadX = Random.Range(-weaponStats.bulletSpread, weaponStats.bulletSpread);
        float spreadY = Random.Range(-weaponStats.bulletSpread, weaponStats.bulletSpread);
        float spreadZ = Random.Range(-weaponStats.bulletSpread, weaponStats.bulletSpread);
        firingPoint.rotation = Quaternion.Euler(firingPoint.eulerAngles.x + spreadX, firingPoint.eulerAngles.y + spreadY, firingPoint.eulerAngles.z + spreadZ);
    }
    private void ResetBulletSpread()
    {
        //Reset
        firingPoint.rotation = Quaternion.Euler(originalSpread);
    }
    public void StartParticleEffect()
    {
        if (cartrideParticles != null)
        {
            cartrideParticles.Play();
        }
        if (fireParticles != null)
        {
            fireParticles.Play();
        }
    }

    public void StopParticleEffect()
    {
        if (cartrideParticles != null)
        {
            cartrideParticles.Stop();
        }
        if ( fireParticles != null)
        {
            fireParticles.Stop();
        }
       
    }

    public void StopShootingAnimation()
    {
        fireAnimator.SetBool("Fire", false);
        
    }
}
