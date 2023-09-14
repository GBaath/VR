using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSpawnPoint : MonoBehaviour
{
    public PropResource.PropSize size;
    public PropResource.PropType type;
    public bool randomSize, randomType;
    PropResource pr;
    new Renderer renderer;

    private void OnEnable()
    {
        SpawnNewProp();
    }
    void SpawnNewProp()
    {
        var al = GameManager.instance.assetLoader;
        
        if(pr == null) 
        {
            if (randomSize && randomType)
            {
                pr = al.GetNewPropResource();
            }
            else if (randomSize)
            {
                pr = al.GetNewPropResource(type);
            }
            else if (randomType)
            {
                pr = al.GetNewPropResource(size);
            }
            else
            {
                pr = al.GetNewPropResource(size, type);
            }
        }
        if (pr.useActualPrefab)
        {
            Instantiate(pr.propPrefab, transform);

            return;
        }

        var meshFilter = gameObject.GetComponent<MeshFilter>();
        meshFilter.mesh = pr.propPrefab.GetComponent<MeshFilter>().sharedMesh;

        var meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = meshFilter.mesh;

        renderer = gameObject.GetComponent<Renderer>();
        renderer.material = pr.propMaterial;
    }
}
