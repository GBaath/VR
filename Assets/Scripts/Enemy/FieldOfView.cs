using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float seeRadius = 8;
    [Range(0, 360)] public float seeAngle = 90;

    public float attackRadius = 3;
    public float attackRadiusIncrease = 1;
    [HideInInspector] public float currentAttackRadiusIncrease;

    public GameObject target;
    public GameObject viewObject;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeeTarget;

    private void Start()
    {
        if (TryGetComponent(out Enemy enemy))
        {
            target = enemy.target;
        }
        else
        {
            target = Camera.main.gameObject;
        }
        InvokeRepeating(nameof(FOVCheck), 0, 0.2f);
    }

    void FOVCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(viewObject.transform.position, seeRadius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - viewObject.transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < seeAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(viewObject.transform.position, target.position);

                if (!Physics.Raycast(viewObject.transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeeTarget = true;
                }
                else
                {
                    canSeeTarget = false;
                }
            }
            else
            {
                canSeeTarget = false;
            }
        }
        else if (canSeeTarget)
        {
            canSeeTarget = false;
        }
    }
}
