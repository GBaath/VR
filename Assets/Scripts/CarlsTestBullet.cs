using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarlsTestBullet : MonoBehaviour
{
    public delegate void OnHitEnemy(GameObject hitBodypart, GameObject bullet);
    public static OnHitEnemy onHitEnemy;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * 5;
    }

    private void OnCollisionEnter(Collision other)
    {
        onHitEnemy(other.collider.gameObject, gameObject);
        Destroy(gameObject);
    }
}
