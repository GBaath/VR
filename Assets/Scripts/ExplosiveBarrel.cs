using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionParticles;
    AudioSource aS;
    private void Start()
    {
        aS = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {           
            Instantiate(explosionParticles, transform.position, explosionParticles.transform.rotation);
            aS.Play();
            Destroy(gameObject, 0.5f);
        }
    }
    
}
