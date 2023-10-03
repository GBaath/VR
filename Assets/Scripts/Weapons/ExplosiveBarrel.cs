using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour, IDamageable
{
    [SerializeField] ParticleSystem explosionParticles;
    AudioSource aS;

    //Damage
    AoeDmg dmgScript;
    public int dmgAmount;
    public float dmgRadius;

    private void Start()
    {
        dmgScript = GetComponent<AoeDmg>();
        aS = GetComponent<AudioSource>();
    }

    void IDamageable.TakeDamage(int amount, bool isDead)
    {

    }

    void IDamageable.Die(float delay)
    {
        Explode();
    }

    public void Explode()
    {
        //var _new =Instantiate(explosionParticles, transform.position, explosionParticles.transform.rotation);
        //var aoegmd = _new.gameObject.AddComponent<AoeDmg>();
        //aoegmd.DealAoEDamage();
        Instantiate(explosionParticles, transform.position, explosionParticles.transform.rotation);
        DealDmg();
        aS.Play();
        Destroy(gameObject);

        
    }

    void DealDmg()
    {
        dmgScript.DealAoEDamage(transform.position, dmgAmount, dmgRadius);
    }
}
