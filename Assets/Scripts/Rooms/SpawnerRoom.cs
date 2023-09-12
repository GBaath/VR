using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerRoom : MonoBehaviour
{
    [SerializeField] RoomEntrance entrance;
    [SerializeField] Transform spawnPos;

    void Start()
    {
        var _new = Instantiate(GameManager.instance.roomManager.roomEntrance, spawnPos.position, Quaternion.identity);

        entrance = _new.GetComponent<RoomEntrance>();
        entrance.LoadRoomContent();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
