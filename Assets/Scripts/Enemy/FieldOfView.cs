using UnityEngine;

public class FieldOfView : MonoBehaviour {
    public float seeRadius = 10;
    public float seeRadiusIncrease = 20;
    [HideInInspector] public float currentSeeRadiusIncrease;
    [Range(0, 360)] public float seeAngle = 90;

    public float attackRadius = 3;
    public float attackRadiusIncrease = 1;
    [HideInInspector] public float currentAttackRadiusIncrease;

    public GameObject target;
    public GameObject viewObject;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeeTarget;

    private void Start() {
        InvokeRepeating(nameof(FOVCheck), 0, 0.2f);
    }

    void FOVCheck() {
        TryGetComponent(out Enemy enemy);
        if (!target) {
            target = enemy.Target;
        }
        if (enemy.Head) {
            viewObject = enemy.Head;
        }

        if (canSeeTarget) {
            currentSeeRadiusIncrease = seeRadiusIncrease;
        } else {
            currentSeeRadiusIncrease = 0;
        }
        Collider[] rangeChecks = Physics.OverlapSphere(viewObject.transform.position, seeRadius + currentSeeRadiusIncrease, targetMask);

        if (rangeChecks.Length != 0) {
            foreach (Collider item in rangeChecks) {
                Transform target = item.transform;
                Vector3 directionToTarget = (target.position - viewObject.transform.position).normalized;
                if (Vector3.Angle(directionToTarget, transform.forward) < seeAngle / 2) {
                    float distanceToTarget = Vector3.Distance(viewObject.transform.position, target.position);
                    if (Physics.Raycast(viewObject.transform.position, directionToTarget, distanceToTarget, obstructionMask)) {
                        canSeeTarget = false;
                    } else {
                        canSeeTarget = true;
                    }
                } else {
                    canSeeTarget = false;
                }
            }
        } else {
            canSeeTarget = false;
        }
    }
}
