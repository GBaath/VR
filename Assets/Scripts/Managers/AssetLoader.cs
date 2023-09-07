using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetLoader : MonoBehaviour
{
    public static AssetLoader instance;

    private void Awake()
    {
        if (instance  && instance != this)
        {
            Destroy(this);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
    }

    public List<LootResource> lootResources;






}


