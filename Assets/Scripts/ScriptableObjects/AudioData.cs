using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AudioData", fileName = "New AudioData")]
public class AudioData : ScriptableObject
{
    public List<AudioClip> randomClipList = new();
    [Range(-3, 3)] public float pitchBase = 1;
    [Range(-3, 3)] public float pitchRange = 0;
    [Range(0, 1)] public float volume = 1;

    public AudioClip GetRandomClip() {
        return randomClipList[Random.Range(0, randomClipList.Count - 1)];
    }
}
