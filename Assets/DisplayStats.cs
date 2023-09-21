using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayStats : MonoBehaviour
{
    [SerializeField]TMP_Text StatsText;
    [SerializeField] ShootingWeapon weaponScript;
    Image displayBKG;
    [SerializeField]SetTierEffectColor tierColor;
    private void Start()
    {
        displayBKG = GetComponent<Image>();
        GetAndSetStats();
        SetBKGColour();
    }


    private void GetAndSetStats()
    {
        string statsInfo = weaponScript.weaponStats.weaponName + "\n" +  //Weapon name
                   "RLD Time: " + weaponScript.weaponStats.reloadTime + "\n" +
                   "DMG: " + weaponScript.weaponStats.damage + "\n" +
                   "Ammo: " + weaponScript.weaponStats.ammoCount;
        //"Weapon Tier: " + weaponScript.weaponStats.Tier;

        StatsText.text = statsInfo;
        
    }
    private void SetBKGColour()
    {
        WeaponTier tier = weaponScript.weaponStats.Tier;

        switch (tier)
        {
            case WeaponTier.Tier1_Basic:
                displayBKG.color = tierColor.colors[0];
                break;
            case WeaponTier.Tier2_Advanced:
                displayBKG.color = tierColor.colors[1];
                break;
            case WeaponTier.Tier3_Epic:
                displayBKG.color = tierColor.colors[2];
                break;
        }

    }
}
