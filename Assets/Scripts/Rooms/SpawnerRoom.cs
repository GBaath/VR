using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerRoom : MonoBehaviour
{
     RoomEntrance entrance;
    [SerializeField] Transform spawnPos;

    void Start()
    {
        var _new = Instantiate(GameManager.instance.roomManager.roomEntrance, spawnPos.position, spawnPos.rotation);

        entrance = _new.GetComponent<RoomEntrance>();
        entrance.previousRoom = GetComponent<RoomEntrance>();
        entrance.LoadRoomContent();
    }
}
