using UnityEngine;

/// <summary>
/// Modifies a transform's position and rotation to maintain a constant offset with a target transform.
/// Useful for syncing the position/rotation of two objects which are siblings within the hierarchy.
/// </summary>
public class VRWC_FollowTransform : MonoBehaviour
{
    [Tooltip("Transform of the rigidbody to follow.")]
    public Transform target;
    public Vector3 extraVector = new (0, 0, 0);

    public Vector3 OffsetPosition
    {
        get { return offsetPosition; }
        set { offsetPosition = value; }
    }
    Vector3 offsetPosition;

    void Start()
    {
        offsetPosition = target.localPosition;
    }

    void Update()
    {
        Vector3 rotatedOffset = target.localRotation * offsetPosition;
        transform.localPosition = target.localPosition + rotatedOffset + extraVector;
        transform.rotation = target.rotation;
    }



}
