using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable {
    // Cached references
    [SerializeReference] GameObject hips;
    [SerializeReference] GameObject head;
    [SerializeReference] Animator animator;
    [SerializeReference] GameObject target;
    [SerializeReference] EnemyData enemyData;
    [SerializeReference] AudioData dmgAudioData;
    [SerializeReference] GameObject deathFX;
    [SerializeReference] FieldOfView fieldOfView;
    [SerializeReference] AudioSource audioSource;
    //[SerializeReference] AudioClip

    [Tooltip("Displays the current state this enemy is in. State cannot be changed outside the EnemyState StateMachine.")]
    [SerializeField] protected string currentState = new IdleEnemyState().ToString();

    // Public variables
    public GameObject Head {
        get {
            if (head) {
                return head;
            } else {
                Debug.LogWarning("Reference to head is missing!");
                return null;
            }
        }
    }
    public Animator Animator {
        get { return animator; }
    }
    public GameObject Target {
        get {
            if (!target) {
                target = Camera.main.gameObject;
                return target;
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
        if (!hips) { return gameObject; }
        List<GameObject> allLimbs = new();
        foreach (Transform childTransform in hips.GetComponentInChildren<Transform>()) {
            allLimbs.Add(childTransform.gameObject);
        }
        if (allLimbs.Count > 0) {
            return allLimbs[Random.Range(0, allLimbs.Count)];
        } else {
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
    [HideInInspector] public float animTimer = 0;

    // Stats
    protected float turnSpeed;
    protected int attackDamage;
    [HideInInspector] public float movementSpeed;

    // Magic numbers
    [HideInInspector] public float chaseTurnSpeedMultiplier = 3;
    [HideInInspector] public int attackAnimationLoops = 0;
    const float movementSpeedVariation = 0.5f;
    const float checkWaitRate = 0.1f;
    float currentMaterialColor = 0f;

    // Private variables
    MaterialPropertyBlock propertyBlock;
    new SkinnedMeshRenderer renderer;
    GameObject spawnFX = null;

    //bool IDamageable.IsDead { get => isDead; set => isDead = value; }
    public virtual void TakeDamage(float amount, bool isDead) {
        if (dmgAudioData) {
            audioSource.PlayOneShot(dmgAudioData.GetRandomClip(), 1);
        } else {
            audioSource.clip = GameManager.instance.audioManager.enemyHit;
            audioSource.Play();
        }
        SetDmgFlash();
        if (isDead) { return; }
        FieldOfView.canSeeTarget = true;
        TakeKnockback(amount, Camera.main.gameObject);
    }
    public virtual void Die(float delay) {
        state = state.Die(this);
        Invoke(nameof(SpawnFX), delay - .01f);
        Destroy(gameObject, delay);
        spawnFX = deathFX;
        isWaitingForOtherEnemies = true;
        if (!hips) { return; }
        RagdollSetActive(true);
    }
    void SpawnFX() {
        Debug.Log("spawnFX : DeathFX");
        GameObject newFX;
        if (spawnFX) {
            newFX = Instantiate(spawnFX, transform.position, Quaternion.identity);
            foreach (ParticleSystem particleSystem in newFX.GetComponentsInChildren<ParticleSystem>()) {
                particleSystem.Play();
            }
            spawnFX = null;
        } else {
            Debug.LogWarning("Missing particle: " + spawnFX);
        }
    }
    void SetDmgFlash() {
        currentMaterialColor = 0.4f;
        propertyBlock.SetColor("_EmissionColor", new Color(currentMaterialColor, currentMaterialColor, currentMaterialColor));
        propertyBlock.SetFloat("_EmissionIntensity", currentMaterialColor);
        renderer.SetPropertyBlock(propertyBlock);
    }
    void LessenDmgFlash() {
        currentMaterialColor = Mathf.MoveTowards(currentMaterialColor, 0, 1.25f * Time.deltaTime);
        propertyBlock.SetColor("_EmissionColor", new Color(currentMaterialColor, currentMaterialColor, currentMaterialColor));
        propertyBlock.SetFloat("_EmissionIntensity", currentMaterialColor);
        renderer.SetPropertyBlock(propertyBlock);
    }
    void TakeKnockback(float dmg, GameObject dmgSource) {
        if (dmgSource == null) { return; }
        Vector3 directionVector = new Vector3(
            transform.position.x, transform.position.y, transform.position.z) -
            new Vector3(dmgSource.transform.position.x, transform.position.y, dmgSource.transform.position.z);

        transform.position += 0.2f * dmg * (1 / transform.localScale.y) * directionVector.normalized;
    }
    protected virtual void Start() {
        renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        propertyBlock = new MaterialPropertyBlock();
        propertyBlock.SetColor("_EmissionColor", Color.black);
        propertyBlock.SetFloat("_EmissionIntensity", 0);
        renderer.SetPropertyBlock(propertyBlock);

        transform.Rotate(new Vector3(transform.rotation.x, Random.Range(0, 360), transform.rotation.z));
        RagdollSetActive(false);

        turnSpeed = EnemyData.TurnSpeed;
        attackDamage = EnemyData.AttackDamage;
        movementSpeed = EnemyData.MovementSpeed;

        if (GameManager.instance.enemiesToChaseAtOnce > 0) {
            InvokeRepeating(nameof(WaitForOtherEnemies), 0, checkWaitRate);
        } else {
            isWaitingForOtherEnemies = false;
        }
        state = state.Idle(this);
    }
    void RagdollSetActive(bool enable) {
        GetComponent<BoxCollider>().enabled = !enable;
        Animator.enabled = !enable;
        // Get all colliders that also have character joint components
        foreach (CharacterJoint joint in GetComponentsInChildren<CharacterJoint>()) {
            joint.enablePreprocessing = enable;
            joint.enableProjection = enable;
            joint.enableCollision = enable;
            joint.TryGetComponent(out Collider collider);
            collider.enabled = enable;
            joint.TryGetComponent(out Rigidbody rb);
            rb.detectCollisions = enable;
            rb.useGravity = enable;
            rb.isKinematic = !enable;
        }
    }
    void WaitForOtherEnemies() {
        if (state == new DeadEnemyState()) { return; }
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
    protected virtual void Update() {
        currentState = state.ToString();
        state = state.Update(this);
        animTimer += Time.deltaTime;
        LessenDmgFlash();

        if (state == new DeadEnemyState()) { state = state.Die(this); return; }
        if (!Target || isWaitingForOtherEnemies || !FieldOfView.canSeeTarget) {
            state = state.Idle(this);
            return;
        }
        if (Vector3.Distance(transform.position, Target.transform.position) < FieldOfView.attackRadius + FieldOfView.currentAttackRadiusIncrease) {
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
    public virtual void TryAttack() {
        if (canDamage) {
            canDamage = false;
            attackAnimationLoops++;
            Attack();
        }
    }
    protected virtual void Attack() {
        // Melee is default enemy attack style
        Ray ray = new(new Vector3(transform.position.x, Target.transform.position.y, transform.position.z), transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit)) {
            if (hit.transform.GetComponentInParent<Health>() && hit.transform.GetComponentInParent<Health>().TryGetComponent(out Health health)) {
                health.TakeDamage(attackDamage);
            }
        }
    }
}
