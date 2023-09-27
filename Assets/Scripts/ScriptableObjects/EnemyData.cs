using UnityEngine;

[CreateAssetMenu(menuName = "New EnemyData", fileName = "EnemyData")]
public class EnemyData : ScriptableObject {
    [Header("ENEMY DATA")]
    [SerializeField] float baseScale = 1;
    public float BaseScale {
        get { return baseScale; }
    }
    [SerializeField] float minScale = 0.6f, maxScale = 1.4f;
    public float MinScale {
        get { return minScale; }
    }
    public float MaxScale {
        get { return maxScale; }
    }
    [SerializeField] int maxHealth = 5;
    public int MaxHealth {
        get { return maxHealth; }
    }
    [SerializeField] float turnSpeed = 25f;
    public float TurnSpeed {
        get { return turnSpeed; }
    }
    [SerializeField] int attackDamage = 50;
    public int AttackDamage {
        get { return attackDamage; }
    }
    [SerializeField] float movementSpeed = 1f;
    public float MovementSpeed {
        get { return movementSpeed; }
    }
    [SerializeField] float attackSpeedMultiplier = 1.5f;
    public float AttackSpeedMultiplier {
        get { return attackSpeedMultiplier; }
    }

    [Header("ANIMATION DATA")]
    [Tooltip("Goes from 0 to 1, and acts as progress from start to end of animation.")]
    public string animProgress = "animProgress";

    [Space]
    public AnimationClip idleAnimation;
    public AnimationClip chaseAnimation,
        cheerAnimation,
        danceAnimation,
        attackAnimation,
        confusedAnimation,
        surprisedAnimation;

    [Space]
    public string idleTrigger = "idle";
    public string chaseTrigger = "chase",
        cheerTrigger = "cheer",
        danceTrigger = "dance",
        attackTrigger = "attack",
        confuseTrigger = "confuse",
        surpriseTrigger = "surprise";

    public delegate void OnRefreshEnemyData();
    public static OnRefreshEnemyData onRefreshEnemyData;

    public void RefreshEnemyData() {
        if (Application.isPlaying) {
            onRefreshEnemyData();
        }
        else {
            Debug.Log("Can't refresh enemy data while in editor, nor does it need to");
        }
    }
}
