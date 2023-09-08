using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator anim;
    public GameObject target;

    public int turnSpeed = 10;

    public enum State
    {
        chase,
        attack
    }
    public State currentState = State.chase;

    int maxHits = 2;
    int currentHits;
    float movementSpeed = 1f;
    float attackRange = 2;

    Vector3 lastPosition = Vector3.zero;
    bool waitingToMove = false;

    [SerializeReference] EnemyData enemyData;

    // Start is called before the first frame update
    void Start()
    {
        maxHits = enemyData.maxHits;
        movementSpeed = enemyData.movementSpeed;
        attackRange = enemyData.attackRange;
        Application.targetFrameRate = 90;
    }

    // Update is called once per frame
    void Update()
    {
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
        // Debugging
        Debug.DrawLine(transform.position, target.transform.position, Color.yellow);
        currentState = State.chase;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("attacking"))
        {
            anim.SetBool("waitingToMove", true);
            return;
        }
        transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
        lastPosition = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, movementSpeed * Time.deltaTime);
        anim.SetTrigger("chase");
        anim.SetFloat("movementSpeed", Vector3.Distance(lastPosition, transform.position) / Time.deltaTime);
    }

    private void Attack()
    {
        // Debugging
        Debug.DrawLine(transform.position, target.transform.position, Color.red);
        currentState = State.attack;

        anim.SetTrigger("attack");
        anim.SetBool("waitingToMove", false);

        Vector3 direction = target.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        //transform.rotation = Quaternion.RotateTowards(transform.position, target.transform.position);

        //Vector3.RotateTowards(transform.position, target.transform.position, movementSpeed * Time.deltaTime, movementSpeed * Time.deltaTime);
    }
}
