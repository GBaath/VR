using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class ActivateGrab : MonoBehaviour
{
    XRGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.enabled = false;
    }
    public void EnableGrab()
    {
        grabInteractable.enabled = true;
    }
    public void DeactivateGrab()
    {
        grabInteractable.enabled = false;
    }

}
