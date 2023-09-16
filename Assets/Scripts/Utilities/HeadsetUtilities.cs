using UnityEngine;

public class HeadsetUtilities : MonoBehaviour
{
    private void Start()
    {
        ResetHeadsetPosition();
    }

    public void ResetHeadsetPosition()
    {
        gameObject.transform.position = Camera.main.transform.position;
    }
}
