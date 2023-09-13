using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraOffsetTransform : MonoBehaviour
{
    public GameObject wheelChair;

    private void Start()
    {
        StartCoroutine(SetCameraOffset(1));
    }

    IEnumerator SetCameraOffset(float t)
    {
        yield return new WaitForSeconds(t);
        gameObject.transform.position = wheelChair.transform.position;
    }
}
