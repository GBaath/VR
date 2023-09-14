using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : MonoBehaviour
{
    [SerializeReference] Animator anim;
    public GameObject target;
    [SerializeReference] EnemyData enemyData;
    [SerializeReference] ParticleSystem doHitFX;
    [SerializeReference] ParticleSystem getHitFX;

    [SerializeReference] GameObject hips;
    [SerializeReference] GameObject head;
    [SerializeReference] GameObject[] legs = new GameObject[2];

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
    [HideInInspector] public bool isWaitingForOtherEnemies = true;

    /// <returns>A random limb from body.</returns>
    public GameObject GetRandomLimb()
    {
        if (!hips) { Debug.LogError("can't find thicc hips"); return null; }

        List<GameObject> allLimbs = new();
        foreach (Transform childTransform in hips.GetComponentInChildren<Transform>())
        {
            allLimbs.Add(childTransform.gameObject);
        }
        if (allLimbs.Count > 0)
        {
            return allLimbs[Random.Range(0, allLimbs.Count)];
        }
        else
        {
            Debug.Log("can't return null limbs");
            return gameObject;
        }
    }

    bool mayAttack = true;
    bool isOutOfReach = false;
    bool isDead = false;

    bool isDismembered = false;
    bool iLostUpperBody = false;
    bool iOnlyHaveUpperBody = false;
    bool iLostLeg = false;
    bool isLaunched = false;

    // Default EnemyData
    int maxHits;
    int currentHits;
    float movementSpeed;
    float moveAnimSpeed;
    float attackRange;
    float turnSpeed;
    bool allowCustomDismemberment;

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
    const float onlyTorsoAngle = 10f;
    const float lostLegsAngle = 1f;

    private void OnEnable()
    {
        EnemyData.onRefreshEnemyData += RefreshEnemyData;
        DismemberBullet.onHitEnemy += OnHit;
        // TODO: Bullet.onHitEnemy += DismemberBody;
    }

    private void OnDisable()
    {
        EnemyData.onRefreshEnemyData -= RefreshEnemyData;
        DismemberBullet.onHitEnemy -= OnHit;
        // TODO: Bullet.onHitEnemy -= DismemberBody;
    }

    void OnHit(GameObject bodyHit, GameObject bullet)
    {
        if (isDead) { return; }

        if (!allowCustomDismemberment)
        {
            RagdollSetActive(true, bodyHit, bullet);
            return;
        }

        if (!isDismembered)
        {
            DismemberBody(bodyHit, bullet);
        }
        else
        {
            KillThisEnemy();
        }
    }

    public void RagdollSetActive(bool enable, GameObject impactedLimb = null, GameObject impactProjectile = null)
    {
        GetComponent<BoxCollider>().enabled = !enable;
        anim.enabled = !enable;
        isDead = enable;

        // Get all colliders that also have character joint components
        foreach (CharacterJoint joint in GetComponentsInChildren<CharacterJoint>().Where(j => j.GetComponent<Collider>()))
        {
            joint.enableCollision = enable;
            joint.GetComponent<Collider>().enabled = enable;
            joint.GetComponent<Rigidbody>().isKinematic = !enable;
            joint.GetComponent<Rigidbody>().detectCollisions = enable;
            joint.GetComponent<Rigidbody>().useGravity = enable;

            //if (enable && joint.gameObject == impactedLimb)
            //{
            //    gameObject.GetComponent<Rigidbody>().AddExplosionForce(1000, joint.transform.position, 10);
            //}
        }
    }

    void DismemberBody(GameObject limbHit, GameObject bullet)
    {
        if (limbHit.transform.GetComponentInParent<Enemy>() != this && !head && legs == null) { return; }

        // "Remove" limb
        limbHit.transform.localScale = Vector3.zero;

        iLostLeg = false;
        foreach (GameObject leg in legs)
        {
            foreach (Transform limb in limbHit.GetComponentsInChildren<Transform>().Where(l => l == leg.transform))
            {
                iLostLeg = true;
            }
        }

        iLostUpperBody = false;
        foreach (Transform limb in head.GetComponentsInParent<Transform>().Where(l => l == limbHit.transform))
        {
            iLostUpperBody = true;
        }

        bool iLostHead = false;
        foreach (Transform limb in limbHit.GetComponentsInChildren<Transform>().Where(b => b == head.transform))
        {
            iLostHead = true;
        }

        // Spawn enemy without all other parts (this essentially spawns a body part)
        GameObject newBody = Instantiate(gameObject, transform.position, Quaternion.identity);
        GameObject impactedLimb = null;
        List<GameObject> relatedTransforms = new();

        // Find the dismembered body part
        foreach (Transform limbTransform in newBody.GetComponentsInChildren<Transform>().Where(b => b.transform.localScale == Vector3.zero))
        {
            limbTransform.localScale = Vector3.one;
            impactedLimb = limbTransform.gameObject;
            impactedLimb.transform.SetParent(newBody.transform);

            relatedTransforms.Add(impactedLimb);
            relatedTransforms.Add(newBody);
        }

        // Keep the transforms of related body parts
        foreach (Transform limbChildTransform in impactedLimb.GetComponentsInChildren<Transform>().Where(b => b.CompareTag(tag)))
        {
            relatedTransforms.Add(limbChildTransform.gameObject);
        }

        // Set hip transform to the dismembered body part
        Transform hipTransform = newBody.transform.GetChild(1);
        hipTransform.SetParent(impactedLimb.transform);
        hipTransform.position = impactedLimb.transform.position;

        // "Remove" unrelated body parts
        foreach (Transform limbTransform in newBody.GetComponentsInChildren<Transform>().Where(b => !relatedTransforms.Contains(b.gameObject)))
        {
            limbTransform.localScale = Vector3.zero;
            limbTransform.position = hipTransform.position;
        }

        if (iLostUpperBody) // The dismembered body will be the new enemy
        {
            if (iLostHead)
                newBody.GetComponent<Enemy>().KillThisEnemy();

            if (iLostLeg)
                newBody.GetComponent<Enemy>().anim.SetBool("isDismembered", true);

            newBody.GetComponent<Enemy>().iOnlyHaveUpperBody = true;

            //RagdollSetActive(true);
            //GetComponentInChildren<SkinnedMeshRenderer>().gameObject.AddComponent<HighlightGrab>();
            Destroy(this);
        }
        else // This will remain as enemy
        {
            if (iLostLeg)
                anim.SetBool("isDismembered", true);

            //newBody.GetComponent<Enemy>().RagdollSetActive(true);
            //newBody.GetComponentInChildren<SkinnedMeshRenderer>().gameObject.AddComponent<HighlightGrab>();
            Destroy(newBody.GetComponent<Enemy>());
        }

        newBody.GetComponent<Enemy>().isDismembered = true;
        isDismembered = true;

        ParticleSystem newLimbFX = Instantiate(getHitFX, newBody.transform.position, Quaternion.identity);
        Destroy(newLimbFX, newLimbFX.main.duration * 2);

        newBody.GetComponent<Rigidbody>().AddForceAtPosition(bullet.GetComponent<Rigidbody>().velocity, limbHit.transform.position, ForceMode.Impulse);
    }

    public void KillThisEnemy()
    {
        isDead = true;
        // TODO: Destroy gameObject and trigger any onEnemyKilled events
    }

    // Start is called before the first frame update
    void Start()
    {
        RefreshEnemyData();

        RagdollSetActive(false);

        // Check whether or not other enemies are against the same target at the same time
        InvokeRepeating(nameof(WaitForOtherEnemies), 0, checkWaitRate);

        // Dance or clap until any enemy of the same type finds the target
        KillTime();
    }

    void RefreshEnemyData()
    {
        maxHits = enemyData.maxHits;
        movementSpeed = enemyData.movementSpeed;
        moveAnimSpeed = enemyData.moveAnimSpeed;
        attackRange = enemyData.attackRange;
        turnSpeed = enemyData.turnSpeed;
        allowCustomDismemberment = enemyData.allowCustomDismemberment;
    }

    void WaitForOtherEnemies()
    {
        if (isDead) { return; }

        if (!target)
        {
            target = Camera.main.gameObject;
        }

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

    void KillTime()
    {
        if (!isDancer && mayKillTime || isDead) { return; }
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
        if (!target || isDead) { return; }

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

    void Chase()
    {
        Debug.DrawLine(transform.position, target.transform.position, Color.yellow);

        if (anim.GetCurrentAnimatorStateInfo(0).IsName(attackState))
        {
            anim.SetBool(waitingToMove, true);
            isOutOfReach = true;
            return;
        }

        if (isDismembered && iOnlyHaveUpperBody)
        {
            transform.LookAt(new Vector3(target.transform.position.x, transform.position.y - onlyTorsoAngle, target.transform.position.z));
        }
        else if (isDismembered && iLostLeg)
        {
            transform.LookAt(new Vector3(target.transform.position.x, transform.position.y - lostLegsAngle, target.transform.position.z));
        }
        else
        {
            Vector3 direction = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * chaseTurnSpeedMultiplier * Time.deltaTime);
        }

        if (!isWaitingForOtherEnemies)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, movementSpeed * Time.deltaTime);

            if (isDismembered && iOnlyHaveUpperBody)
            {
                attackRange = enemyData.attackRange + attackRangeIncrease;
            }
            else
            {
                attackRange = enemyData.attackRange;
            }

            anim.SetFloat(animMovementSpeed, moveAnimSpeed);
        }
        else // Wait for other enemies to "finish the job"
        {
            anim.SetFloat(animMovementSpeed, 0);
        }

        anim.SetTrigger(chaseTrigger);
        attackAnimationLoops = -1;
    }

    void Attack()
    {
        if (isDismembered && iOnlyHaveUpperBody)
        {
            // Launch at target, then die on impact with anything
            transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, movementSpeed * Time.deltaTime * 5);
            isLaunched = true;
            return;
        }
        else
        {
            Vector3 direction = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            Debug.DrawRay(transform.position, direction, Color.red);
        }

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
                ParticleSystem newHitFX = Instantiate(doHitFX, hit.transform.position, Quaternion.identity, transform);
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

    private void OnCollisionEnter(Collision other)
    {
        if (isLaunched && other.gameObject.layer == LayerMask.NameToLayer("Wheelchair"))
        {
            Destroy(gameObject);
            // TODO: Damage target
        }
    }
}
