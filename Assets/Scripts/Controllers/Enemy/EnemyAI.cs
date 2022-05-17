using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

public class EnemyAI : MonoBehaviour
{

    //Helps in creating an enemy that will avoid obstacles, have a patrol and go to the player if seen
    [SerializeField] private NavMeshAgent agent; 
    
    [SerializeField] private Transform player; 

    [SerializeField] private LayerMask whatIsGround, whatIsPlayer; 

    //Patroling
    [SerializeField] private Vector3 walkPoint; 
    private bool walkPointSet;
    [SerializeField] private float walkPointRange;
    
    //Attacking
    [SerializeField] private float timeBetweenAttacks; 
    private bool alreadyAttacked; 
    
    //Attacking
    [SerializeField] private float sightRange, attackRange; 
    private bool playerInSightRange, playerInAttackRange; 


    
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if in attack and sight range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);


        if (!playerInSightRange && !playerInAttackRange)
        {
            Patrolling();
        }
        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        if (playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
        }
    }


    private void Patrolling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        
        if (walkPointSet)
        {
            //Sets the AI to walk to destination
            agent.SetDestination(walkPoint);
        }

        //Calculate distance to walkpoint
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        
        //If distance <1  the walkpoint is reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false; //<-- Now the AI will search for a new one
        }
    }

    private void SearchWalkPoint()
    {
        //Debug.Log("ENEMY SEARCHING");

        
        //Random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        //Searches for random point
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        
        //Fix for only being inside map
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
            
        }

    }

    private void ChasePlayer()
    {
        //Debug.Log("ENEMY CHASING");

        agent.SetDestination(player.position);
    }
    
    private void AttackPlayer()
    {
       // Debug.Log("ENEMY ATTACKS");
        
        //Enemy should not move when attacking
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            //Insert attack animations
            
            
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    
}
