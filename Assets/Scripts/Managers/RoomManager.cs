using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField]private List<GameObject> roomBaseList, roomExtensionList, corridorList;
    public GameObject roomEntrance;
    public bool nextIsCorridor;

    public int roomsPassed;
    
    public GameObject GetNewRoomBase()
    {
        return roomBaseList[Random.Range(0,roomBaseList.Count)];
    }
    public GameObject GetNewCorridor()
    {
        return corridorList[Random.Range(0, corridorList.Count)];
    }
    public GameObject GetNewRoomExtension()
    {
        return roomExtensionList[Random.Range(0, roomExtensionList.Count)];
    }
}
