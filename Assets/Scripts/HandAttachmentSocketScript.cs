using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class HandAttachmentSocketScript : MonoBehaviour
{
    public ActionBasedController controller;
    public ShootingWeapon weapon;
    private XRSocketInteractor socket;

    private void OnEnable()
    {
        controller.activateAction.action.performed += OnActionPerformed;
        controller.activateAction.action.canceled += OnActionCanceled;
    }


    private void Start()
    {
        socket = GetComponent<XRSocketInteractor>();
    }
    private void OnDisable()
    {
        controller.activateAction.action.Disable();
    }

    private void OnActionPerformed(InputAction.CallbackContext context)
    {
        if (weapon == null) return;
        //weapon.StartFiring();
        Debug.Log("Select Action Performed");
        // Your select action handling code here
    }
    private void OnActionCanceled(InputAction.CallbackContext context)
    {
        if (weapon == null) return;
        //weapon.StopFiring();
        Debug.Log("Select Action Canceled");
        // Your select action cancellation handling code here
    }
    public void OnSelectEnter(SelectEnterEventArgs args)
    {
        StartCoroutine(OnSelectEntered());
    }

    private IEnumerator OnSelectEntered()
    {
        yield return new WaitForEndOfFrame();

        if (socket == null)
        {
            Debug.LogError("Socket is null");
            yield break;
        }

        IXRSelectInteractable objName = socket.GetOldestInteractableSelected();

        if (objName != null && objName.transform != null)
        {
            GameObject objGameObject = objName.transform.gameObject;
            if (objGameObject != null)
            {
                weapon = objGameObject.GetComponent<ShootingWeapon>();
                if (weapon == null)
                {
                    Debug.LogError("ShootingWeapon component not found on the gameObject");
                }
            }
            else
            {
                Debug.LogError("GameObject is null");
            }
        }
        else
        {
            Debug.LogError("Interactable object or its transform is null");
        }
    }

    public void OnSelectExit(SelectExitEventArgs args)
    {
        // When an object is detached, set weapon to null
        weapon = null;
    }

}



