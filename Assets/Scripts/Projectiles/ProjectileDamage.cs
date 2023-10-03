using UnityEngine;

public class ProjectileDamage : MonoBehaviour {
    public int damage = 1;
    public bool isHoming = false;
    public float homingSpeed = 10;
    public bool fromEnemy = false;

    void Start() {
        InvokeRepeating(nameof(HomeInOnEnemy), 0, 0.2f);
    }

    private void HomeInOnEnemy() {
        if (!isHoming) { return; }
        float closestDistance = Mathf.Infinity;
        Transform closestTargetTransform = null;
        if (fromEnemy) {
            closestTargetTransform = Camera.main.transform;
        } else {
            if (!FindAnyObjectByType<HealthProperty>()) { return; }
            foreach (HealthProperty hp in FindObjectsOfType<HealthProperty>()) {
                if (hp.TryGetComponent(out Enemy enemy) && enemy.isDead) { continue; }
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
        if (fromEnemy) {
            if (otherTransform.GetComponentInParent<Health>() && otherTransform.GetComponentInParent<Health>().TryGetComponent(out Health playerHealth)) {
                // Damage player
                playerHealth.TakeDamage(damage, gameObject);
                Destroy(gameObject);
                return;
            }
        } else {
            if (otherTransform.GetComponentInParent<HealthProperty>() && otherTransform.GetComponentInParent<HealthProperty>().TryGetComponent(out HealthProperty enemyHP)) {
                // Damage enemy
                enemyHP.LoseHealth(damage, GameManager.instance.playerPhysicsBase);
                AudioSource.PlayClipAtPoint(GameManager.instance.audioManager.hitFeedback, Camera.main.transform.position, 1);
                Destroy(gameObject);
                return;
            }
        }
        if (!fromEnemy && otherTransform.GetComponentInParent<ProjectileDamage>() && otherTransform.GetComponentInParent<ProjectileDamage>().fromEnemy) {
            Destroy(otherTransform.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other) {
        OnHit(other.transform);
    }
}
