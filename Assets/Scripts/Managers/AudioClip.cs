using UnityEngine;

[CreateAssetMenu(fileName = "NewAudioClip", menuName = "AudioClip")]
public class AudioClip : ScriptableObject
{
    public AudioClip audioClip;
    public float pitch;

    public float GetRandomPitch()
    {
        return pitch;
    }
}
