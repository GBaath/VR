using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(MeshRenderer),(typeof(XRGrabInteractable)))]
public class HighlightGrab : MonoBehaviour
{
    Renderer renderer;
    [SerializeField] List<Material> materials;
    Material highlightMaterial;
    XRGrabInteractable grabintr;


    void Start()
    {
        renderer = GetComponent<Renderer>();
        grabintr = GetComponent<XRGrabInteractable>();

        highlightMaterial = GameManager.instance.assetLoader.highlightMaterial;

        renderer.GetMaterials(materials);
        materials.Add(highlightMaterial);
        renderer.SetMaterials(materials);


        grabintr.hoverEntered.AddListener(HighlightToggle);
        grabintr.hoverExited.AddListener(HighlightToggle);

    }

    private void HighlightToggle(HoverEnterEventArgs arg)
    {
        renderer.materials[1].SetFloat("_Outline_Size", 0.0025f);
    }
    private void HighlightToggle(HoverExitEventArgs arg)
    {
        renderer.materials[1].SetFloat("_Outline_Size", 0f);
    }
}
