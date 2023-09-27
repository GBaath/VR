using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingSimpson : Enemy, IDamageable
{
    public override void TakeDamage(int amount) {
        base.TakeDamage(amount);
    }

    public override void Die(float delay) {
        Destroy(gameObject);
    }
}
