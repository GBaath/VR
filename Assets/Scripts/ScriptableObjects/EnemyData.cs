using UnityEngine;

[CreateAssetMenu(menuName = "New EnemyData", fileName = "EnemyData")]
public class EnemyData : ScriptableObject {

    [Header("ENEMY DATA")]
    [SerializeField] EnemyType enemyType;
    public EnemyType TypeOfEnemy {
        get { return enemyType; }
    }
    public enum EnemyType {
        skeleton,
        goblin,
        ghost
    }
    [SerializeField] float startSize = 1;
    public float StartSize {
        get { return startSize; }
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
    [SerializeField] float animationSpeedMultiplier = 1f;
    public float AnimationSpeedMultiplier {
        get { return animationSpeedMultiplier; }
    }

    [Header("ANIMATION DATA")]
    [Tooltip("The speed of animations and transitions. Goes from 0 to 1 after 1s. Value cannot be changed outside the EnemyState StateMachine.")]
    public string animSpeed = "animSpeed";

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
