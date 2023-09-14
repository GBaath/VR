using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticInteractable : MonoBehaviour
{
    [Range(0,1)]
    public float intensity;
    public float duration;
    XRBaseControllerInteractor baseController;
    


    void Start()
    {
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        interactable.activated.AddListener(SetController);
        interactable.deactivated.AddListener(RemoveController);
        
        
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

        Debug.Log("Set controller");
        if (eventArgs.interactorObject is XRBaseControllerInteractor controllerInteractor)
        {
            baseController = controllerInteractor;
        }
    }

    public void TriggerHapticPublic()
    {
        if (baseController == null) { Debug.Log("no controller"); return; }
        
        if (intensity > 0)
        {
            baseController.SendHapticImpulse(intensity, duration);
        }
    }
    public void TriggerHaptic(XRBaseController controller)
    {
        if(intensity > 0)
        {
            controller.SendHapticImpulse(intensity, duration);
        }
    }

}
