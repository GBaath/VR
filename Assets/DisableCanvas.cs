using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCanvas : MonoBehaviour
{

    [SerializeField] GameObject canvas;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
             Disable();
        }
    }
    public void Disable()
    {
        canvas.SetActive(false);
    }
    public void Enable()
    {
        canvas.SetActive(true);
    }
}
