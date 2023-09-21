using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTierEffectColor : MonoBehaviour
{
    public List<Color> colors;
    [SerializeField]ShootingWeapon weaponScript;
    [SerializeField] Light tierLight;
    [SerializeField] ParticleSystem particleSparkles;

    private void Start()
    {
        SetEffectColour();
    }
    private void SetEffectColour()
    {
        WeaponTier tier = weaponScript.weaponStats.Tier;

        switch (tier)
        {
            case WeaponTier.Tier1_Basic:
                SetParticelColours(0);
                SetLightColor(0);
                break;
            case WeaponTier.Tier2_Advanced:
                SetParticelColours(1);
                SetLightColor(1);
                break;
            case WeaponTier.Tier3_Epic:
                SetParticelColours(2);
                SetLightColor(2);
                break;
        }

    }
    private void SetParticelColours(int colorIndex)
    {
        var main = particleSparkles.main;
        main.startColor = colors[colorIndex];
    }
    private void SetLightColor(int colorIndex)
    {
        tierLight.color = colors[colorIndex];
    }
}
