using UnityEngine;

public class Ghost : Enemy {
    [SerializeReference] GameObject homingHead;
    [SerializeReference] Transform spawnHeadPoint;

    public override void Die(float delay) {
        state = state.Die(this);
        spawnFX = deathFX;
        Invoke(nameof(SpawnFX), delay - Time.deltaTime * 2);
        Destroy(gameObject, delay);
        isWaitingForOtherEnemies = true;
    }

    protected override void Update() {
        if (state == new DeadEnemyState()) {
            Debug.Log("shrinking");
            transform.localScale -= new Vector3(1f, 1f, 1f);
            return;
        }
        base.Update();
    }

    public override void TryAttack() {
        base.TryAttack();
    }

    protected override void Attack() {
        if (!homingHead || !spawnHeadPoint) { return; }
        GameObject newHomingHead = Instantiate(homingHead, spawnHeadPoint.position, Quaternion.identity);
        if (newHomingHead.TryGetComponent(out ProjectileDamage pd)) {
            pd.damage = attackDamage;
        }
        Destroy(newHomingHead, 10);
    }
}
