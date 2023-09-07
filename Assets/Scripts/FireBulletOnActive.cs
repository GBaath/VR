using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FireBulletOnActive : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform shootingPoint;
    [SerializeField] float reloadTime = 0.5f; // time between shots in seconds
    public float bulletSpeed;


    HapticInteractable hapticScript;

    AudioSource aS;

    private float lastFiredTime = 0f; // time the last bullet was fired
    private XRGrabInteractable grabbable;
    private bool isFiring = false; // Track firing state

    private void Start()
    {
        aS = GetComponent<AudioSource>();
        hapticScript = GetComponent<HapticInteractable>();
        grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(StartFiring);
        grabbable.deactivated.AddListener(StopFiring);
        grabbable.selectExited.AddListener(ObjectDropped);
    }

    private void Update()
    {
        if (isFiring && Time.time - lastFiredTime >= reloadTime)
        {
            FireBullet();
            lastFiredTime = Time.time; // update the last fired time
        }
    }

    public void StartFiring(ActivateEventArgs arg)
    {
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

    private void FireBullet()
    {
        if (hapticScript != null)
        {

            hapticScript.TriggerHapticPublic();
        }
        aS.Play();
        GameObject spawnedBullet = Instantiate(bulletPrefab);
        spawnedBullet.transform.position = shootingPoint.position;
        spawnedBullet.GetComponent<Rigidbody>().velocity = shootingPoint.forward * bulletSpeed;
        Destroy(spawnedBullet, 5);
    }
}
