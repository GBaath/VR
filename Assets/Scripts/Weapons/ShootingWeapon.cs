using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class ShootingWeapon : MonoBehaviour
{
    public ShootingWeaponSO weaponStats;
    public HapticInteractable hapticScript;
    public Transform firingPoint;
    public int currentAmmo;
    private AudioSource aS;
    bool isFiring;
    private float lastFiredTime = 0f; // time the last bullet was fired

    private XRGrabInteractable grabbable;
    private Vector3 originalSpread;
    [SerializeField] ParticleSystem cartrideParticles;
    [SerializeField] ParticleSystem fireParticles;

    [SerializeField] Animator fireAnimator;
    [SerializeField] WeaponType currentWeaponType;


    //Select stuff
    private Coroutine deselectCoroutine;


    //For spining thing
    public bool isSelected;
    [SerializeField]LootFloat lootFloatScript;

    //Reload
    [SerializeField]Canvas reloadCanvas;
    [SerializeField] Image reloadCircle;
    private bool outOfAmmo = false;
    private bool isReloading = false;
    public bool canReloadBoost = true;
    IEnumerator reloadFail;

    //chest check
    public bool isInChest;
    
    public enum WeaponType
    {
        SMG,
        Pistol,
        Shotgun

    }
    private void Start()
    {
        reloadCanvas.enabled = false;
        hapticScript = GetComponent<HapticInteractable>();
        //Add grab Listeners
        grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(StartFiring);
        grabbable.deactivated.AddListener(StopFiring);
        //grabbable.lastselectExited.AddListener(ObjectDropped);
        grabbable.lastSelectExited.AddListener(ObjectDropped);
        grabbable.lastSelectExited.AddListener(SetSelectFalse);
        grabbable.firstSelectEntered.AddListener(SetSelectTrue);


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

                if (isFiring && Time.time - lastFiredTime >= weaponStats.fireRate)
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
            if(isReloading&&canReloadBoost)
                BoostReloadCheck();
            else
                OutOfAmmoSound();
            return;
        }
        else
        {
            
            if (fireAnimator != null)
            {
                fireAnimator.SetBool("Fire", true);

            }
        }

        switch (currentWeaponType)
        {
            case WeaponType.SMG:
                isFiring = true;
                break;
            case WeaponType.Pistol:
                FireBullet();
                Invoke(nameof(StopParticleEffect), 0.05f);
                break;
            case WeaponType.Shotgun:
                for (int i = 0; i < weaponStats.shotgunSlugs; i++)
                {
                    FireBullet();
                }
                currentAmmo -= 1;
                break;
        }
        if (currentAmmo <= 0)
        {
            if (!outOfAmmo)
            {
                Invoke(nameof(StartReload),0.4f);
                outOfAmmo = true;
            }
        }

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
    private void ObjectDropped(SelectExitEventArgs arg)
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
                StartReload();
                outOfAmmo = true;
            }
        }
        else
        {
            StartParticleEffect();
            BulletSpread();
            FireSound();
            GameObject spawnedBullet = Instantiate(weaponStats.projectilePrefab);
            spawnedBullet.GetComponent<ProjectileDamage>().damage = weaponStats.damage;
            spawnedBullet.transform.position = firingPoint.position;
            spawnedBullet.GetComponent<Rigidbody>().velocity = firingPoint.forward * weaponStats.bulletSpeed;
            Destroy(spawnedBullet, 2);
            switch (currentWeaponType)
            {
                case WeaponType.SMG:
                case WeaponType.Pistol:
                    currentAmmo -= 1;
                    break;
            }

            ResetBulletSpread();
            hapticScript.TriggerHapticPublic();


            GameManager.instance.playerPhysicsBase.GetComponent<WeaponPhysicsMove>().ApplyWeaponForce(firingPoint.forward, weaponStats.knockBack);
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
    private void StartReload()
    {
        if (isReloading) return;
        reloadCanvas.enabled = true;
        aS.clip = weaponStats.reloadSound;
        aS.Play();
        isReloading = true;
        reloadCircle.fillAmount = 0;
        //Invoke(nameof(StopReload), weaponStats.reloadTime);
        StartCoroutine(ReloadProgress());
    }
    private void StopReload()
    {
        StopCoroutine(ReloadProgress());
        reloadCanvas.enabled = false;
        reloadCircle.fillAmount = 0;
        currentAmmo = weaponStats.ammoCount;
        outOfAmmo = false;
        isReloading = false;
        aS.Stop();
        aS.clip = weaponStats.reloadSound;
        aS.Play();
    }
    //Reload progress for reload circle
    private IEnumerator ReloadProgress()
    {
        StopParticleEffect();
        StopShootingAnimation();
        float reloadTime = weaponStats.reloadTime;
        float timePassed = 0f;

        while (timePassed < reloadTime&&reloadCircle.fillAmount<1)
        {
            timePassed += Time.deltaTime;
            reloadCircle.fillAmount = timePassed / reloadTime;
            yield return null;
        }
        StopReload();
    }
    IEnumerator LerpreLoadFail(float duration)
    {
        float time = 0;
        Color startValue = Color.red;
        Color endValue = Color.white;
        canReloadBoost = false;
        while (time < duration)
        {
            reloadCircle.color = Color.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        reloadCircle.color = endValue;
        canReloadBoost = true;
    }
    private void BoostReloadCheck()
    {
        if (!isReloading)
            return;
        //within 10% of .5 hardcoded reload timer
        if (Mathf.Abs(.5f - reloadCircle.fillAmount) < .1f)
        {
            reloadCircle.fillAmount = 1;

            //StopCoroutine(ReloadProgress());
            //StopReload();
        }
        else
        {
            reloadFail = LerpreLoadFail(1);
            StopCoroutine(reloadFail);
            StartCoroutine(reloadFail);
        }
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
        if (fireParticles != null)
        {
            fireParticles.Stop();
        }
    }

    public void StopShootingAnimation()
    {
        if (fireAnimator != null)
        {
            fireAnimator.SetBool("Fire", false);
        }
    }
    //private void invokeSelectTrue()
    //{
    //    isSelected = true;
    //}
    private void SetSelectTrue(SelectEnterEventArgs arg)
    {
        if (deselectCoroutine != null)
        {
            StopCoroutine(deselectCoroutine);
            deselectCoroutine = null;
        }
        isSelected = true;
    }
    private void SetSelectFalse(SelectExitEventArgs arg)
    {
        if (deselectCoroutine != null)
        {
            StopCoroutine(deselectCoroutine);
        }
        deselectCoroutine = StartCoroutine(DeselectAfterDelay());
    }
    private IEnumerator DeselectAfterDelay()
    {
        yield return new WaitForSeconds(0.3f);
        isSelected = false;
    }
}
