using UnityEngine;
using System.Linq;

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
                playerHealth.TakeDamage(damage);
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
