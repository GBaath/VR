using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWeaponInChest : MonoBehaviour
{

    public float chance1;
    public float chance2;
    public float chance3;
    public List<Color> colors;
    public List<Color> glowColors;
    public List<GameObject> Tier3;
    public List<GameObject> Tier2;
    public List<GameObject> Tier1;
    private List<GameObject> weapons;
    [SerializeField] Transform spawnPoint;
    public ParticleSystem particlesGlow;
    public ParticleSystem particleSparkles;
    GameObject spawnedWeapon;
    public SkinnedMeshRenderer rayMeshRenderer;
    private void Start()
    {
        //weapons = PickList();
        //SpawnWeapon();
        //SetEffectColour();
    }
    private void OnEnable()
    {
        weapons = PickList();
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

    List<GameObject> PickList()
    {
        float totalChance = chance1 + chance2 + chance3;
        float randomValue = Random.Range(0f, totalChance);

        if (randomValue <= chance1)
            return Tier1;
        else if (randomValue <= chance1 + chance2)
            return Tier2;
        else
            return Tier3;
    }


}
