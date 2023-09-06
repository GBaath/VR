using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class AnimateHandOnInput : MonoBehaviour
{

    [SerializeField] InputActionProperty pinchAnimationAction;
    [SerializeField] InputActionProperty gripAnimationAction;
    [SerializeField] Animator handAnimator;


 
    void Update()
    {

        //Pinch
        float triggerValue = pinchAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Trigger",triggerValue);
        //Grip
        float gripValue = gripAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Grip", gripValue);
    }
}
