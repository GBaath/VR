using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public int maxHits = 2;
    public float movementSpeed = 0.75f;
    public float moveAnimSpeed = 0.75f;
    public float attackRange = 2;
    public float turnSpeed = 25;
    public bool allowDismemberment = true;

    public delegate void OnRefreshEnemyData();
    public static OnRefreshEnemyData onRefreshEnemyData;

    public void RefreshEnemyData()
    {
        if (Application.isPlaying)
            onRefreshEnemyData();
        else
        {
            Debug.Log("Can't refresh enemy data while in editor, nor does it need to");
        }
    }
}
