using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEngine.Rendering.DebugUI;

namespace UnityEngine
{
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


        public PropResource GetNewPropResource(PropResource.PropSize size, PropResource.PropType type)
        {
            List<int> indexes = new List<int>();

            foreach(PropResource pr in propResources.Where(p => p.propSize == size))
            {
                indexes.Add(propResources.IndexOf(pr));
            }
            foreach(int i in indexes)
            {
                if (propResources[i].propType != type)
                {
                    indexes.Remove(i);
                }
            }

            if(indexes.Count > 0)
            {
                int returnIndex = indexes[Random.Range(0, indexes.Count)];
                return propResources[returnIndex];
            }

            Debug.Log("no elligible props");
            return propResources[0];
        }
        public PropResource GetNewPropResource(PropResource.PropType type)
        {
            List<int> indexes = new List<int>();

            foreach (PropResource pr in propResources.Where(p => p.propType == type))
            {
                indexes.Add(propResources.IndexOf(pr));
            }

            if (indexes.Count > 0)
            {
                int returnIndex = indexes[Random.Range(0, indexes.Count)];
                return propResources[returnIndex];
            }

            Debug.Log("no elligible props");
            return propResources[0];
        }
        public PropResource GetNewPropResource(PropResource.PropSize size)
        {
            List<int> indexes = new List<int>();

            foreach (PropResource pr in propResources.Where(p => p.propSize == size))
            {
                indexes.Add(propResources.IndexOf(pr));
            }

            if (indexes.Count > 0)
            {
                int returnIndex = indexes[Random.Range(0, indexes.Count)];
                return propResources[returnIndex];
            }

            Debug.Log("no elligible props");
            return propResources[0];
        }
        public PropResource GetNewPropResource()
        {
            return propResources[Random.Range(0,propResources.Count)];
        }



        public List<LootResource> lootResources;
        public List<PropResource> propResources;
        public Material highlightMaterial;
    }

}


