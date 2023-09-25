using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class CrossbowSocket : MonoBehaviour
{




    private XRSocketInteractor socketInteractor;

    private void Awake()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
        socketInteractor.onSelectEntered.AddListener(OnObjectInserted);
        socketInteractor.onSelectExited.AddListener(OnObjectRemoved);
    }

    private void OnObjectInserted(XRBaseInteractable interactable)
    {

        if(interactable.TryGetComponent<Crossbow>(out Crossbow crossbowScript))
        {
            crossbowScript.InWeaponSlot();
        }
        //Crossbow crossbow = interactable.GetComponent<Crossbow>();
        //if (crossbow)
        //{
        //    crossbow.InWeaponSlot();
        //}
    }

    private void OnObjectRemoved(XRBaseInteractable interactable)
    {

        if (interactable.TryGetComponent<Crossbow>(out Crossbow crossbowScript))
        {
            crossbowScript.NotInWeaponSlot();
        }
        //Crossbow crossbow = interactable.GetComponent<Crossbow>();
        //if (crossbow)
        //{
        //    crossbow.NotInWeaponSlot();
        //}
    }
}


