using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    NavMeshAgent myAgent;
    public LayerMask whatIsGround, whatIsPlayer;
    public Transform player;
    public Transform firePosition;

    //guarding
    public Vector3 destinationPoint;
    bool destinationSet;
    public float destinationRange;

    //chasing
    public float chaseRange;
    private bool playerInChaseRange;

    //attacking
    public float attackRange;
    private bool playerInAttackRange;
    public GameObject attackProjectile;
    public bool readyToFire = true;
    public float timeBetweenShots;

    //melee
    public bool isMeleeAttacker;
    public int meleeDamageAMT = 2;
    Animator myAnimator;



    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        player = FindObjectOfType<Player>().transform;
        myAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInChaseRange = Physics.CheckSphere(transform.position, chaseRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInChaseRange && !playerInAttackRange)
            Guarding();

        if (playerInChaseRange && !playerInAttackRange)
            ChasingPlayer();

        if (playerInChaseRange && playerInAttackRange)
            AttackingPlayer();
    }

    private void Guarding()
    {
        if(!destinationSet)
        {
            myAnimator.SetTrigger("Running");
            SearchForDestination();
        }
        else
        {
            myAnimator.SetTrigger("Running");
            myAgent.SetDestination(destinationPoint);
        }

        Vector3 distanceToDestination = transform.position - destinationPoint;
        if (distanceToDestination.magnitude < 1f)
            destinationSet = false;
    }
    private void ChasingPlayer()
    {
        myAnimator.SetTrigger("Running");
        myAgent.SetDestination(player.position);
    }

    private void SearchForDestination()
    {
        //Create random point for our agent to walk to
        float randPotitionZ = Random.Range(-destinationRange, destinationRange);
        float randPotitionX = Random.Range(-destinationRange, destinationRange);

        //set the destination
        destinationPoint = new Vector3(transform.position.x + randPotitionX, transform.position.y, transform.position.z + randPotitionZ);

        if (Physics.Raycast(destinationPoint, -transform.up, 2f, whatIsGround))
            destinationSet = true;
    }
    private void AttackingPlayer()
    {
        myAgent.SetDestination(transform.position);
        transform.LookAt(player);
        
        if(readyToFire && !isMeleeAttacker)
        {
            myAnimator.SetTrigger("Attack");

            firePosition.LookAt(player);

            Instantiate(attackProjectile, firePosition.position, firePosition.rotation);
            readyToFire = false;
            StartCoroutine(Reloading());
        }
        else if(readyToFire && isMeleeAttacker)
        {
            myAnimator.SetTrigger("Attack");
        }
    }

    public void MeleeDamage()
    {
        if(playerInAttackRange)
        {
            player.GetComponent<PlayerHealthSystem>().TakeDamage(meleeDamageAMT);

        }
    }

    IEnumerator Reloading()
    {
        yield return new WaitForSeconds(timeBetweenShots);

        readyToFire = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
