using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWeaponInChest : MonoBehaviour
{
    public List<Color> colors;
    public List<Color> glowColors;
    public List<GameObject> weapons;
    [SerializeField] Transform spawnPoint;
    public ParticleSystem particlesGlow;
    public ParticleSystem particleSparkles;
    GameObject spawnedWeapon;
    public SkinnedMeshRenderer rayMeshRenderer;
    private void Start()
    {
        SpawnWeapon();
        SetEffectColour();
    }
    private void SpawnWeapon()
    {
        int weaponIndex = Random.Range(0, weapons.Count);
        spawnedWeapon = Instantiate(weapons[weaponIndex], spawnPoint.position, spawnPoint.rotation);
    }
    private void SetEffectColour()
    {
        WeaponTier tier = spawnedWeapon.GetComponent<ShootingWeapon>().weaponStats.Tier;
        
        switch (tier)
        {
            case WeaponTier.Tier1_Basic:
                SetParticelColours(0);
                SetGlowColour(glowColors[0]);
                break;
            case WeaponTier.Tier2_Advanced:
                //main.startColor = colors[1];
                SetParticelColours(1);
                SetGlowColour(glowColors[1]);
                break;
            case WeaponTier.Tier3_Epic:
                SetParticelColours(2);
                SetGlowColour(glowColors[2]);
                break;
        }

    }
    private void SetParticelColours(int colorIndex)
    {
        var main = particlesGlow.main;
        main.startColor = colors[colorIndex];
        main = particleSparkles.main;
        main.startColor = colors[colorIndex];
    }
    private void SetGlowColour(Color newColor)
    {
        Material newMat = new Material(rayMeshRenderer.material);
        newMat.SetColor("_TintColor", newColor);
        rayMeshRenderer.material = newMat;
    }

}
