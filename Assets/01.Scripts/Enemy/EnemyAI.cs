using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;   

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
    private Animator anim;

    private EnemySpawn enemySpawn;
    [SerializeField]
    private EnemyTargetType enemyTargetType;

    [SerializeField]
    private EnemyAtkType enemyAtkType;

    public NavMeshAgent agent;

    public Transform Target;

    public LayerMask whatIsGround, whatIsPlayer;

    private Transform BulletPosition;

    private float health;

    [SerializeField]
    private float MaxHealth;

    private Vector3 walkPoint;
    bool walkPointSet;
    private float walkPointRange;

    public Slider healthBar;

    public int DropCandyMin;
    public int DropCandyMax;

    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;


    private float sightRange = 1001f;
    public float attackRange;
    private bool playerInSightRange;
    public bool playerInAttackRange;

    public bool isAtk = false;
    private bool stop = false;

    [SerializeField]
    Renderer _renderer;


    private void Start()
    {
        health = MaxHealth;
        FindTarget();
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(health>=MaxHealth)
        {
            healthBar.gameObject.SetActive(false);
        }
        else
        {
            Health();
            healthBar.gameObject.SetActive(true);
        }



        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange&&alreadyAttacked==false && !stop) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();

        if(Target == null)
        {
            stop = true;
            FindTarget();
        }
    }

    void Health()
    {
        healthBar.value = health / MaxHealth;
    }

    private void FindTarget()
    {
        switch (enemyTargetType)
        {
            case EnemyTargetType.Player:
                Target = GameObject.Find("Player").transform;
                break;
            case EnemyTargetType.Tree:
                Target = GameObject.Find("Tree").transform;
                break;
            case EnemyTargetType.anyone:
                break;
            default:
                FindTarget();
                break;

        }
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;


        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(Target.position);
        anim.SetBool("ToTarget", true);

    }

    private void AttackPlayer()
    { 
        agent.SetDestination(transform.position);
        transform.LookAt(Target);
        anim.SetBool("ToTarget", false);

        if (!alreadyAttacked)
        {
            switch(enemyAtkType)
            {
                case EnemyAtkType.longRange:
                    Rigidbody rb = Instantiate(projectile, BulletPosition.position, Quaternion.identity).GetComponent<Rigidbody>();
                    rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
                    break;
                case EnemyAtkType.shortRange:
                    isAtk = true;
                    anim.SetTrigger("Attack");
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

    public void TakeDamage(float damage)
    {

        Debug.Log("current Health: " + health);
        Debug.Log("current Dmg: " + damage);

        health -= damage;

        Debug.Log("next Health: " + health);
        Debug.Log("next Dmg: " + damage);

        StartCoroutine(Hit());
        if (health <= 0)
        {
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }

    private IEnumerator Hit()
    {
        _renderer.materials[0].color = Color.red;
        _renderer.materials[1].color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _renderer.materials[0].color = Color.white;
        _renderer.materials[1].color = Color.white;
    }

    private void DestroyEnemy()
    {
        //ÀÌÆåÆ® Ãß°¡
        Destroy(gameObject);
        GameManager.Instance.AddCandy(GameManager.Instance.RandomCandy(DropCandyMin, DropCandyMax));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Shovel"))
        {
            TakeDamage(GameManager.Instance.ShovelDmg);
        }

        if(other.CompareTag("SnowBall"))
        {
            TakeDamage(GameManager.Instance.SnowBallDmg);
            StartCoroutine(Freeze());
        }

        if(other.CompareTag("SnowManBall"))
        {
            TakeDamage(GameManager.Instance.TurretDmg);
        }

        if (other.CompareTag("TreeAttack"))
        {
            TakeDamage(GameManager.Instance.TreeStarDmg);
        }
    }

    private IEnumerator Freeze()
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(GameManager.Instance.FreezeTime);
        agent.isStopped = false;
    }
}
