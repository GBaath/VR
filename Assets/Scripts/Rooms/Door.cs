using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class Door : MonoBehaviour
{
    public bool locked;
    public XRGrabInteractable doorhandle;

    public void Lock(bool _lock)
    {
        doorhandle.enabled = !_lock;
        locked = _lock;
    }
}
