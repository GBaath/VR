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

    private bool opened = false;

    public void Lock(bool _lock)
    {
        doorhandle.enabled = !_lock;
        locked = _lock;
        rb.isKinematic = _lock;

        //close
        transform.rotation = initRot;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !opened)
        {
            AudioSource.PlayClipAtPoint(GameManager.instance.audioManager.doorSqueak, transform.position);
            opened = true;
        }
    }
}
