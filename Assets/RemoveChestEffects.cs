using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveChestEffects : MonoBehaviour
{

    [SerializeField] GameObject effectOne;
    [SerializeField] GameObject effectTwo;

    public void RemoveEffects()
    {
        Destroy(effectOne);
        Destroy(effectTwo);
    }
}
