using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChestRemoveSelected : MonoBehaviour
{


    XRSocketInteractor socket;



    ShootingWeapon sw;
    Crossbow cb;
    void Start()
    {
        socket = GetComponent<XRSocketInteractor>();
    }

    //public void SetNotSelected()
    //{
    //    Invoke(nameof(nono), 0.1f);
    //}
    public void RemoveFromChest()
    {
 
            IXRSelectInteractable objName = socket.GetOldestInteractableSelected();
            objName.transform.gameObject.TryGetComponent<ShootingWeapon>(out sw);
            if (sw != null)
            {
                sw.isInChest = false;
            }
            else if (sw == null)
            {
                objName.transform.gameObject.TryGetComponent<Crossbow>(out cb);
            }
        if (cb != null)
        {
            cb.isInChest = false;

        }
    }
    public void InstertedInChest()
    {
        IXRSelectInteractable objName = socket.GetOldestInteractableSelected();
        sw = objName.transform.gameObject.GetComponent<ShootingWeapon>();
        if (sw != null)
        {
            sw.isInChest = true;
        }
        else if (sw == null)
        {
            cb = objName.transform.gameObject.GetComponent<Crossbow>();
        }
        if(cb != null)
        {
            cb.isInChest = true;
        }
    }
}
