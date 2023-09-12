using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField]private List<GameObject> roomBaseList, roomExtensionList;
    public GameObject roomEntrance;
    
    public GameObject GetNewRoomBase()
    {
        return roomBaseList[Random.Range(0,roomBaseList.Count)];
    }
    public GameObject GetNewRoomExtension()
    {
        return roomExtensionList[Random.Range(0, roomExtensionList.Count)];
    }
}
