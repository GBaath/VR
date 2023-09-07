using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SMG", menuName = "Weapons")]
public class ShootingWeaponSO : ScriptableObject
{
    public string weaponName;
    public GameObject projectilePrefab;
    public float reloadTime = 0.5f;
    public float bulletSpeed;
    public bool SMG;
    public float hapticDuration;
    public UnityEngine.AudioClip firingSound;
    public UnityEngine.AudioClip noAmmoSound;
    public int ammoCount;
    public float maxPitch;
    public float minPitch;
    public float bulletSpread;
}
