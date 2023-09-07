using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator anim;
    public GameObject player;

    int maxHits = 2;
    int currentHits;
    float movementSpeed = 2;
    float attackRange = 2;

    [SerializeReference] EnemyData enemyData;

    // Start is called before the first frame update
    void Start()
    {
        maxHits = enemyData.maxHits;
        movementSpeed = enemyData.movementSpeed;
        attackRange = enemyData.attackRange;
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
            transform.LookAt(player.transform);
        if (Vector3.Distance(gameObject.transform.position, player.transform.position) > attackRange)
        {
            // Chase player
            Debug.DrawLine(gameObject.transform.position, player.transform.position, Color.yellow);
            //transform.Translate(movementSpeed * Time.deltaTime * transform.forward);
            transform.position = Vector3.MoveTowards(gameObject.transform.position, player.transform.position, movementSpeed * Time.deltaTime);
        }
        else
        {
            // Attack player
            Debug.DrawLine(gameObject.transform.position, player.transform.position, Color.red);
            anim.SetTrigger("attack");
        }
    }
}
