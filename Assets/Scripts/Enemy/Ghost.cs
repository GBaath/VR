using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ghost : Enemy {
    [SerializeReference] GameObject homingHead;
    [SerializeReference] GameObject handLimb;
    [SerializeReference] List<GameObject> destroyWhenKilled = new();

    public override void Die(float delay) {
        base.Die(delay);
        foreach (GameObject item in destroyWhenKilled) {
            Destroy(item);
        }
    }

    public override void Attack() {
        if (!homingHead || !handLimb) { return; }
        GameObject newHomingHead = Instantiate(homingHead, handLimb.transform.position, Quaternion.identity);
        if (newHomingHead.TryGetComponent(out ProjectileDamage pd)) {
            pd.damage = attackDamage;
        }
        Destroy(newHomingHead, 10);
    }
}
