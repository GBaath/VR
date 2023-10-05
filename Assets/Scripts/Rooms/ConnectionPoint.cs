using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionsPoint : MonoBehaviour
{
    public bool isUsed;
    public RoomBase baseConnection;
    public bool isEntrance;



    //new random connection module from list
    public void SpawnRandomModule()
    {
        //connectionpoint has set rotation and pos
        var _new = Instantiate(GameManager.instance.roomManager.GetNewRoomExtension(),transform);
        _new.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        isUsed = true;
        _new.GetComponent<RoomModule>().AddPropsToBase(baseConnection);
    }
    //spawn exit module and set some new refs for new room
    public void SpawnExitModule(out RoomEntrance spawned)
    {
        var _new = Instantiate(GameManager.instance.roomManager.roomEntrance,transform.position,transform.rotation);
        isUsed = true;
        spawned = _new.GetComponent<RoomEntrance>();
        spawned.previousRoom = baseConnection.entrance;
        //spawned.loadDoor = spawned.GetComponentInChildren<Door>();
    }

}
