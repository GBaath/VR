using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Linq;

public class Enemy : MonoBehaviour
{
    public Animator anim;
    public GameObject target;

    [SerializeReference] EnemyData enemyData;

    [HideInInspector] public bool mayFight = false;

    // Default EnemyData
    int maxHits = 2;
    int currentHits;
    float movementSpeed = 0.75f;
    float attackRange = 2;
    float turnSpeed = 25;

    Vector3 lastPosition = Vector3.zero;
    const float checkWaitRate = 0.1f;
    const float chaseTurnSpeedMultiplier = 3;

    // Start is called before the first frame update
    void Start()
    {
        maxHits = enemyData.maxHits;
        movementSpeed = enemyData.movementSpeed;
        attackRange = enemyData.attackRange;
        turnSpeed = enemyData.turnSpeed;

        // Check whether or not other enemies are against the target at the same time
        InvokeRepeating(nameof(WaitForOtherEnemies), 0, checkWaitRate);
    }

    private void WaitForOtherEnemies()
    {
        // Get enemies closest to target
        float closestDistance = Mathf.Infinity;
        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            if (Vector3.Distance(target.transform.position, enemy.transform.position) < closestDistance)
            {
                closestDistance = Vector3.Distance(target.transform.position, enemy.transform.position);
                foreach (Enemy enemy2 in FindObjectsOfType<Enemy>())
                {
                    enemy2.mayFight = false;
                }
                enemy.mayFight = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Behaviour
        if (target && Vector3.Distance(transform.position, target.transform.position) >= attackRange)
        {
            Chase();
        }
        else
        {
            Attack();
        }
    }

    private void Chase()
    {
        Debug.DrawLine(transform.position, target.transform.position, Color.yellow);

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("attacking"))
        {
            anim.SetBool("waitingToMove", true);
            return;
        }

        Vector3 direction = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * chaseTurnSpeedMultiplier * Time.deltaTime);

        if (mayFight)
        {
            lastPosition = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, movementSpeed * Time.deltaTime);

            attackRange = enemyData.attackRange;

            anim.SetFloat("movementSpeed", Vector3.Distance(lastPosition, transform.position) / Time.deltaTime);
        }
        else
        {
            // Waits for other enemies to "finish the job"
            anim.SetFloat("movementSpeed", 0);
        }

        anim.SetTrigger("chase");
    }

    private void Attack()
    {
        Debug.DrawLine(transform.position, target.transform.position, Color.red);

        Vector3 direction = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        attackRange = enemyData.attackRange + 0.2f;

        anim.SetTrigger("attack");
        anim.SetBool("waitingToMove", false);
    }
}
