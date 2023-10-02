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

    public RoomBase roomBase;

    [HideInInspector]public RoomEntrance nextRoomEntrance;



    public void UnloadPrev()
    {
        try //fix for spawner room
        {
            //previousRoom.roomBase.gameObject.SetActive(false);
            Destroy(previousRoom.roomBase.gameObject);
            Destroy(previousRoom.gameObject);
        }
        catch { Destroy(previousRoom.gameObject); }

        //previousRoom.gameObject.SetActive(false);
  
        GameManager.instance.dw.DestroyWeaponsOnGround();
        //todo destroy and pool
    }
    public void LoadNext()
    {
        nextRoomEntrance.SpawnRoomBase();
    }
    public void SpawnRoomBase()
    {
        GameObject _new;
        if (GameManager.instance.roomManager.nextIsCorridor)
        {
            _new = Instantiate(GameManager.instance.roomManager.GetNewCorridor(), spawnpoint.position, Quaternion.identity);
            roomBase = _new.GetComponent<RoomBase>();
            roomBase.entrance = this;
        }
        else
        {
            ////spawn entrance module and set variables, correct transform connection is set from roombase
            _new = Instantiate(GameManager.instance.roomManager.GetNewRoomBase(),spawnpoint.position,Quaternion.identity);
            roomBase = _new.GetComponent<RoomBase>();
            roomBase.entrance = this;
        }

        roomBase.LoadRoomcontent();
        loadDoor.Lock(false);

        GameManager.instance.roomManager.roomsPassed++;
        GameManager.instance.roomManager.nextIsCorridor = !GameManager.instance.roomManager.nextIsCorridor;
    }

}
