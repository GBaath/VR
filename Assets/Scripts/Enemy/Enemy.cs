using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : MonoBehaviour, IDamageable
{
    // Cached references
    public Animator animator;
    public GameObject target;
    public EnemyData enemyData;
    public FieldOfView fov;
    [SerializeReference] ParticleSystem doHitFX;
    [SerializeReference] ParticleSystem getHitFX;
    [SerializeReference] AudioSource audioSource;

    [SerializeReference] GameObject hips;
    [SerializeReference] GameObject head;
    [SerializeReference] GameObject[] legs = new GameObject[2];

    public EnemyType enemyType = EnemyType.skeleton;
    public enum EnemyType
    {
        skeleton,
        goblin,
        ghost
    }

    [Header("Animation Variables")]
    [Tooltip("Displays the current state this enemy is in. State cannot be changed outside the EnemyState StateMachine.")]
    public string currentState = "idle";
    
    [Space, Tooltip("The speed of animations and transitions. Goes from 0 to 1 after 1s. Value cannot be changed outside the EnemyState StateMachine.")]
    public string animSpeed = "animSpeed";

    [Space]
    public string idleTrigger = "idle";
    public string chaseTrigger = "chase",
        cheerTrigger = "cheer",
        danceTrigger = "dance",
        attackTrigger = "attack",
        confuseTrigger = "confuse",
        surpriseTrigger = "stumble";

    // Public variables
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

    // Public non-references
    [HideInInspector] public IEnemyState previousState = new IdleEnemyState();
    [HideInInspector] public IEnemyState state = new IdleEnemyState();
    [HideInInspector] public bool isWaitingForOtherEnemies = true;
    [HideInInspector] public Color decayColor = Color.white;
    [HideInInspector] public float maxDanceDistance = 10;
    [HideInInspector] public bool mayKillTime = true;
    [HideInInspector] public bool canDamage = true;
    [HideInInspector] public bool isDancer = false;
    [HideInInspector] public bool isCaster = false;
    [HideInInspector] public bool isDead = false;
    [HideInInspector] public float animTimer = 0;

    // Magic numbers
    [HideInInspector] public float attackAnimationImpactTime = 0.28f;
    [HideInInspector] public float chaseTurnSpeedMultiplier = 3;
    [HideInInspector] public float attackRangeIncrease = 0.5f;
    [HideInInspector] public int attackAnimationLoops = 0;
    [HideInInspector] public float checkWaitRate = 0.1f;

    // Default EnemyData
    [HideInInspector] public float attackAnimSpeed;
    [HideInInspector] public float movementSpeed;
    [HideInInspector] public float moveAnimSpeed;
    [HideInInspector] public float turnSpeed;

    private void OnEnable()
    {
        EnemyData.onRefreshEnemyData += RefreshEnemyData;
    }

    private void OnDisable()
    {
        EnemyData.onRefreshEnemyData -= RefreshEnemyData;
    }

    void IDamageable.TakeDamage(int amount)
    {
        audioSource.clip = GameManager.instance.audioManager.enemyHit;
        audioSource.Play();
    }

    void IDamageable.Die(float delay)
    {
        RagdollSetActive(true);

        // TODO: Trigger any onEnemyKilled events
        isDead = true;
        isWaitingForOtherEnemies = true;

        state = state.Die(this);

        Destroy(gameObject, delay);
    }

    // Start is called before the first frame update
    private void Start()
    {
        RefreshEnemyData();

        RagdollSetActive(false);

        // Check whether or not other enemies are against the same target at the same time
        InvokeRepeating(nameof(WaitForOtherEnemies), 0, checkWaitRate);

        // Dance or clap until any enemy of the same type finds the target
        //StartDance();

        state = state.Idle(this);
    }

    void RefreshEnemyData()
    {
        movementSpeed = enemyData.movementSpeed;
        turnSpeed = enemyData.turnSpeed;
        attackAnimSpeed = enemyData.attackAnimSpeed;
    }

    public void RagdollSetActive(bool enable)
    {
        GetComponent<BoxCollider>().enabled = !enable;
        animator.enabled = !enable;

        // Get all colliders that also have character joint components
        foreach (CharacterJoint joint in GetComponentsInChildren<CharacterJoint>().Where(j => j.GetComponent<Collider>()))
        {
            joint.enableCollision = enable;
            joint.enablePreprocessing = enable;
            joint.enableProjection = enable;
            joint.GetComponent<Collider>().enabled = enable;
            joint.GetComponent<Rigidbody>().isKinematic = !enable;
            joint.GetComponent<Rigidbody>().detectCollisions = enable;
            joint.GetComponent<Rigidbody>().useGravity = enable;
        }
    }

    void WaitForOtherEnemies()
    {
        if (isDead) { return; }

        if (!target) { target = Camera.main.gameObject; }

        // Get enemies closest to the same target
        float closestDistance = Mathf.Infinity;
        foreach (Enemy enemy in FindObjectsOfType<Enemy>().Where(e => e.target == target && e.fov.canSeeTarget))
        {
            // If I'm closest to my target
            if (Vector3.Distance(target.transform.position, enemy.transform.position) < closestDistance)
            {
                closestDistance = Vector3.Distance(target.transform.position, enemy.transform.position);
                foreach (Enemy enemy2 in FindObjectsOfType<Enemy>().Where(e => e.target == target && e.fov.canSeeTarget))
                {
                    enemy2.isWaitingForOtherEnemies = true;
                }
                enemy.isWaitingForOtherEnemies = false;
            }
        }
    }

    void StartDance()
    {
        if (!isDancer && mayKillTime || isDead) { return; }

        //if (animator.GetCurrentAnimatorStateInfo(0).IsName(danceState)) { return; }

        List<Enemy> cheeringEnemies = new();
        foreach (Enemy enemy in FindObjectsOfType<Enemy>().Where(e => e.enemyType == enemyType && e.mayKillTime && !e.isDancer))
        {
            cheeringEnemies.Add(enemy);
        }

        if (cheeringEnemies.Count < 3) { return; }

        //animator.SetTrigger(danceTrigger);

        //foreach (Enemy cheeringEnemy in cheeringEnemies)
        //{
        //    cheeringEnemy.transform.LookAt(transform.position);
        //    cheeringEnemy.animator.SetTrigger(cheerTrigger);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        currentState = state.ToString();
        state = state.Update(this);
        animTimer += Time.deltaTime;

        if (isDead) { state = state.Die(this); return; }

        if (!target || isWaitingForOtherEnemies || !fov.canSeeTarget) { state = state.Idle(this); return; }

        if (Vector3.Distance(transform.position, target.transform.position) < fov.attackRadius + fov.currentAttackRadiusIncrease)
        {
            state = state.Attack(this); return;
        }
        else
        {
            state = state.Chase(this); return;
        }
    }

    public void TurnTowardsTarget(float turnSpeedMultiplier = 1)
    {
        Vector3 direction = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * turnSpeedMultiplier * Time.deltaTime);
        Debug.DrawRay(transform.position, direction);
    }

    public void Chase()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, movementSpeed * Time.deltaTime * Mathf.Clamp(animTimer, 0, 1));
    }

    public void Attack()
    {
        attackAnimationLoops++;

        Ray ray = new(new Vector3(transform.position.x, target.transform.position.y, transform.position.z), transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            ParticleSystem newHitFX = Instantiate(doHitFX, hit.transform.position, Quaternion.identity, transform);
            Destroy(newHitFX.gameObject, newHitFX.main.duration * 2);

            // TODO: If target is within said range, damage it and/or all non-Enemy objects
        }
    }
    #region Unused
    //void DismemberBody(GameObject limbHit, GameObject bullet)
    //{
    //    if (limbHit.transform.GetComponentInParent<Enemy>() != this && !head && legs == null) { return; }

    //    // "Remove" limb
    //    limbHit.transform.localScale = Vector3.zero;

    //    iLostLeg = false;
    //    foreach (GameObject leg in legs)
    //    {
    //        foreach (Transform limb in limbHit.GetComponentsInChildren<Transform>().Where(l => l == leg.transform))
    //        {
    //            iLostLeg = true;
    //        }
    //    }

    //    iLostUpperBody = false;
    //    foreach (Transform limb in head.GetComponentsInParent<Transform>().Where(l => l == limbHit.transform))
    //    {
    //        iLostUpperBody = true;
    //    }

    //    bool iLostHead = false;
    //    foreach (Transform limb in limbHit.GetComponentsInChildren<Transform>().Where(b => b == head.transform))
    //    {
    //        iLostHead = true;
    //    }

    //    // Spawn enemy without all other parts (this essentially spawns a body part)
    //    GameObject newBody = Instantiate(gameObject, transform.position, Quaternion.identity);
    //    GameObject impactedLimb = null;
    //    List<GameObject> relatedTransforms = new();

    //    // Find the dismembered body part
    //    foreach (Transform limbTransform in newBody.GetComponentsInChildren<Transform>().Where(b => b.transform.localScale == Vector3.zero))
    //    {
    //        limbTransform.localScale = Vector3.one;
    //        impactedLimb = limbTransform.gameObject;
    //        impactedLimb.transform.SetParent(newBody.transform);

    //        relatedTransforms.Add(impactedLimb);
    //        relatedTransforms.Add(newBody);
    //    }

    //    // Keep the transforms of related body parts
    //    foreach (Transform limbChildTransform in impactedLimb.GetComponentsInChildren<Transform>().Where(b => b.CompareTag(tag)))
    //    {
    //        relatedTransforms.Add(limbChildTransform.gameObject);
    //    }

    //    // Set hip transform to the dismembered body part
    //    Transform hipTransform = newBody.transform.GetChild(1);
    //    hipTransform.SetParent(impactedLimb.transform);
    //    hipTransform.position = impactedLimb.transform.position;

    //    // "Remove" unrelated body parts
    //    foreach (Transform limbTransform in newBody.GetComponentsInChildren<Transform>().Where(b => !relatedTransforms.Contains(b.gameObject)))
    //    {
    //        limbTransform.localScale = Vector3.zero;
    //        limbTransform.position = hipTransform.position;
    //    }

    //    if (iLostUpperBody) // The dismembered body will be the new enemy
    //    {
    //        if (iLostHead)
    //            newBody.GetComponent<Enemy>().KillThisEnemy();

    //        if (iLostLeg)
    //            newBody.GetComponent<Enemy>().anim.SetBool("isDismembered", true);

    //        newBody.GetComponent<Enemy>().iOnlyHaveUpperBody = true;

    //        //RagdollSetActive(true);
    //        //GetComponentInChildren<SkinnedMeshRenderer>().gameObject.AddComponent<HighlightGrab>();
    //        Destroy(this);
    //    }
    //    else // This will remain as enemy
    //    {
    //        if (iLostLeg)
    //            anim.SetBool("isDismembered", true);

    //        //newBody.GetComponent<Enemy>().RagdollSetActive(true);
    //        //newBody.GetComponentInChildren<SkinnedMeshRenderer>().gameObject.AddComponent<HighlightGrab>();
    //        Destroy(newBody.GetComponent<Enemy>());
    //    }

    //    newBody.GetComponent<Enemy>().isDismembered = true;
    //    isDismembered = true;

    //    ParticleSystem newLimbFX = Instantiate(getHitFX, newBody.transform.position, Quaternion.identity);
    //    Destroy(newLimbFX, newLimbFX.main.duration * 2);

    //    newBody.GetComponent<Rigidbody>().AddForceAtPosition(bullet.GetComponent<Rigidbody>().velocity, limbHit.transform.position, ForceMode.Impulse);
    //}
    #endregion
}
