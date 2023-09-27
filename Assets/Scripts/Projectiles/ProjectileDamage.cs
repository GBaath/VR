using UnityEngine;

public class ProjectileDamage : MonoBehaviour {
    public int damage = 1;
    public bool fromEnemy = false;
    public bool isHoming = false;
    public float homingSpeed = 10;

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
            if (!FindAnyObjectByType<Enemy>()) { return; }
            foreach (Enemy enemy in FindObjectsOfType<Enemy>()) {
                if (Vector3.Distance(transform.position, enemy.transform.position) < closestDistance) {
                    closestDistance = Vector3.Distance(transform.position, enemy.transform.position);
                    closestTargetTransform = enemy.GetRandomLimb().transform;
                }
            }
        }
        
        transform.LookAt(new Vector3(closestTargetTransform.position.x, closestTargetTransform.position.y, closestTargetTransform.position.z));
        GetComponent<Rigidbody>().velocity = transform.forward * homingSpeed;
    }

    private void OnHit(Transform otherTransform) {
        if (fromEnemy) {
            if (otherTransform.GetComponentInParent<Health>() && otherTransform.GetComponentInParent<Health>().TryGetComponent(out Health health)) {
                health.TakeDamage(damage);
                Destroy(gameObject);
            }
        } else {
            if (otherTransform.GetComponentInParent<HealthProperty>() && otherTransform.TryGetComponent(out HealthProperty hp)) {
                hp.LoseHealth(damage);
                AudioSource.PlayClipAtPoint(GameManager.instance.audioManager.hitFeedback, Camera.main.transform.position, 1);
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision other) {
        OnHit(other.transform);
    }

    private void OnTriggerEnter(Collider other) {
        OnHit(other.transform);
    }
}
