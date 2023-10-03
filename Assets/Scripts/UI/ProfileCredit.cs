using System.Collections.Generic;
using UnityEngine;

public class ProfileCredit : MonoBehaviour {
    public AudioClip voiceClip;
    public AudioSource audioSource;
    public List<GameObject> objectsToToggle = new();
    public bool trigger = false;

    private void OnCollisionEnter(Collision other) {
        EnableThis();
    }

    private void Update() {
        if (trigger) {
            trigger = false;
            EnableThis();
        }
    }

    public void EnableThis() {
        DisableAll();
        foreach (GameObject toggleObject in objectsToToggle) {
            toggleObject.SetActive(true);
        }
        audioSource.PlayOneShot(voiceClip, 1);
        Invoke(nameof(DisableAll), voiceClip.length);
    }

    public void DisableAll() {
        foreach (ProfileCredit profileCredit in FindObjectsOfType<ProfileCredit>()) {
            profileCredit.audioSource.Stop();
            profileCredit.CancelInvoke();
            foreach (GameObject toggleObject in profileCredit.objectsToToggle) {
                toggleObject.SetActive(false);
            }
        }
    }
}
