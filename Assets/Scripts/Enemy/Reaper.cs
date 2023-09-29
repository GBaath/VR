using UnityEngine;
public class Reaper : Enemy
{
    [SerializeField] float movementSpeedMultiplier = 0.001f;
    protected override void Start() {
        state = new ChaseEnemyState();
        RandomizeSizeAndStats();
        movementSpeed = movementSpeedMultiplier;
    }

    protected override void Update() {
        currentState = state.ToString();
        state = state.Update(this);
        animTimer = 1;
        movementSpeed *= 1 + movementSpeedMultiplier;

        if (Vector3.Distance(transform.position, Target.transform.position) < FieldOfView.attackRadius + FieldOfView.currentRadiusIncrease) {
            Attack();
        }
    }
}
