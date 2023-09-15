using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyCarExplosiv : MonoBehaviour
{

    public float speed = 5f, timer = 5;
    [SerializeField] Rigidbody rb;
    private bool isGrounded, explosionStarted;
    private bool buttonPressed = false;
    [SerializeField] ParticleSystem explosionEffect;


    private void Start()
    {
        ButtonPressed();
    }
    void Update()
    {
        if (isGrounded && buttonPressed) // Replace "Fire1" with your VR input button
        {
            MoveForward();
        }
        if (buttonPressed)
        {
            CountDown();
        }
    }
    public void ButtonPressed()
    {
        buttonPressed = true;
    }
    private void MoveForward()
    {
        rb.velocity = transform.forward * speed;
    }

    private void CountDown()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (!explosionStarted)
            {

                ExplodeCar();
                explosionStarted = true;
            }
        }
    }

    private void ExplodeCar()
    {
        Instantiate(explosionEffect, transform.position, explosionEffect.transform.rotation);
        Destroy(gameObject, 2);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
