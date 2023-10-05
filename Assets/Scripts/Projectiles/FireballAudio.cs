using UnityEngine;

public class FireballAudio : MonoBehaviour {
    [SerializeReference] AudioClip fireballImpact;
    [SerializeReference] AudioSource audioSource;

    public void PlayFireballImpact() {
        audioSource.PlayOneShot(fireballImpact, 1);
    }
}
