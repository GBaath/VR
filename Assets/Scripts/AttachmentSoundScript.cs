using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentSoundScript : MonoBehaviour
{

    AudioSource aS;
    [SerializeField] AudioClip attachSound;
    [SerializeField] AudioClip removeSound;
    private void Start()
    {
        aS = GetComponent<AudioSource>();
    }

    public void AttachSound()
    {
        aS.clip = attachSound;
        aS.Play();
    }
    public void RemoveSound()
    {
        aS.clip = removeSound;
        aS.Play();
    }
}
