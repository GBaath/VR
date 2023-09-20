using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class Door : MonoBehaviour
{
    public bool locked;
    public XRGrabInteractable doorhandle;
    [SerializeField] Rigidbody rb;
    Quaternion initRot;

    public void Lock(bool _lock)
    {
        doorhandle.enabled = !_lock;
        locked = _lock;
        rb.isKinematic = _lock;
        transform.rotation = initRot;

        //TODO CLOSE ANIM HERE
    }
    private void LinkAnchor()
    {
        GetComponent<HingeJoint>().connectedAnchor = transform.position;
    }
    private void Start()
    {
        LinkAnchor();
        initRot = transform.rotation;
    }
    private void OnEnable()
    {
        LinkAnchor();
    }
}
