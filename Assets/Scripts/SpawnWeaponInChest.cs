using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWeaponInChest : MonoBehaviour
{

    public List<GameObject> weapons; 
    [SerializeField] Transform spawnPoint;
    private void Start()
    {
        SpawnWeapon();
    }
    private void SpawnWeapon()
    {
        int weaponIndex = Random.Range(0, weapons.Count);
        Instantiate(weapons[weaponIndex], spawnPoint.position,spawnPoint.rotation);
    }

}
