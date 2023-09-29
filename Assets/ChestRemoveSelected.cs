using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChestRemoveSelected : MonoBehaviour
{


    XRSocketInteractor socket;

    void Start()
    {
        socket = GetComponent<XRSocketInteractor>();
    }

    ShootingWeapon sw;


   public void SetNotSelected()
    {
        Invoke(nameof(nono), 0.1f);
    }
    private void nono()
    {
        IXRSelectInteractable objName = socket.GetOldestInteractableSelected();
        sw = objName.transform.gameObject.GetComponent<ShootingWeapon>();
        if (sw != null)
        {
            sw.isSelected = false;
        }
    }
}
