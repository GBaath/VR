using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class LootResource : ScriptableObject
{
    public GameObject lootProp;

    public Material material;

    public int value;
    public float customScale = 1;
    public float customMass = 1;

}
