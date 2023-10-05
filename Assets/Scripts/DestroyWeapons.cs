using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWeapons : MonoBehaviour
{

    [ContextMenu("Destroy weapons")]

    private void TheFunctionInoked()
    {
        ShootingWeapon[] foundWeapons = FindObjectsOfType<ShootingWeapon>();

        foreach (ShootingWeapon weapon in foundWeapons)
        {
            if (!weapon.isSelected && !weapon.isInChest)
            {
                Destroy(weapon.gameObject);
            }
        }
        Crossbow[] foundCrossbows = FindObjectsOfType<Crossbow>();
        foreach (Crossbow crossbow in foundCrossbows)
        {
            if (!crossbow.isSelected && !crossbow.isInChest)
            {
                Destroy(crossbow.gameObject);
            }
        }
    }
    public void DestroyWeaponsOnGround()
    {

        Invoke(nameof(TheFunctionInoked), 0.1f);
    }

}
