using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LootPickup : MonoBehaviour
{ 
    public LootResource lootResource;

   

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

        var renderer = gameObject.GetComponent<Renderer>();

        renderer.material = lootResource.material;

        GetComponent<Rigidbody>().mass = lootResource.customMass;
        transform.localScale = Vector3.one*lootResource.customScale;

    }
    void SetNewRandomLootResource()
    {
        lootResource = AssetLoader.instance.lootResources[Random.Range(0,AssetLoader.instance.lootResources.Capacity)];
    }
}
