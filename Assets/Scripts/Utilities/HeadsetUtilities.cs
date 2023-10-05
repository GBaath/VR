using UnityEngine;
using UnityEngine.InputSystem;

public class HeadsetUtilities : MonoBehaviour
{


    [SerializeField] Transform xrCamera;
    [SerializeField] Transform cameraOffset;
    [SerializeField] Transform resetTarget;
    [SerializeField] VRWC_FollowTransform followtransform;

    private float angle;
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
        if (xButton.action.triggered && yButton.action.IsPressed() || xButton.action.IsPressed() && yButton.action.triggered)
        {
            ResetPosition();
        }

    }
    public void ResetPosition()
    {
        //followtransform.offsetRotation = Quaternion.Euler(0, -xrCamera.localEulerAngles.y, 0);//Quaternion.Inverse(Quaternion.FromToRotation(xrCamera.eulerAngles,resetTarget.eulerAngles));
        Vector3 desiredCameraPosition = resetTarget.position;
        Vector3 currentHeadPositionRelativeToOffset = cameraOffset.InverseTransformPoint(xrCamera.position);
        Vector3 adjustedPosition = desiredCameraPosition - currentHeadPositionRelativeToOffset;
        cameraOffset.position = adjustedPosition;
        followtransform.OffsetPosition = cameraOffset.position - followtransform.target.position;


    }




    //public void ResetPosition()
    //{
    //    // Calculate the rotated offset at the current frame.
    //    Vector3 rotatedOffset = followtransform.target.localRotation * followtransform.OffsetPosition;

    //    // Calculate the desired offset to align cameraOffset to target's position.
    //    Vector3 newExtraVector = resetTarget.localPosition - cameraOffset.localPosition - rotatedOffset;


    //    // Retain the Y value from the current extraVector
    //    newExtraVector.y = followtransform.extraVector.y;

    //    //followtransformtwo.extraVector = newExtraVector;
    //    followtransform.extraVector = newExtraVector;
    //}


}
