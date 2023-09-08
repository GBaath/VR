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
    private float pitch;
    private Vector3 originalSpread;
    [SerializeField] ParticleSystem cartrideParticles;

    [SerializeField] Animator sliderAnimator;

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
        if (weaponStats.SMG)
        {
            if (isFiring && Time.time - lastFiredTime >= weaponStats.reloadTime)
            {

                FireBullet();
                lastFiredTime = Time.time; // update the last fired time
            }
        }
    }
    public void StartFiring(ActivateEventArgs arg)
    {
        if (outOfAmmo)
        {
            OutOfAmmoSound();
            return;
        }
        if (sliderAnimator != null)
        {

            sliderAnimator.SetBool("Fire", true);
        }
        StartParticleEffect();
        isFiring = true;
    }

    public void StopFiring(DeactivateEventArgs arg)
    {
        if (sliderAnimator != null)
        {
            sliderAnimator.SetBool("Fire", false);

        }
        StopParticleEffect();
        isFiring = false;
    }
    public void ObjectDropped(SelectExitEventArgs arg)
    {
        isFiring = false; // Stop firing when object is dropped
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
            currentAmmo -= 1;
            ResetBulletSpread();
            hapticScript.TriggerHapticPublic();
        }

    }
    private void ResetBulletSpread()
    {
        //Reset
        firingPoint.rotation = Quaternion.Euler(originalSpread);
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
    public void StartParticleEffect()
    {
        if (cartrideParticles != null)
        {
            cartrideParticles.Play();
        }
    }

    public void StopParticleEffect()
    {
        if (cartrideParticles != null)
        {
            cartrideParticles.Stop();
        }
    }
}