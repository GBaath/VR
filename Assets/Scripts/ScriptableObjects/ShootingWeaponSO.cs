using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SMG", menuName = "Weapons")]
public class ShootingWeaponSO : ScriptableObject
{
    public string weaponName;
    public GameObject projectilePrefab;
    public float fireRate = 0.5f;
    public float reloadTime = 3;
    public float bulletSpeed;
    public float knockBack = 1;
    public float damage = 1;
    public float hapticDuration;
    public UnityEngine.AudioClip firingSound;
    public UnityEngine.AudioClip noAmmoSound;
    public UnityEngine.AudioClip reloadSound;
    public int ammoCount;
    public float maxPitch;
    public float minPitch;
    public float bulletSpread;
    public int shotgunSlugs;
    public WeaponTier Tier;
    
}
public enum WeaponTier
{
    Tier1_Basic,
    Tier2_Advanced,
    Tier3_Epic,
}