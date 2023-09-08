using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LootPickup : MonoBehaviour
{ 
    public LootResource lootResource;

    new private Renderer renderer;

    private void Start()
    {
        if (lootResource == null)
            SetNewRandomLootResource();

        //failed load
        if (lootResource == null)
            return;

        var meshFilter = gameObject.GetComponent<MeshFilter>();
        meshFilter.mesh = lootResource.lootProp.GetComponent<MeshFilter>().sharedMesh;

        var meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = meshFilter.mesh;

        var meshRenderer = gameObject.GetComponent<MeshRenderer>();

        renderer = gameObject.GetComponent<Renderer>();

        renderer.material = lootResource.material;

        GetComponent<Rigidbody>().mass = lootResource.customMass;
        transform.localScale = Vector3.one*lootResource.customScale;

    }  
    public void HighlightToggle(bool enabled)
    {
        if (enabled)
        {
            renderer.materials[1].SetFloat("_Outline_Size", 0.0025f);
        }
        else
        {
            renderer.materials[1].SetFloat("_Outline_Size", 0f);
        }
    }


    void SetNewRandomLootResource()
    {
        lootResource = AssetLoader.instance.lootResources[Random.Range(0,AssetLoader.instance.lootResources.Capacity)];
    }
}
