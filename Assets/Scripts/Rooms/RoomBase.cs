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

    private void Start()
    {
        //link connections point transforms
        for (int i = 0; i < connectionspointsHolder.childCount; i++)
        {
            connectionsPoints.Add(connectionspointsHolder.GetChild(i).GetComponent<ConnectionsPoint>());
            connectionsPoints[i].baseConnection = this;
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
        Debug.Log("trg");
        entrance.UnloadPrev();
        entrance.LoadNext();
        entrance.loadDoor.Lock(true);
    }
}
