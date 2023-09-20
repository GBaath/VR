using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomModule : MonoBehaviour
{
    public GameObject secretDoor;
    [SerializeField]List<GameObject> propSets;
    public void AddPropsToBase(RoomBase baseConnection)
    {
        if(propSets.Count > 0)
        {
            //randomize propset & send
            int i = Random.Range(0, propSets.Count);
            propSets[i].SetActive(true);
            for (int j = 0; j < propSets[i].transform.childCount; j++)
            {
                baseConnection.props.Add(propSets[i].transform.GetChild(j).gameObject);
            }
        }
        if(secretDoor != null)
        {
            int i = Random.Range(0, baseConnection.switchSpawnPoints.Count);
            baseConnection.switchSpawnPoints[i].SetActive(true);
            baseConnection.switchSpawnPoints[i].GetComponent<SecretSwitch>().moduleConnections.Add(this);
        }

    }
}
