using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerRadius : MonoBehaviour
{


    public delegate void EnemyEnteredEvent(HealthProperty health);
    public delegate void EnemyExitedEvent(HealthProperty health);

    public event EnemyEnteredEvent OnEnemyEnter;
    public event EnemyEnteredEvent OnEnemyExit;

    private List<HealthProperty> EnemiesInRadius = new List<HealthProperty>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<HealthProperty>(out HealthProperty health))
        {
            EnemiesInRadius.Add(health);
            OnEnemyEnter?.Invoke(health);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<HealthProperty>(out HealthProperty enemy))
        {
            EnemiesInRadius.Remove(enemy);
            OnEnemyExit?.Invoke(enemy);
        }
    }

    private void OnDisable()
    {
        foreach (HealthProperty health in EnemiesInRadius)
        {
            OnEnemyExit?.Invoke(health);
        }

        EnemiesInRadius.Clear();
    }
}


