using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Controllers;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class EnemyAI : MonoBehaviour
{
    //Attack stuff
    [SerializeField] private float range;
    [SerializeField] private float damage;

    //Helps in creating an enemy that will avoid obstacles, have a patrol and go to the player if seen
    [SerializeField] private NavMeshAgent agent; 
    
    [SerializeField] private Transform player; 

    [SerializeField] private LayerMask whatIsGround, whatIsPlayer; 

    //Patroling
    [SerializeField] private Vector3 walkPoint; 
    private bool _walkPointSet;
    [SerializeField] private float walkPointRange;
    
    //Attacking
    [SerializeField] private float timeBetweenAttacks; 
    private bool _alreadyAttacked; 
    
    //Attacking
    [SerializeField] private float sightRange, attackRange; 
    private bool _playerInSightRange, _playerInAttackRange; 

    private Animator _animator;
    private String mutanAnimationString = "MutantAnimations";
    private String mutantAttackString = "MutantAttack";


    // Start is called before the first frame update
    void Awake()
    {

        
        _animator = agent.GetComponent<Animator>();
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }



    // Update is called once per frame
    void Update()
    {
        //Check if in attack and sight range
        _playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        _playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);


        if (!_playerInSightRange && !_playerInAttackRange)
        {
            _animator.SetFloat(mutanAnimationString,1);

            Patrolling();
        }
        if (_playerInSightRange && !_playerInAttackRange)
        {
            _animator.SetFloat(mutanAnimationString,1);

            ChasePlayer();
        }
        if (_playerInSightRange && _playerInAttackRange)
        {
            _animator.SetTrigger(mutantAttackString);

            AttackPlayer();
        }
    }


    private void Patrolling()
    {
        if (!_walkPointSet)
        {
            SearchWalkPoint();
        }
        
        if (_walkPointSet)
        {
            //Sets the AI to walk to destination
            agent.SetDestination(walkPoint);
        }

        //Calculate distance to walkpoint
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        
        //If distance <1  the walkpoint is reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            _walkPointSet = false; //<-- Now the AI will search for a new one
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
            _walkPointSet = true;
            
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


        
        if (!_alreadyAttacked)
        {
            _alreadyAttacked = true;
            DamangeHandler damangeHandler = player.GetComponent<DamangeHandler>();
            if (damangeHandler != null)
            {
                damangeHandler.TakeDamage(damage, damangeHandler.name);
            }
            
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        _alreadyAttacked = false;
    }





    
}
