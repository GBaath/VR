using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ReturnItemToPosition : MonoBehaviour
{
    [SerializeField] Transform returnPosition;
    private XRGrabInteractable xrGrabInteractable;
    private bool isAttachedOrGrabbed = false;

    void Awake()
    {
        xrGrabInteractable = GetComponent<XRGrabInteractable>();
    }

    void OnEnable()
    {
        xrGrabInteractable.selectEntered.AddListener(OnSelectEntered);
        xrGrabInteractable.selectExited.AddListener(OnSelectExited);
    }

    void OnDisable()
    {
        xrGrabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        xrGrabInteractable.selectExited.RemoveListener(OnSelectExited);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        isAttachedOrGrabbed = true;
        CancelInvoke(nameof(ReturnAfterDelay));
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        isAttachedOrGrabbed = false;
        Invoke(nameof(ReturnAfterDelay), 3f);
    }

    void ReturnAfterDelay()
    {
        if (!isAttachedOrGrabbed)
        {
            transform.position = returnPosition.position;
        }
    }

}
