using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CrossbowFloat : MonoBehaviour
{
    private WeaponGroundState groundState;
    //ShootingWeapon weaponScript;
    Rigidbody rb;
    [SerializeField] GameObject tierEffectGO;
    [SerializeField] GameObject statsDisplay;

    private bool isSelected;

    // Stats

    public float spinSpeed = 45.0f;

    public float speed = 5f; // The speed of the movement
    public float distance = 2f; // The maximum distance the object can move up and down

    private float initialPositionY; // Initial Y position of the object
    private float newPositionY; // New Y position of the object to which it will move
    public float yOffset = 0.5f;

    private XRBaseInteractable xrInteractable;

    private void Start()
    {
        groundState = WeaponGroundState.NotOnGround;
        rb = GetComponent<Rigidbody>();
        
        if (tierEffectGO != null)
        {
            tierEffectGO.SetActive(false);
        }
        if (statsDisplay != null)
        {
            statsDisplay.SetActive(false);
        }

        xrInteractable = GetComponent<XRBaseInteractable>();
        xrInteractable.onSelectEntered.AddListener(IsInSocketOrHand);
        xrInteractable.lastSelectExited.AddListener(TurnOnGravity);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            OnGround();
        }
    }

    private void Update()
    {
        switch (groundState)
        {
            case WeaponGroundState.OnGround:
                Rotate();
                UpAndDown();
                break;
            case WeaponGroundState.NotOnGround:
                break;
        }
    }

    private void UpAndDown()
    {
        if (isSelected) return;
        newPositionY = initialPositionY + Mathf.Sin(Time.time * speed) * distance;
        transform.position = new Vector3(transform.position.x, newPositionY, transform.position.z);
    }
    private void Rotate()
    {

        if (isSelected) return;
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
    }
    private void OnGround()
    {

        if (tierEffectGO != null)
        {
            tierEffectGO.SetActive(true);
        }
        if (statsDisplay != null)
        {
            statsDisplay.SetActive(true);
        }
        initialPositionY = transform.position.y + yOffset;
        transform.rotation = Quaternion.Euler(90, 0, 0);
        groundState = WeaponGroundState.OnGround;
        if (rb != null)
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }
    public void TurnOnGravity(SelectExitEventArgs args)
    {
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }
    public void IsInSocketOrHand(XRBaseInteractor interactor)
    {
        groundState = WeaponGroundState.NotOnGround;
        if (tierEffectGO != null)
        {
            tierEffectGO.SetActive(false);
        }
        if (statsDisplay != null)
        {
            statsDisplay.SetActive(false);
        }
        //if (rb != null)
        //{
        //    rb.useGravity = true;
        //    rb.isKinematic = false;
        //}
    }
    //public void SetSelectTrue()
    //{
    //    isSelected = true;
    //}
    //public void SetSelectFalse()
    //{
    //    isSelected = false;
    //}
}
public enum CrossbowGroundSate
{
    OnGround,
    NotOnGround
}



