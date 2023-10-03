using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView))]
public class FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;

        Handles.color = Color.white;
        Handles.DrawWireArc(fov.viewObject.transform.position, Vector3.up, Vector3.forward, 360, fov.seeRadius + fov.currentSeeRadiusIncrease);

        Handles.color = Color.red;
        Handles.DrawWireArc(fov.viewObject.transform.position, Vector3.up, Vector3.forward, 360, fov.attackRadius + fov.currentAttackRadiusIncrease);

        Vector3 viewAngle01 = DirectionFromAngle(fov.viewObject.transform.eulerAngles.y, -fov.seeAngle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.viewObject.transform.eulerAngles.y, fov.seeAngle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.viewObject.transform.position, fov.viewObject.transform.position + viewAngle01 * fov.seeRadius);
        Handles.DrawLine(fov.viewObject.transform.position, fov.viewObject.transform.position + viewAngle02 * fov.seeRadius);

        if (fov.canSeeTarget && fov.target)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.viewObject.transform.position, fov.target.transform.position);
        }
    }

    Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
