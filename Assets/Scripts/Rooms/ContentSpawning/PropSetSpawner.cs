using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSetSpawner : MonoBehaviour
{
    public RoomBase room;
    public List<PropSpawnPoint> spawnPoints;

    private void Start()
    {
        if(room != null)
        {
            if(room.acceptedTypes.Count > 0)
            {
                //check accepted types and only spawn accepted types
            }
        }

        foreach(PropSpawnPoint spawnPoint in spawnPoints)
        {
            spawnPoint.gameObject.SetActive(true);
        }
    }
}
