using UnityEngine;

[CreateAssetMenu(fileName = "NewDebugger", menuName = "Debugger")]
public class Debugger : ScriptableObject
{
    //public void DebugMessage()
    //{
    //    Debug.Log(message);
    //}

    public void DebugMessage(int message)
    {
        Debug.Log(message);
    }

    public void DebugMessage(string message = "empty message")
    {
        Debug.Log(message);
    }
}
