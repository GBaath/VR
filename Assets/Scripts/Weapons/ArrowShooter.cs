using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShooter : MonoBehaviour
{

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform shootingPoint;
    public float firingSpeed;
    [SerializeField] GameObject loadedArrow;
    [SerializeField] UnityEngine.AudioClip reloadSound;
    [SerializeField] UnityEngine.AudioClip fireSound;
    AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void Fire()
    {
        GameObject spawnedBullet = Instantiate(bulletPrefab);
        spawnedBullet.transform.position = shootingPoint.position;
        spawnedBullet.transform.rotation = shootingPoint.rotation; // Set the rotation to match the shootingPoint
        spawnedBullet.GetComponent<Rigidbody>().velocity = shootingPoint.forward * firingSpeed;
        Destroy(spawnedBullet, 5);
        HideArrow();
    }

    private void HideArrow()
    {
        loadedArrow.SetActive(false);
        source.clip = fireSound;
        source.Play();
       
    }
    public void ShowArrow()
    {
        source.clip = reloadSound;
        source.Play();
        loadedArrow.SetActive(true);
    }
}
