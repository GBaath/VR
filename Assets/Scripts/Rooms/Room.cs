using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject previousRoom;



    public Room nextRoom;
    [SerializeField] Transform nextRoomSpawnpoint;

    public List <GameObject> enemies, loot, props;

    private bool enemiesLoaded, lootLoaded, propsLoaded;
    int indexE=0, indexL=0, indexP = 0;

    public void UnloadPrev()
    {
        previousRoom.SetActive(false);
    }
    public void LoadNext()
    {
        nextRoom.Load();
    }
    public void Load()
    {
        nextRoom = Instantiate(GameManager.instance.roomManager.GetNewRoom().GetComponent<Room>());
        nextRoom.previousRoom = gameObject;
        nextRoom.transform.position = nextRoomSpawnpoint.position;


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
        Load();
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
        Load();
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
        Load();
    }
}
