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

    //isLoaded for Arrow check and isArmed for slider check
    bool isArmed, isLoaded;
    private void Start()
    {

        hapticScript = GetComponent<HapticInteractable>();
    }
    public void CheckValue()
    {

        if (reloadSlider.value == 0)
        {
            socketInteractor.socketActive = true;
            isArmed = true;

        }
        else
        {
            isArmed = false;
            socketInteractor.socketActive = false;

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

    }
    public void AddArrow(SelectEnterEventArgs args)
    {

        isLoaded = true;
        //if (args.interactableObject.transform.TryGetComponent(out CrossbowArrow crossbowArrow))//If you got this do the method
        //{
        //    Debug.Log("AddArrow");
        //    //crossbowArrow.InsertedInCrossbow();

        //}

    }
    public void RemoveArrow(SelectExitEventArgs args)
    {
        isLoaded = false;

    }




}
