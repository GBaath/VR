using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public int roomsToDragon = 10;


    [SerializeField]private List<GameObject> roomBaseList, roomExtensionList, corridorList;
    [SerializeField] GameObject dragonRoom;
    public GameObject roomEntrance;
    public bool nextIsCorridor;

    public int roomsPassed; 

    
    public GameObject GetNewRoomBase()
    {
        if(roomsPassed == roomsToDragon)
        {
            return dragonRoom;
        }
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
