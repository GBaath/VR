using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionsPoint : MonoBehaviour
{
    public bool isUsed;
    public RoomBase baseConnection;


    public void SpawnRandomModule()
    {
        //connectionpoint has set rotation and pos
        var _new = Instantiate(GameManager.instance.roomManager.GetNewRoomExtension(),transform);
        _new.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        isUsed = true;
    }
    public void SpawnExitModule(out RoomEntrance spawned)
    {
        var _new = Instantiate(GameManager.instance.roomManager.roomEntrance, transform);
        _new.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        isUsed = true;
        spawned = _new.GetComponent<RoomEntrance>();
        spawned.previousRoom = baseConnection.entrance.gameObject;
    }

}
