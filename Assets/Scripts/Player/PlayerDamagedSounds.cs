using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamagedSounds : MonoBehaviour
{
    [SerializeField] private AudioClip[] hitClips;
    [SerializeField] private AudioClip heartBeat;

    private void PlayHitSounds()
    {
        AudioSource.PlayClipAtPoint(HitSound(), transform.position, 0.4f);
    }
    
    private AudioClip HitSound()
    {
        return hitClips[UnityEngine.Random.Range(0, hitClips.Length)];
    }

    private void PlayHeartBeat()
    {
        AudioSource.PlayClipAtPoint(heartBeat, transform.position, 0.2f);
    }
}
