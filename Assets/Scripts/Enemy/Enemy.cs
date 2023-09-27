using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable {
    // Cached references
    [SerializeReference] GameObject hips;
    [SerializeReference] Animator animator;
    [SerializeReference] GameObject target;
    [SerializeReference] EnemyData enemyData;
    //[SerializeReference] ParticleSystem doHitFX;
    [SerializeReference] ParticleSystem getHitFX;
    [SerializeReference] FieldOfView fieldOfView;
    [SerializeReference] AudioSource audioSource;

    [Tooltip("Displays the current state this enemy is in. State cannot be changed outside the EnemyState StateMachine.")]
    public string currentState = "IdleEnemyState";

    // Public variables
    public Animator Animator {
        get { return animator; }
    }

    public GameObject Target {
        get {
            if (!target) {
                return Camera.main.gameObject;
            } else {
                return target;
            }
        }
    }

    public EnemyData EnemyData {
        get { return enemyData; }
    }

    public FieldOfView FieldOfView {
        get { return fieldOfView; }
    }

    /// <returns>A random limb from body.</returns>
    public GameObject GetRandomLimb() {
        if (!hips) {
            Debug.LogError("can't find thicc hips");
            return null;
        }

        List<GameObject> allLimbs = new();
        foreach (Transform childTransform in hips.GetComponentInChildren<Transform>()) {
            allLimbs.Add(childTransform.gameObject);
        }
        if (allLimbs.Count > 0) {
            return allLimbs[Random.Range(0, allLimbs.Count)];
        } else {
            Debug.Log("can't return null limbs");
            return gameObject;
        }
    }

    // Public non-references
    [HideInInspector] public IEnemyState previousState = new IdleEnemyState();
    [HideInInspector] public IEnemyState state = new IdleEnemyState();
    [HideInInspector] public bool isWaitingForOtherEnemies = false;
    [HideInInspector] public Color decayColor = Color.white;
    [HideInInspector] public float maxDanceDistance = 10;
    [HideInInspector] public bool mayKillTime = true;
    [HideInInspector] public bool wasSpawned = false;
    [HideInInspector] public float randomPitch = 1;
    [HideInInspector] public bool canDamage = true;
    [HideInInspector] public bool isDancer = false;
    [HideInInspector] public bool isCaster = false;
    [HideInInspector] public bool isDead = false;
    [HideInInspector] public float animTimer = 0;

    // Stats
    [HideInInspector] public float turnSpeed;
    [HideInInspector] public int attackDamage;
    [HideInInspector] public float movementSpeed;
    //[HideInInspector] public float animationSpeed;

    // Magic numbers
    [HideInInspector] public float attackAnimationImpactTime = 0.28f;
    [HideInInspector] public float chaseTurnSpeedMultiplier = 3;
    [HideInInspector] public int attackAnimationLoops = 0;
    [HideInInspector] public float checkWaitRate = 0.1f;
    readonly float movementSpeedVariation = 0.5f;

    void IDamageable.TakeDamage(int amount) {
        audioSource.clip = GameManager.instance.audioManager.enemyHit;
        audioSource.Play();
    }

    void IDamageable.Die(float delay) {
        RagdollSetActive(true);

        // TODO: Trigger any onEnemyKilled events
        isDead = true;
        isWaitingForOtherEnemies = true;

        state = state.Die(this);

        Destroy(gameObject, delay);
    }

    // Start is called before the first frame update
    private void Start() {

        transform.Rotate(new Vector3(transform.rotation.x, Random.Range(0, 360), transform.rotation.z));

        RagdollSetActive(false);

        if (GameManager.instance.enemiesToChaseAtOnce > 0) {
            InvokeRepeating(nameof(WaitForOtherEnemies), 0, checkWaitRate);
        } else {
            isWaitingForOtherEnemies = false;
        }

        RandomizeSizeAndStats();

        // Check whether or not other enemies are against the same target at the same time

        // Dance or clap until any enemy of the same type finds the target
        //StartDance();

        state = state.Idle(this);
    }

    public void RagdollSetActive(bool enable) {
        //GetComponent<BoxCollider>().enabled = !enable;
        Animator.enabled = !enable;

        // Get all colliders that also have character joint components
        foreach (CharacterJoint joint in GetComponentsInChildren<CharacterJoint>().Where(j => j.GetComponent<Collider>())) {
            joint.enableCollision = enable;
            joint.enablePreprocessing = enable;
            joint.enableProjection = enable;
            //joint.GetComponent<Collider>().enabled = enable;
            joint.GetComponent<Rigidbody>().isKinematic = !enable;
            joint.GetComponent<Rigidbody>().detectCollisions = enable;
            joint.GetComponent<Rigidbody>().useGravity = enable;
        }
    }

    void WaitForOtherEnemies() {
        if (isDead) { return; }
        //int enemiesAtOnce = GameManager.instance.enemiesToChaseAtOnce;

        // Get enemies closest to the same target
        float closestDistance = Mathf.Infinity;
        foreach (Enemy enemy in FindObjectsOfType<Enemy>().Where(e => e.Target == Target && e.FieldOfView.canSeeTarget)) {
            //enemiesAtOnce--;
            //if (enemiesAtOnce <= 0) { return; }
            // If I'm closest to my target
            if (Vector3.Distance(Target.transform.position, enemy.transform.position) < closestDistance) {
                closestDistance = Vector3.Distance(Target.transform.position, enemy.transform.position);
                foreach (Enemy enemy2 in FindObjectsOfType<Enemy>().Where(e => e.Target != null && e.Target == Target && e.FieldOfView.canSeeTarget)) {
                    enemy2.isWaitingForOtherEnemies = true;
                }
                enemy.isWaitingForOtherEnemies = false;
            }
        }
    }

    void RandomizeSizeAndStats() {
        float minScale = 0.6f;
        float maxScale = 1.4f;
        float randomScaleFloat = Random.Range(minScale, maxScale);
        int scaleInt = 0;
        for (int i = 0; i < randomScaleFloat / minScale; i++) {
            scaleInt++;
        }
        if (transform.TryGetComponent(out HealthProperty hp)) {
            hp.maxHealth = EnemyData.MaxHealth + scaleInt;
        }
        transform.localScale = new Vector3(EnemyData.StartSize * randomScaleFloat, EnemyData.StartSize * randomScaleFloat, EnemyData.StartSize * randomScaleFloat);
        turnSpeed = EnemyData.TurnSpeed / randomScaleFloat;
        attackDamage = (int)(EnemyData.AttackDamage * randomScaleFloat);
        movementSpeed = EnemyData.MovementSpeed / (randomScaleFloat / movementSpeedVariation);
        //animationSpeed = EnemyData.AnimationSpeedMultiplier / randomScaleFloat;
        randomPitch = randomScaleFloat;
    }

    // Update is called once per frame
    void Update() {
        currentState = state.ToString();
        state = state.Update(this);
        animTimer += Time.deltaTime;

        if (isDead) { state = state.Die(this); return; }
        if (!Target || isWaitingForOtherEnemies || !FieldOfView.canSeeTarget) {
            state = state.Idle(this);
            return;
        }
        if (Vector3.Distance(transform.position, Target.transform.position) < FieldOfView.attackRadius + FieldOfView.currentRadiusIncrease) {
            state = state.Attack(this); return;
        } else {
            state = state.Chase(this); return;
        }
    }

    public void TurnTowardsTarget(float turnSpeedMultiplier = 1) {
        Vector3 direction = new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z) - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * turnSpeedMultiplier * Time.deltaTime);
        Debug.DrawRay(transform.position, direction);
    }

    public void Chase() {
        transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, movementSpeed * Time.deltaTime * Mathf.Clamp(animTimer, 0, 1));
    }

    public void TryAttack() {
        if (canDamage) {
            canDamage = false;
            Attack();
        }
    }

    public void Attack() {
        attackAnimationLoops++;
        Ray ray = new(new Vector3(transform.position.x, Target.transform.position.y, transform.position.z), transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit)) {
            //ParticleSystem newHitFX = Instantiate(doHitFX, hit.transform.position, Quaternion.identity, transform);
            //Destroy(newHitFX.gameObject, newHitFX.main.duration * 2);

            if (hit.transform.GetComponentInParent<Health>().TryGetComponent(out Health health)) {
                health.TakeDamage(attackDamage);
            }

            // TODO: If target is within said range, damage it and/or all non-Enemy objects
        }
    }
}
