using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretSwitch : MonoBehaviour
{
    public List<RoomModule> moduleConnections;


    public void Open()
    {
        foreach (RoomModule module in moduleConnections)
        {
            module.secretDoor.SetActive(false);
        }
        //sound here
    }
    
}
