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
    public RoomEntrance entrance;
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

        //rotation transform magic
        connectionsPoints[r].transform.position = entrance.transform.position;
        connectionsPoints[r].transform.rotation = Quaternion.identity;
    }
    public void SpawnConnections()
    {

        //spawn exit first
        //get random free point
        int index = freeIndexes[Random.Range(0, freeIndexes.Count)];


        //spawning modules sets connection point as used
        connectionsPoints[index].SpawnExitModule(out entrance.nextRoomEntrance);

        //spawn new module on free space from roommanager
        foreach (ConnectionsPoint c in connectionsPoints.Where(cp => !cp.GetComponent<ConnectionsPoint>().isUsed))
        {
            c.SpawnRandomModule();
        }
    }
}
