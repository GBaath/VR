using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSounds : MonoBehaviour
{

    AudioSource aS;
    private void Start()
    {
        aS = GetComponent<AudioSource>();
    }

    public void StartSound()
    {
        aS.Play();
    }
    public void StopSound()
    {
        aS.Stop();
    }
}
