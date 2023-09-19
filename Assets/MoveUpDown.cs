using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpDown : MonoBehaviour
{
    public float speed = 5f; // The speed of the movement
    public float distance = 2f; // The maximum distance the object can move up and down

    private float initialPositionY; // Initial Y position of the object
    private float newPositionY; // New Y position of the object to which it will move

    // Start is called before the first frame update
    void Start()
    {
        initialPositionY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        newPositionY = initialPositionY + Mathf.Sin(Time.time * speed) * distance;
        transform.position = new Vector3(transform.position.x, newPositionY, transform.position.z);
    }
}
