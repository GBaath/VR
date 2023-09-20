using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public int maxHealth = 5;
    public float movementSpeed = 0.75f;
    public float turnSpeed = 25;
    public float attackAnimSpeed = 1.5f;

    public AnimationClip idleAnimation, confusedAnimation, surprisedAnimation, chaseAnimation, attackAnimation, danceAnimation, cheerAnimation;

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
