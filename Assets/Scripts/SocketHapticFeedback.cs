using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketHapticFeedback: MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out HapticInteractable hapticScript))
        {
            hapticScript.TriggerHapticPublic();
        }
        else
        {
            Debug.Log("Cant find script");
        }
    }
    
}
