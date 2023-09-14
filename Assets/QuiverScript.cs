using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuiverScript : MonoBehaviour
{

    [SerializeField] GameObject arrowPrefab;
    [SerializeField] Transform spawnPoint;
    

    public void ArrowTaken()
    {
        Instantiate(arrowPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
