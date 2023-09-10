using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : MonoBehaviour
{
    [SerializeReference] Animator anim;
    [SerializeReference] GameObject target;
    [SerializeReference] EnemyData enemyData;
    [SerializeReference] ParticleSystem hitFX;

    [HideInInspector] public bool isWaitingForOtherEnemies = true;
    bool mayAttack = true;
    bool isOutOfReach = false;

    // Default EnemyData
    int maxHits;
    int currentHits;
    float movementSpeed;
    float moveAnimSpeed;
    float attackRange;
    float turnSpeed;

    Vector3 lastPosition = Vector3.zero;
    int attackAnimationLoops = -1;

    // Magic numbers
    const string chaseTrigger = "chase";
    const string attackTrigger = "attack";
    const string animMovementSpeed = "movementSpeed";
    const string waitingToMove = "waitingToMove";
    const string attackState = "attacking";
    const float attackRangeIncrease = 0.5f;
    const float checkWaitRate = 0.1f;
    const float chaseTurnSpeedMultiplier = 3;
    const float attackAnimationImpactTime = 0.28f;

    private void OnEnable()
    {
        EnemyData.onRefreshEnemyData += RefreshEnemyData;
    }

    private void OnDisable()
    {
        EnemyData.onRefreshEnemyData -= RefreshEnemyData;
    }

    void RefreshEnemyData()
    {
        maxHits = enemyData.maxHits;
        movementSpeed = enemyData.movementSpeed;
        moveAnimSpeed = enemyData.moveAnimSpeed;
        attackRange = enemyData.attackRange;
        turnSpeed = enemyData.turnSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        RefreshEnemyData();

        // Check whether or not other enemies are against the same target at the same time
        InvokeRepeating(nameof(WaitForOtherEnemies), 0, checkWaitRate);
    }

    private void WaitForOtherEnemies()
    {
        // Get enemies closest to the same target
        float closestDistance = Mathf.Infinity;
        foreach (Enemy enemy in FindObjectsOfType<Enemy>().Where(e => e.target == target))
        {
            if (Vector3.Distance(target.transform.position, enemy.transform.position) < closestDistance)
            {
                closestDistance = Vector3.Distance(target.transform.position, enemy.transform.position);
                foreach (Enemy enemy2 in FindObjectsOfType<Enemy>().Where(e => e.target == target))
                {
                    enemy2.isWaitingForOtherEnemies = true;
                }
                enemy.isWaitingForOtherEnemies = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Behaviour
        if (target && Vector3.Distance(transform.position, target.transform.position) >= attackRange)
        {
            Chase();
        }
        else
        {
            // TODO: Check what the target is, then attack or interact with it
            // depending on what it is (presumably using tags)
            Attack();
        }
    }

    private void Chase()
    {
        Debug.DrawLine(transform.position, target.transform.position, Color.yellow);

        if (anim.GetCurrentAnimatorStateInfo(0).IsName(attackState))
        {
            anim.SetBool(waitingToMove, true);
            isOutOfReach = true;
            return;
        }

        Vector3 direction = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * chaseTurnSpeedMultiplier * Time.deltaTime);

        if (!isWaitingForOtherEnemies)
        {
            lastPosition = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, movementSpeed * Time.deltaTime);

            attackRange = enemyData.attackRange;

            anim.SetFloat(animMovementSpeed, moveAnimSpeed);
        }
        else // Wait for other enemies to "finish the job"
        {
            anim.SetFloat(animMovementSpeed, 0);
        }

        anim.SetTrigger(chaseTrigger);
        attackAnimationLoops = -1;
    }

    private void Attack()
    {
        Vector3 direction = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        Debug.DrawRay(transform.position, direction, Color.red);

        attackRange = enemyData.attackRange + attackRangeIncrease;

        anim.SetTrigger(attackTrigger);
        anim.SetBool(waitingToMove, false);

        // Triggers at a specific key frame in the attack animation
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime - attackAnimationLoops >= attackAnimationImpactTime && mayAttack && anim.GetCurrentAnimatorStateInfo(0).IsName(attackState))
        {
            mayAttack = false;

            Ray ray = new(new Vector3(transform.position.x, target.transform.position.y, transform.position.z), transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit) && !isOutOfReach)
            {
                ParticleSystem newHitFX = Instantiate(hitFX, hit.transform.position, Quaternion.identity, transform);
                Destroy(newHitFX.gameObject, newHitFX.main.duration * 2);

                // TODO: If target is within said range, damage it and/or all non-Enemy objects
            }

        }
        else if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime - attackAnimationLoops >= 1)
        {
            mayAttack = true;
            attackAnimationLoops++;
        }

        isOutOfReach = false;
    }
}
