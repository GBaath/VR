using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public int maxHits = 2;
    public float movementSpeed = 2;
    public float attackRange = 2;
}
