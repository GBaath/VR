using UnityEngine;

public class FieldOfView : MonoBehaviour {
    public float seeRadius;
    [Range(0, 360)] public float seeAngle = 90;

    public float attackRadius = 3;
    public float radiusIncrease = 1;
    [HideInInspector] public float currentRadiusIncrease;

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

        Collider[] rangeChecks = Physics.OverlapSphere(viewObject.transform.position, seeRadius + currentRadiusIncrease, targetMask);

        if (rangeChecks.Length != 0) {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - viewObject.transform.position).normalized;

            if (/*Vector3.Angle(directionToTarget, transform.forward) < seeAngle / 2*/ true) {
                float distanceToTarget = Vector3.Distance(viewObject.transform.position, target.position);
                if (!Physics.Raycast(viewObject.transform.position, directionToTarget, distanceToTarget, obstructionMask)) {
                    canSeeTarget = true;
                } else {
                    canSeeTarget = false;
                }
            }
        } else if (canSeeTarget) {
            canSeeTarget = false;
        }
    }
}
