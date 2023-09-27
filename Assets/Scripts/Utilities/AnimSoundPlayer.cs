using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class AnimSoundPlayer : MonoBehaviour
{
    public AudioClip clip1, clip2, clip3;
    public float pitchRange = 0;
    public float volume = 1;

    //for use with animatorevents
    public void PlayClip1()
    {
        CustomPlayClipAtPoint(clip1, transform.position,volume,Random.Range(-pitchRange,pitchRange));
    }
    public void PlayClip2()
    {
        CustomPlayClipAtPoint(clip2, transform.position,volume,Random.Range(-pitchRange,pitchRange));
    }
    public void PlayClip3()
    {
        CustomPlayClipAtPoint(clip3, transform.position,volume,Random.Range(-pitchRange,pitchRange));
    }


    public void CustomPlayClipAtPoint(AudioClip clip, Vector3 position, [UnityEngine.Internal.DefaultValue("1.0F")] float volume, float pitch)
    {
        GameObject gameObject = new GameObject("One shot audio");
        gameObject.transform.position = position;
        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.clip = clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.Play();
        Object.Destroy(gameObject, clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }
}
