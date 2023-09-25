using UnityEngine;

public class FieldOfView : MonoBehaviour {
    public float seeRadius = 8;
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
        if (!target) {
            if (TryGetComponent(out Enemy enemy)) {
                target = enemy.target;
            } else {
                target = Camera.main.gameObject;
            }
        }

        Collider[] rangeChecks = Physics.OverlapSphere(viewObject.transform.position, seeRadius + currentRadiusIncrease, targetMask);

        if (rangeChecks.Length != 0) {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - viewObject.transform.position).normalized;

            if (/*Vector3.Angle(directionToTarget, transform.forward) < seeAngle / 2*/ true) {
                float distanceToTarget = Vector3.Distance(viewObject.transform.position, target.position);
                if (!Physics.Raycast(viewObject.transform.position, directionToTarget, distanceToTarget, obstructionMask)) {
                    Debug.Log("1");
                    canSeeTarget = true;
                } else {
                    Debug.Log("a");
                    canSeeTarget = false;
                }
            }
            //} else {
            //    Debug.Log("b");
            //    canSeeTarget = false;
            //}
        } else if (canSeeTarget) {
            Debug.Log("c");
            canSeeTarget = false;
        }
    }
}
