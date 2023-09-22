using UnityEngine;

/// <summary>
/// Modifies a transform's position and rotation to maintain a constant offset with a target transform.
/// Useful for syncing the position/rotation of two objects which are siblings within the hierarchy.
/// </summary>
public class VRWC_FollowTransform : MonoBehaviour
{
    [Tooltip("Transform of the rigidbody to follow.")]
    public Transform targetFrame;
    Vector3 offset;

    void Start()
    {
        offset = transform.localPosition - targetFrame.localPosition;
    }

    void Update()
    {
        Vector3 rotatedOffset = targetFrame.localRotation * offset;
        transform.localPosition = targetFrame.localPosition + rotatedOffset;

        transform.rotation = targetFrame.rotation;
    }

    public void SetOffset(Vector3 newPosition)
    {
        offset = newPosition;
    }
}
