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

    //info about loded objects in room
    public List <GameObject> enemies, loot, props;

    private bool enemiesLoaded, lootLoaded, propsLoaded;
    int indexE=0, indexL=0, indexP = 0;

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
        nextRoomEntrance.LoadRoomContent();
    }
    public void LoadRoomContent()
    {
        ////spawn entrance module and set variables, correct transform connection is set from roombase
        var _new = Instantiate(GameManager.instance.roomManager.GetNewRoomBase(),spawnpoint.position,Quaternion.identity);
        roomBase = _new.GetComponent<RoomBase>();
        roomBase.entrance = this;



        if (!enemiesLoaded)
        {
            Invoke(nameof(LoadNextEnemy),.1f);
            return;
        }

        if (!lootLoaded)
        {
            Invoke(nameof(LoadNextLoot),.1f);
            return;
        }

        if (!propsLoaded)
        {
            Invoke(nameof(LoadNextProp),.1f);
            return;
        }
    }
    public void LoadNextEnemy()
    {
        if (indexE >= enemies.Count)
        {
            enemiesLoaded = true;
            return;
        }
        enemies[indexE].SetActive(true);
        indexE++;
        LoadRoomContent();
    }
    public void LoadNextLoot()
    {
        if (indexL >= loot.Count)
        {
            lootLoaded = true;
            return;
        }

        loot[indexL].SetActive(true);
        indexL++;
        LoadRoomContent();
    }
    public void LoadNextProp()
    {
        if (indexP >= props.Count)
        {
            propsLoaded = true;
            return;
        }
        props[indexP].SetActive(true);
        indexP++;
        LoadRoomContent();
    }
}
