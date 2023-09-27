using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSoundPlayer : MonoBehaviour
{
    public AudioClip clip1, clip2, clip3;

    //for use with animatorevents
    public void PlayClip1()
    {
        AudioSource.PlayClipAtPoint(clip1, transform.position);
    }
    public void PlayClip2()
    {
        AudioSource.PlayClipAtPoint(clip2, transform.position);
    }
    public void PlayClip3()
    {
        AudioSource.PlayClipAtPoint(clip3, transform.position);
    }
}
