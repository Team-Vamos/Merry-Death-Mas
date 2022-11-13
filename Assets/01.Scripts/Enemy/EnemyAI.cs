using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyTargetType
{
    Tree,
    Player,
    anyone,
}

public enum EnemyAtkType
{
    longRange,
    shortRange,
}

public class EnemyAI : MonoBehaviour
{

    private EnemySpawn enemySpawn;
    [SerializeField]
    private EnemyTargetType enemyTargetType;

    [SerializeField]
    private EnemyAtkType enemyAtkType;

    public NavMeshAgent agent;

    public Transform Target;

    public LayerMask whatIsGround, whatIsPlayer;

    public Transform BulletPosition;
    
    public float health;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Start()
    {

    }

    private void Awake()
    {
        switch(enemyTargetType)
        {
            case EnemyTargetType.Player:
                Target = GameObject.Find("Player").transform;
                break;
            case EnemyTargetType.Tree:
                Target = GameObject.Find("Tree").transform;
                break;
            case EnemyTargetType.anyone:
                break;

        }

        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(Target.position);
    }

    private void AttackPlayer()
    { 
        agent.SetDestination(transform.position);

        transform.LookAt(Target);

        if (!alreadyAttacked)
        {
            switch(enemyAtkType)
            {
                case EnemyAtkType.longRange:
                    ///Attack code here
                    Rigidbody rb = Instantiate(projectile, BulletPosition.position, Quaternion.identity).GetComponent<Rigidbody>();
                    rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
                    ///End of attack code
                    break;
                case EnemyAtkType.shortRange:
                    Debug.Log("Attack");
                    break;
            }




            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
