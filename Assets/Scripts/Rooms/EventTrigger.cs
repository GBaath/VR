using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    public UnityEvent linkedEvent;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            linkedEvent.Invoke();
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
