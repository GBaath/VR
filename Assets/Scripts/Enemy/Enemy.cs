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

    [SerializeReference] GameObject head;

    [Header("Dancing")]
    public EnemyType enemyType = EnemyType.skeleton;
    public enum EnemyType
    {
        skeleton,
        goblin,
        ghost
    }

    public bool isDancer = false;
    [SerializeField] float maxDanceDistance = 10;

    [HideInInspector] public bool mayKillTime = true;
    public bool isWaitingForOtherEnemies = true;
    bool mayAttack = true;
    bool isOutOfReach = false;
    bool isDismembered = false;

    // Default EnemyData
    int maxHits;
    int currentHits;
    float movementSpeed;
    float moveAnimSpeed;
    float attackRange;
    float turnSpeed;
    bool allowDismemberment;

    Vector3 lastPosition = Vector3.zero;
    int attackAnimationLoops = -1;

    // Magic numbers
    const string chaseTrigger = "chase";
    const string attackTrigger = "attack";
    const string danceTrigger = "dance";
    const string cheerTrigger = "cheer";
    const string attackState = "attacking";
    const string danceState = "swing_dance";
    const string animMovementSpeed = "animMoveSpeed";
    const string waitingToMove = "waitingToMove";
    const float attackRangeIncrease = 0.5f;
    const float checkWaitRate = 0.1f;
    const float chaseTurnSpeedMultiplier = 3;
    const float attackAnimationImpactTime = 0.28f;

    private void OnEnable()
    {
        EnemyData.onRefreshEnemyData += RefreshEnemyData;
        CarlsTestBullet.onHitEnemy += DismemberBody;
        // TODO: Bullet.onHitEnemy += DismemberBody;
    }

    private void OnDisable()
    {
        EnemyData.onRefreshEnemyData -= RefreshEnemyData;
        CarlsTestBullet.onHitEnemy -= DismemberBody;
        // TODO: Bullet.onHitEnemy -= DismemberBody;
    }

    void RefreshEnemyData()
    {
        maxHits = enemyData.maxHits;
        movementSpeed = enemyData.movementSpeed;
        moveAnimSpeed = enemyData.moveAnimSpeed;
        attackRange = enemyData.attackRange;
        turnSpeed = enemyData.turnSpeed;
        allowDismemberment = enemyData.allowDismemberment;
    }

    void DismemberBody(GameObject body, GameObject bullet)
    {
        if (body.transform.GetComponentInParent<Enemy>() != this || !allowDismemberment) { return; }

        // "Remove" body part
        body.transform.localScale = Vector3.zero;

        bool lostHead = false;
        foreach (Transform bodyPart in body.GetComponentsInChildren<Transform>().Where(b => b == head.transform))
        {
            lostHead = true;
        }

        // Spawn enemy without all other parts (this essentially spawns a body part)
        GameObject newBody = Instantiate(gameObject, transform.position, Quaternion.identity);
        GameObject impactedBodyPart = null;
        List<GameObject> relatedTransforms = new();

        // Find the dismembered body part
        foreach (Transform bodyPartTransform in newBody.GetComponentsInChildren<Transform>().Where(b => b.transform.localScale == Vector3.zero))
        {
            bodyPartTransform.localScale = Vector3.one;
            impactedBodyPart = bodyPartTransform.gameObject;
            impactedBodyPart.transform.SetParent(newBody.transform);

            relatedTransforms.Add(impactedBodyPart);
            relatedTransforms.Add(newBody);
        }

        // Keep the transforms of related body parts
        foreach (Transform bodyPartChildTransform in impactedBodyPart.GetComponentsInChildren<Transform>().Where(b => b.CompareTag(tag)))
        {
            relatedTransforms.Add(bodyPartChildTransform.gameObject);
        }

        // Set hip transform to the dismembered body part
        Transform hip = newBody.transform.GetChild(1);
        hip.transform.SetParent(impactedBodyPart.transform);
        hip.transform.position = impactedBodyPart.transform.position;

        // "Remove" unrelated body parts
        foreach (Transform bodyPartTransform in newBody.GetComponentsInChildren<Transform>().Where(b => !relatedTransforms.Contains(b.gameObject)))
        {
            bodyPartTransform.localScale = Vector3.zero;
        }

        // Remove all logic (besides physics) from the body parts or the remains of this
        if (lostHead)
        {
            newBody.GetComponent<Enemy>().anim.SetBool("isDismembered", true);
            anim.enabled = false;
            enabled = false;
            Destroy(gameObject.GetComponent<Enemy>());
        }
        else
        {
            newBody.GetComponent<Enemy>().enabled = false;
            newBody.GetComponent<Animator>().enabled = false;
            Destroy(newBody.GetComponent<Enemy>());

            anim.SetBool("isDismembered", true);
        }

        newBody.GetComponent<Enemy>().isDismembered = true;
        isDismembered = true;

        Rigidbody newRigidbody = newBody.GetComponent<Rigidbody>();
        newRigidbody.AddForceAtPosition(bullet.GetComponent<Rigidbody>().velocity, body.transform.position, ForceMode.Impulse);
    }

    // Start is called before the first frame update
    void Start()
    {
        RefreshEnemyData();

        // Check whether or not other enemies are against the same target at the same time
        InvokeRepeating(nameof(WaitForOtherEnemies), 0, checkWaitRate);

        // Dance or clap until any enemy of the same type finds the target
        KillTime();
    }

    private void WaitForOtherEnemies()
    {
        // Get enemies closest to the same target
        float closestDistance = Mathf.Infinity;
        foreach (Enemy enemy in FindObjectsOfType<Enemy>().Where(e => e.target == target))
        {
            // If I'm closest to my target
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

    private void KillTime()
    {
        if (!isDancer && mayKillTime) { return; }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(danceState)) { return; }

        List<Enemy> cheeringEnemies = new();
        foreach (Enemy enemy in FindObjectsOfType<Enemy>().Where(e => e.enemyType == enemyType && e.mayKillTime && !e.isDancer))
        {
            cheeringEnemies.Add(enemy);
        }

        if (cheeringEnemies.Count < 3) { return; }

        anim.SetTrigger(danceTrigger);

        foreach (Enemy cheeringEnemy in cheeringEnemies)
        {
            cheeringEnemy.transform.LookAt(transform.position);
            cheeringEnemy.anim.SetTrigger(cheerTrigger);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mayKillTime)
        {
            if (isDancer)
            {
                // Check the distance between target and dancer
                if (Vector3.Distance(transform.position, target.transform.position) < maxDanceDistance)
                {
                    foreach (Enemy enemy in FindObjectsOfType<Enemy>().Where(e => e.enemyType == enemyType))
                    {
                        enemy.mayKillTime = false;
                    }
                }
                else
                {
                    foreach (Enemy enemy in FindObjectsOfType<Enemy>().Where(e => e.enemyType == enemyType))
                    {
                        enemy.mayKillTime = true;
                        enemy.KillTime();
                    }
                }
            }
            // WOOHOO, LET'S KILL TIME!!!
            return;
        }
        else if (isDancer)
        {
            // Check the distance between target and dancer
            if (Vector3.Distance(transform.position, target.transform.position) >= maxDanceDistance)
            {
                foreach (Enemy enemy in FindObjectsOfType<Enemy>().Where(e => e.enemyType == enemyType))
                {
                    enemy.mayKillTime = true;
                    enemy.KillTime();
                }
            }
        }

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
