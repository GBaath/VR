using UnityEngine;

public class ProjectileDamage : MonoBehaviour {
    public float damage = 1;
    public bool isHoming = false;
    public float homingSpeed = 10;
    public bool fromEnemy = false;
    public bool isFireball = false;
    [HideInInspector] public bool isDead = false;

    void Start() {
        InvokeRepeating(nameof(HomeInOnEnemy), 0, 0.2f);
    }

    private void HomeInOnEnemy() {
        if (!isHoming || isDead) { return; }
        float closestDistance = Mathf.Infinity;
        Transform closestTargetTransform = null;
        if (fromEnemy) {
            closestTargetTransform = Camera.main.transform;
        } else {
            if (!FindAnyObjectByType<HealthProperty>()) { return; }
            foreach (HealthProperty hp in FindObjectsOfType<HealthProperty>()) {
                if (hp.TryGetComponent(out Enemy enemy) && enemy.state == new DeadEnemyState()) { continue; }
                if (Vector3.Distance(transform.position, hp.transform.position) < closestDistance) {
                    closestDistance = Vector3.Distance(transform.position, hp.transform.position);
                    if (enemy) {
                        closestTargetTransform = enemy.GetRandomLimb().transform;
                    } else {
                        closestTargetTransform = hp.transform;
                    }
                }
            }
        }
        if (!closestTargetTransform) { Destroy(gameObject); return; }
        transform.LookAt(new Vector3(closestTargetTransform.position.x, closestTargetTransform.position.y, closestTargetTransform.position.z));
        GetComponent<Rigidbody>().velocity = transform.forward * homingSpeed;
    }

    private void OnHit(Transform otherTransform) {
        if (!fromEnemy && otherTransform.CompareTag("MainCamera") && otherTransform.GetComponentInParent<Health>()) {
            Suicide(otherTransform);
            return;
        }
        if (!fromEnemy && otherTransform.GetComponentInParent<ProjectileDamage>() && otherTransform.GetComponentInParent<ProjectileDamage>().fromEnemy) {
            KillHomingSkull(otherTransform);
            return;
        }
        if (fromEnemy && otherTransform.GetComponentInParent<Health>() && otherTransform.GetComponentInParent<Health>().TryGetComponent(out Health playerHealth)) {
            if (isFireball) {
                DamagePlayer(playerHealth, true);
            } else {
                DamagePlayer(playerHealth);
            }
            return;
        }
        if (otherTransform.GetComponentInParent<HealthProperty>() && otherTransform.GetComponentInParent<HealthProperty>().TryGetComponent(out HealthProperty enemyHP)) {
            DamageEnemy(enemyHP);
            return;
        }
    }

    private void Suicide(Transform otherTransform) {
        otherTransform.GetComponentInParent<Health>().TryGetComponent(out Health playerHealth);
        playerHealth.Die(0.2f);
    }

    private void KillHomingSkull(Transform otherTransform) {
        ProjectileDamage otherPD = otherTransform.GetComponentInParent<ProjectileDamage>();
        otherPD.isDead = true;
        AudioSource.PlayClipAtPoint(GameManager.instance.audioManager.hitFeedback, transform.position, 1);
        Destroy(otherPD.gameObject);
        Destroy(gameObject);
    }

    private void DamagePlayer(Health playerHealth, bool fromFireball = false) {
        if (fromFireball && TryGetComponent(out FireballAudio audio)) {
            audio.PlayFireballImpact();
        }
        playerHealth.TakeDamage(damage);
        Destroy(gameObject);
    }

    private void DamageEnemy(HealthProperty enemyHP) {
        //if (!enemyHP.isDead)
        //    AudioSource.PlayClipAtPoint(GameManager.instance.audioManager.hitFeedback, Camera.main.transform.position, 1);
        enemyHP.LoseHealth(damage);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other) {
        OnHit(other.transform);
    }
}
