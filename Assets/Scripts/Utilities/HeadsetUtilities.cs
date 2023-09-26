using UnityEngine;
using UnityEngine.InputSystem;

public class HeadsetUtilities : MonoBehaviour
{
    public Transform tempTransform;

    public VRWC_FollowTransform cameraOffset;
    public InputActionReference xButton;
    public InputActionReference yButton;

    private void OnEnable()
    {
        xButton.action.Enable();
        yButton.action.Enable();
    }

    private void OnDisable()
    {
        xButton.action.Disable();
        yButton.action.Disable();
    }

    void Update()
    {
        if (xButton.action.triggered && yButton.action.IsPressed() || xButton.action.IsPressed() && yButton.action.triggered || Input.GetKeyDown(KeyCode.Space))
        {
            ResetHeadsetPosition(cameraOffset.transform.localPosition);
        }
    }

    public void ResetHeadsetPosition(Vector3 newPosition)
    {
        cameraOffset.OffsetPosition = newPosition;
        Debug.Log("camera offset set at " + newPosition + "!");
    }
}
