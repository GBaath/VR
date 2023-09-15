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
    public void Explode()
    {
        Instantiate(explosionParticles, transform.position, explosionParticles.transform.rotation);
        aS.Play();
        Destroy(gameObject, 0.5f);
    }

}
