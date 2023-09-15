using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempenemySpawner : MonoBehaviour
{
    public GameObject enemy;
    [SerializeField]private List<Transform> spawnpoints;


    private void Start()
    {
        foreach (Transform t in transform.GetComponentsInChildren<Transform>())
        {
            spawnpoints.Add(t);
        }
        spawnpoints.RemoveAt(0);
        //spawnpoints = transform.GetComponentsInChildren<Transform>();
    }
    public void SpawnEnemies(int amount)
    {
        foreach(Transform transform in spawnpoints)
        {
            Instantiate(enemy, transform);
        }
    }
}
