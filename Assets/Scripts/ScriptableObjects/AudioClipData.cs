using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AudioClipData : ScriptableObject
{
    public AudioClip clip;
    public float pitchVariation;
    public bool looping;
}
