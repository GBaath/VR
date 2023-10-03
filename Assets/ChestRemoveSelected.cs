using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChestRemoveSelected : MonoBehaviour
{


    XRSocketInteractor socket;


    IXRSelectInteractable item;
    public ShootingWeapon sw;
    public Crossbow cb;
    void Start()
    {
        socket = GetComponent<XRSocketInteractor>();
    }

    public void RemoveFromChest()
    {
        
        //if(socket != null)
        //{

             //item = socket.GetOldestInteractableSelected();

            // Try to get the ShootingWeapon component
            if (item.transform.gameObject.TryGetComponent<ShootingWeapon>(out sw) && sw != null)
            {
                sw.isInChest = false;
                sw = null;  // Reset sw
                return;  // Exit the method once the operation is successful
            }

            // Try to get the Crossbow component if ShootingWeapon wasn't found
            if (item.transform.gameObject.TryGetComponent<Crossbow>(out cb) && cb != null)
            {
                cb.isInChest = false;
                cb = null;  // Reset cb
            }
        //}

    }
    

    public void InstertedInChest()
    {
        item = socket.GetOldestInteractableSelected();

        // Try to get the ShootingWeapon component
        if (item.transform.gameObject.TryGetComponent<ShootingWeapon>(out sw) && sw != null)
        {
            sw.isInChest = true;
            sw = null;  // Reset sw
            return;  // Exit the method once the operation is successful
        }

        // Try to get the Crossbow component if ShootingWeapon wasn't found
        if (item.transform.gameObject.TryGetComponent<Crossbow>(out cb) && cb != null)
        {
            cb.isInChest = true;
            cb = null;  // Reset cb
        }
    }

}
