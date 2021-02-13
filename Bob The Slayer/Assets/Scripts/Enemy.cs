using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{

    public float health = 50f;

    public NavMeshAgent agent;

    public Transform player;


    public LayerMask whatIsGround, whatIsPlayer;

    private State state = State.Patrolling;

    enum State
    {
        Patrolling,
        Chasing,
        Attacking
    }

    // Patroling

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking

    public float timeBetweenAttacks;
    bool alreadyAttacked;

    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        Debug.Log(agent.name+ " has spawned");
    }
    private void Update()
    {

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patrolling();
            if (state != State.Patrolling)
            {
                Debug.Log(agent.gameObject.name + " is now Patrolling");
                state = State.Patrolling;
            }
        }
        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
            if (state != State.Chasing && !alreadyAttacked)
            {
                state = State.Chasing;
                Debug.Log(agent.gameObject.name + " is now Chasing");
            }
        }

        if (playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
            if (state != State.Attacking)
            {
                state = State.Attacking;
                
            }
        }

    }
    private void Patrolling()
    {
        if (!walkPointSet) { SearchWalkPoint(); }
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }
        Vector3 distancetoWalkPoint = transform.position - walkPoint;
        if (distancetoWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    private void AttackPlayer()
    {
        // Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        // transform.LookAt(player);
        if (!alreadyAttacked)
        {
            /// Attack code here
            player.GetComponent<Player>().TakeDamage(20f);
            Debug.Log(agent.gameObject.name + " is now Attacking");
            /// 
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
        Debug.Log("Enemy now has " + health + " hp after i having taken "+amount+" of damage");
    }
    void Die()
    {
        Destroy(gameObject);
        Debug.Log(agent.gameObject.name + "has died");
        Debug.Log(FindObjectsOfType<Enemy>().Length-1 + " enemies are left");
        if(FindObjectsOfType<Enemy>().Length == 1)
        {
            SceneManager.LoadScene("Victory");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
