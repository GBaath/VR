using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticInteractable : MonoBehaviour
{
    public string Spotinditactor;
    [Range(0, 1)]
    public float intensity;
    public float duration;
    XRBaseControllerInteractor baseController;
    public bool shouldNotAutoListen;
    void Start()
    {
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        if (!shouldNotAutoListen)
        {
        if (interactable != null)
        {
            interactable.activated.AddListener(SetController);
            //interactable.deactivated.AddListener(RemoveController);
            interactable.hoverEntered.AddListener(HoverEnter);
            //interactable.hoverExited.AddListener(HoverExit);
        }

        }
    }

    public void RemoveController(BaseInteractionEventArgs eventArgs)
    {
        if (eventArgs.interactorObject is XRBaseControllerInteractor controllerInteractor)
        {
            
            baseController = null;
        }
    }

    public void SetController(BaseInteractionEventArgs eventArgs)
    {
        if (eventArgs.interactorObject is XRBaseControllerInteractor controllerInteractor)
        {
            baseController = controllerInteractor;
        }
    }

    public void HoverEnter(BaseInteractionEventArgs eventArgs)
    {
        if (eventArgs.interactorObject is XRBaseControllerInteractor controllerInteractor)
        {
            baseController = controllerInteractor;
            TriggerHaptic(controllerInteractor.xrController);
        }
    }

    public void HoverExit(BaseInteractionEventArgs eventArgs)
    {
        if (eventArgs.interactorObject is XRBaseControllerInteractor controllerInteractor)
        {
            baseController = null;
        }
    }

    public void TriggerHapticPublic()
    {
        if (baseController == null) { return; }

        if (intensity > 0)
        {
            baseController.SendHapticImpulse(intensity, duration);
        }
    }

    public void TriggerHaptic(XRBaseController controller)
    {
        if (intensity > 0)
        {
            controller.SendHapticImpulse(intensity, duration);
        }
    }
}
