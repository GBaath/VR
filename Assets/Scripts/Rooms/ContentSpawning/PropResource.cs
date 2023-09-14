using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PropResource : ScriptableObject
{
    public enum PropSize
    {
        small, medium, large
    }
    public enum PropType
    {
        bookcase, crate, pot, seating, table, science, walldecor, debris, bottle, chest
    }
    public PropSize propSize;
    public PropType propType;
    public Material propMaterial;
    public GameObject propPrefab;

    public bool useActualPrefab;

}
