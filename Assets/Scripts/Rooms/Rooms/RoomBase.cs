using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomBase : MonoBehaviour
{
    [Tooltip("NEEDS TO HAVE LEAST 2 CONNECTIONS")]
    [SerializeField] Transform connectionspointsHolder;
    [SerializeField]List<ConnectionsPoint> connectionsPoints;
    [SerializeField] Transform roomBase;
    [HideInInspector] public RoomEntrance entrance;
    List<int> freeIndexes = new List<int>();

    [SerializeField] Transform enemyHolder;

    [Tooltip("Leave empty for all")]
    public List<PropResource.PropType> acceptedTypes;
    private Material customRoomMaterial;

    //info about loded objects in room
    public List<GameObject> enemies, loot, props, propSets, switchSpawnPoints;

    [SerializeField]private bool enemiesLoaded, lootLoaded, propsLoaded;
    int indexE = 0, indexL = 0, indexP = 0;

    private void Start()
    {
        customRoomMaterial = GameManager.instance.assetLoader.dungeonMaterials[Random.Range(0, GameManager.instance.assetLoader.dungeonMaterials.Length)];

        //link connections point transforms
        for (int j = 0; j < connectionspointsHolder.childCount; j++)
        {
            connectionsPoints.Add(connectionspointsHolder.GetChild(j).GetComponent<ConnectionsPoint>());
            connectionsPoints[j].baseConnection = this;
        }
        int i = Random.Range(0, propSets.Count);
        propSets[i].SetActive(true);
        //this might break ad add parent to list also
        for (int j = 0; j < propSets[i].transform.childCount; j++)
        {
            props.Add(propSets[i].transform.GetChild(j).gameObject);
        }
        for (int j = 0; j < enemyHolder.childCount; j++)
        {
            enemies.Add(enemyHolder.GetChild(j).gameObject);
        } 



        SetEntryConnection();
        SpawnConnections();
    }
    //places the roombase correctly for random connection point
    void SetEntryConnection()
    {
        int r = Random.Range(0, connectionsPoints.Count);

        connectionsPoints[r].isUsed = true;


        //connect to rotationpoint and connect
        roomBase.parent = connectionsPoints[r].transform;

        foreach (ConnectionsPoint c in connectionsPoints.Where(cp => !cp.GetComponent<ConnectionsPoint>().isUsed))
        {
            c.transform.parent = roomBase;
            //save indexes of free connectionpoints
            freeIndexes.Add(connectionsPoints.IndexOf(c));
        }

        //rotation transform magic for correct pos
        connectionsPoints[r].transform.position = entrance.spawnpoint.position;
        connectionsPoints[r].transform.rotation = entrance.spawnpoint.rotation; ;
    }
    public void SpawnConnections()
    {

        //get random free point
        int index = freeIndexes[Random.Range(0, freeIndexes.Count)];

        //spawn exit first
        connectionsPoints[index].SpawnExitModule(out entrance.nextRoomEntrance);

        //spawn new module on free space from roommanager
        foreach (ConnectionsPoint c in connectionsPoints.Where(cp => !cp.GetComponent<ConnectionsPoint>().isUsed))
        {
            c.SpawnRandomModule();
        }
    }
    public void LoadTrigger()
    {
        entrance.UnloadPrev();
        entrance.LoadNext();
        entrance.loadDoor.Lock(true);
    }
    public void LoadRoomcontent()
    {
        if (!enemiesLoaded)
        {
            Invoke(nameof(LoadNextEnemy), .1f);
            return;
        }

        if (!lootLoaded)
        {
            Invoke(nameof(LoadNextLoot), .1f);
            return;
        }

        if (!propsLoaded)
        {
            Invoke(nameof(LoadNextProp), .1f);
            return;
        }

        entrance.nextRoomEntrance.loadDoor.Lock(false);
    }

    public void LoadNextEnemy()
    {
        if (indexE >= enemies.Count)
        {
            enemiesLoaded = true;
            LoadRoomcontent();
            return;
        }
        enemies[indexE].SetActive(true);
        indexE++;
        LoadRoomcontent();
    }
    public void LoadNextLoot()
    {
        if (indexL >= loot.Count)
        {
            lootLoaded = true;
            LoadRoomcontent();
            return;
        }

        loot[indexL].SetActive(true);
        indexL++;
        LoadRoomcontent();
    }
    public void LoadNextProp()
    {
        if (indexP >= props.Count)
        {
            propsLoaded = true;
            LoadRoomcontent();
            return;
        }
        props[indexP].SetActive(true);

        if(customRoomMaterial != null)
        {
            if (props[indexP].GetComponent<MeshRenderer>())
                props[indexP].GetComponent<MeshRenderer>().sharedMaterial = customRoomMaterial;
        }
        indexP++;
        LoadRoomcontent();
    }
}
