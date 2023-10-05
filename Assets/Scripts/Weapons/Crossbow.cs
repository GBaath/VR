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
    public InteractionLayerMask hand;
    public InteractionLayerMask noInteraction;
    private XRGrabInteractable grabbable;

    //isLoaded for Arrow check and isArmed for slider check
    bool isArmed, isLoaded;
    public bool isSelected;
    public bool isInChest;

    //Select stuff
    private Coroutine deselectCoroutine;
    private void Start()
    {
        grabbable = GetComponent<XRGrabInteractable>();
        reloadArrowPrefab.SetActive(false);
        hapticScript = GetComponent<HapticInteractable>();
        NotInWeaponSlot();
        grabbable.selectEntered.AddListener(IsInHand);
        grabbable.lastSelectExited.AddListener(SetSelectFalse);
        grabbable.firstSelectEntered.AddListener(SetSelectTrue);
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
    [System.Obsolete]
    public void FireArrow(ActivateEventArgs arg)
    {
        if (isLoaded && isArmed)
        {
            
            DestroyArrow();

            isLoaded = false;
            isArmed = false;
            Fire();
            //reloadSlider.interactionLayers = hand;
            // Force the slider to drop if it's selected.

            
            Invoke(nameof(EnableInteractionSlider),0.3f);
            reloadSlider.value = 1;
            reloadSlider.enabled = false;
        }
        else
        {
            reloadSlider.value = 1;
            Debug.Log("Cant shoot");

        }
    }
    public void EnableInteractionSlider()
    {
        reloadSlider.enabled = true;
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

    private void SetSelectTrue(SelectEnterEventArgs arg)
    {
        if (deselectCoroutine != null)
        {
            StopCoroutine(deselectCoroutine);
            deselectCoroutine = null;
        }
        isSelected = true;
    }
    private void SetSelectFalse(SelectExitEventArgs arg)
    {
        if (deselectCoroutine != null)
        {
            StopCoroutine(deselectCoroutine);
        }
        deselectCoroutine = StartCoroutine(DeselectAfterDelay());
    }
    private IEnumerator DeselectAfterDelay()
    {
        yield return new WaitForSeconds(0.3f);
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
    [System.Obsolete]
    public void IsInHand(SelectEnterEventArgs args)
    {
        if (args.interactor is XRBaseControllerInteractor)
        {
            reloadSlider.enabled = true;
        }
    }


}
