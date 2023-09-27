using UnityEngine;

public class ProjectileDamage : MonoBehaviour {
    public int damage = 1;
    public bool fromEnemy = false;
    public bool isHoming = false;

    void Start() {
        InvokeRepeating(nameof(HomeInOnEnemy), 0, 0.2f);
    }

    private void HomeInOnEnemy() {
        if (!isHoming) { return; }

        float closestDistance = Mathf.Infinity;
        Transform closestTargetTransform = null;
        if (fromEnemy) {
            closestTargetTransform = GameManager.instance.playerPhysicsBase.transform;
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
        GetComponent<Rigidbody>().velocity = transform.forward * 10;
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log(other.gameObject);
        if (fromEnemy) {

        } else {
            if (other.gameObject.TryGetComponent(out HealthProperty hp)) {
                hp.LoseHealth(damage);
                AudioSource.PlayClipAtPoint(GameManager.instance.audioManager.hitFeedback, Camera.main.transform.position, 1);
                Destroy(gameObject);
            }
        }
    }
}
