using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour
{
    public GameObject destructionPrefab;
    public bool canBeShot;
    private GameObject destructableObject;

    public void DestructableDie()
    {
            destructableObject = Instantiate(destructionPrefab, transform.position, transform.rotation);
            destructableObject.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
            Destroy(this.gameObject);     
    }

}
