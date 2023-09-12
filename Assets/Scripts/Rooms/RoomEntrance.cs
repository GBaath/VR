using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEntrance : MonoBehaviour
{
    public GameObject previousRoom;

    RoomBase roomBase;

    public RoomEntrance nextRoomEntrance;
    //[SerializeField] public Transform nextRoomSpawnpoint;

    public List <GameObject> enemies, loot, props;

    private bool enemiesLoaded, lootLoaded, propsLoaded;
    int indexE=0, indexL=0, indexP = 0;

    public void UnloadPrev()
    {
        previousRoom.SetActive(false);
        //todo destroy and pool
    }
    public void LoadNext()
    {
        nextRoomEntrance.LoadRoomContent();
    }
    public void LoadRoomContent()
    {
        //TODO spawn modules from connectionpoint

        ////spawn entrance module and set variables
        var _new = Instantiate(GameManager.instance.roomManager.GetNewRoomBase(),transform.position,Quaternion.identity);
        roomBase = _new.GetComponent<RoomBase>();
        roomBase.entrance = this;
        roomBase.transform.position = transform.position;



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
