using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPosition : MonoBehaviour
{
   public GameObject wheelChair;
    private void Start()
    {
        gameObject.transform.position = wheelChair.transform.position;
    }
}
