using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FireBulletOnActive : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform shootingPoint;
    public float firingSpeed;

    private XRGrabInteractable grabbable;
    private bool isFiring = false; // Track firing state

    private void Start()
    {
        grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(StartFiring);
        grabbable.deactivated.AddListener(StopFiring);
        grabbable.selectExited.AddListener(ObjectDropped);
    }

    private void Update()
    {
        // Check if firing is active
        if (isFiring)
        {
            FireBullet(); // Continuously fire bullets
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
        GameObject spawnedBullet = Instantiate(bulletPrefab);
        spawnedBullet.transform.position = shootingPoint.position;
        spawnedBullet.GetComponent<Rigidbody>().velocity = shootingPoint.forward * firingSpeed;
        Destroy(spawnedBullet, 5);
    }
}
