using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ghost : Enemy {
    [SerializeReference] GameObject homingHead;
    [SerializeReference] Transform spawnHeadPoint;
    //[SerializeReference] List<Transform> destroyWhenKilled = new();

    //public override void Die(float delay) {
    //    base.Die(delay);
    //    foreach (Transform item in destroyWhenKilled) {
    //        Destroy(item.gameObject);
    //    }
    //}

    public override void Attack() {
        if (!homingHead || !spawnHeadPoint) { return; }
        GameObject newHomingHead = Instantiate(homingHead, spawnHeadPoint.position, Quaternion.identity);
        if (newHomingHead.TryGetComponent(out ProjectileDamage pd)) {
            pd.damage = attackDamage;
        }
        Destroy(newHomingHead, 10);
    }
}
