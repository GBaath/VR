using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    public int damage = 1;

    public bool isHoming = false;

    public delegate void OnHitEnemy(GameObject hitBodypart, GameObject bullet);
    public static OnHitEnemy onHitEnemy;

    void Start()
    {
        InvokeRepeating(nameof(HomeInOnEnemy), 0, 0.2f);
    }

    private void HomeInOnEnemy()
    {
        if (!isHoming) { return; }

        float closestDistance = Mathf.Infinity;
        Transform closestEnemyTransform = null;
        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < closestDistance)
            {
                closestDistance = Vector3.Distance(transform.position, enemy.transform.position);
                closestEnemyTransform = enemy.GetRandomLimb().transform;
            }
        }
        transform.LookAt(new Vector3(closestEnemyTransform.position.x, closestEnemyTransform.position.y, closestEnemyTransform.position.z));
        GetComponent<Rigidbody>().velocity = transform.forward * 10;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.GetComponent<ProjectileDamage>())
        {
            //onHitEnemy(other.collider.gameObject, gameObject);
            Destroy(gameObject);
        }
    }
}
