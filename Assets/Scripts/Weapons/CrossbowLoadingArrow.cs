using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;



public class CrossbowLoadingArrow : MonoBehaviour
{

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
        CancelInvoke(nameof(DestroyAfterDelay));
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        isAttachedOrGrabbed = false;
        Invoke(nameof(DestroyAfterDelay), 3f);
    }

    void DestroyAfterDelay()
    {
        if (!isAttachedOrGrabbed)
        {
            Destroy(gameObject);
        }
    }
    void OnApplicationQuit()
    {
        CancelInvoke(nameof(DestroyAfterDelay));
        Destroy(gameObject);
    }
}
