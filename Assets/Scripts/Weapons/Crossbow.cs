using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

public class Crossbow : MonoBehaviour
{
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] XRSlider reloadSlider;
    [SerializeField] XRSocketInteractor socketInteractor;
    [SerializeField] Transform firingPoint;
    [SerializeField] float arrowSpeed = 40;
    HapticInteractable hapticScript;
    [SerializeField]AudioSource fireAudioSource;
    [SerializeField] GameObject reloadArrowPrefab;


    //isLoaded for Arrow check and isArmed for slider check
    bool isArmed, isLoaded;
    public bool isSelected;
    private void Start()
    {
        reloadArrowPrefab.SetActive(false);
        hapticScript = GetComponent<HapticInteractable>();
    }
    public void CheckValue()
    {
        Debug.Log("Checking Value");
        if (reloadSlider.value == 0)
        {
            //socketInteractor.socketActive = true;
            isArmed = true;
            AddArrowNoReload();

        }
        else
        {

            
            isArmed = false;
            RemoveArrowNoReload();
            //socketInteractor.socketActive = false;

        }
    }

    public void FireArrow(ActivateEventArgs arg)
    {
        if (isLoaded && isArmed)
        {
            
            DestroyArrow();
            reloadSlider.value = 1;
            isLoaded = false;
            isArmed = false;
            Fire();

        }
        else
        {
            reloadSlider.value = 1;
            Debug.Log("Cant shoot");

        }
    }
    private void DestroyArrow()
    {
        IXRSelectInteractable objName = socketInteractor.GetOldestInteractableSelected();
        if (objName != null)
        {

            Destroy(objName.transform.gameObject);
        }
    }
    private void Fire()
    {
        hapticScript.TriggerHapticPublic();
        GameObject spawnedBullet = Instantiate(arrowPrefab);
        spawnedBullet.transform.position = firingPoint.position;
        spawnedBullet.transform.rotation = firingPoint.rotation;
        spawnedBullet.GetComponent<Rigidbody>().velocity = firingPoint.forward * arrowSpeed;
        Destroy(spawnedBullet, 5);
        fireAudioSource.Play();

    }
    public void AddArrow(SelectEnterEventArgs args)
    {

        isLoaded = true;
        IXRSelectInteractable objName = socketInteractor.GetOldestInteractableSelected();
        objName.transform.gameObject.GetComponent<XRGrabInteractable>().interactionLayers = InteractionLayerMask.GetMask("NoHands");

    }
    public void RemoveArrow(SelectExitEventArgs args)
    {
        isLoaded = false;

    }

    public void AddArrowNoReload()
    {
        isLoaded = true;
        reloadArrowPrefab.SetActive(true);

    }
    public void RemoveArrowNoReload()
    {
        isLoaded = false;
        reloadArrowPrefab.SetActive(false);
    }

    public void IsSelected()
    {
        isSelected = true;
    }
    public void IsNotSelected()
    {
        isSelected = false;
    }

    public void InWeaponSlot()
    {
        reloadSlider.enabled = false;
    }
    public void NotInWeaponSlot()
    {
        reloadSlider.enabled = true;
    }


}
