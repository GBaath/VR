using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSounds : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    private Health health;
 
     private void Awake()
     {
         health = GetComponentInParent<Health>();
     }
 
     private void OnEnable()
     {
         //health.onNearDeath += LowHealthSound;
         //health.onResetHealth += ResetHealthSound;
         //health.onhitDamage += RandomHitSound;
     }
 
     private void OnDisable()
     {
         //health.onNearDeath -= LowHealthSound;
         //health.onResetHealth -= ResetHealthSound;
         //health.onhitDamage -= RandomHitSound;
     }
 
     private void LowHealthSound()
     {
         _audioSource.clip = GameManager.instance.audioManager.heartbeat;
         _audioSource.loop = true;
         _audioSource.Play();
     }
 
     private void ResetHealthSound()
     {
         _audioSource.loop = false;
         _audioSource.Stop();
     }
 
     private void RandomHitSound()
     {
         _audioSource.PlayOneShot(HitSound());
     }
 
     private AudioClip HitSound()
     {
         return GameManager.instance.audioManager.hurtSounds[UnityEngine.Random.Range(0, GameManager.instance.audioManager.hurtSounds.Length)];
     }
}
