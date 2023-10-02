using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChestRemoveSelected : MonoBehaviour
{


    XRSocketInteractor socket;



    ShootingWeapon sw;

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
        sw = objName.transform.gameObject.GetComponent<ShootingWeapon>();
        if (sw != null)
        {
            sw.isInChest = false;
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
    }
}
