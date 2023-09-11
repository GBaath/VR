using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public List<GameObject> roomList; 

    
    public GameObject GetNewRoom()
    {
        return roomList[Random.Range(0,roomList.Count)];
    }
}
