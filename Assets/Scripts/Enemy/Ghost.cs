using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Enemy
{
    [SerializeReference] GameObject homingHead;
    [SerializeReference] GameObject handLimb;

    public override void Attack() {
        if (!homingHead || !handLimb) { return; }
        GameObject newHomingHead = Instantiate(homingHead, handLimb.transform.position, Quaternion.identity);
        if (newHomingHead.TryGetComponent(out ProjectileDamage pd)) {
            pd.damage = attackDamage;
        }
        Destroy(newHomingHead, 10);
    }
}
