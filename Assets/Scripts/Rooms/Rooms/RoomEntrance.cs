using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEntrance : MonoBehaviour
{
    //roombase needs custom point because of transform stats
    public Transform spawnpoint;
    public RoomEntrance previousRoom;

    //used from other scripts when loading and unloading
    public Door loadDoor;

    RoomBase roomBase;

    [HideInInspector]public RoomEntrance nextRoomEntrance;



    public void UnloadPrev()
    {
        try //fix for spawner room
        {
            previousRoom.roomBase.gameObject.SetActive(false);
        }
        catch { }

        previousRoom.gameObject.SetActive(false);


        //todo destroy and pool
    }
    public void LoadNext()
    {
        nextRoomEntrance.SpawnRoomBase();
    }
    public void SpawnRoomBase()
    {
        ////spawn entrance module and set variables, correct transform connection is set from roombase
        var _new = Instantiate(GameManager.instance.roomManager.GetNewRoomBase(),spawnpoint.position,Quaternion.identity);
        roomBase = _new.GetComponent<RoomBase>();
        roomBase.entrance = this;

        roomBase.LoadRoomcontent();
        loadDoor.Lock(false);

       
    }

}
