using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.VFX;

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

[RequireComponent(typeof(AudioSource))]
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

    public Transform BulletPosition;

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

    private bool isballDmg;
    private float BallDmgCounting = 0f;
    private float BallDmgDelay;

    public bool isAtk = false;
    private bool stop = false;

    [SerializeField]
    Renderer _renderer;

    [SerializeField]
    private Collider atkCollider;

    [SerializeField]
    VisualEffect visualEffect;

    [SerializeField]
    private HpText hpTxt;

    private Color OriginColor;

    [SerializeField]
    private AudioSource audio;

    private void Start()
    {
        health = MaxHealth;
        audio = GetComponent<AudioSource>();
        FindTarget();
        BallDmgDelay = GameManager.Instance.BallAtkDelay;
        OriginColor = _renderer.material.color;
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(isballDmg == true)
        {
            if (BallDmgDelay <= 0f)
            {
                isballDmg = false;
                BallDmgDelay = GameManager.Instance.BallAtkDelay;
            }
            BallDmgDelay -= Time.deltaTime;
        }

        if(health>=MaxHealth)
        {
            healthBar.gameObject?.SetActive(false);
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
                Target = GameObject.Find("Player")?.transform;
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
        transform.LookAt(Target.position);
        anim.SetBool("ToTarget", true);

    }

    private void AttackPlayer()
    { 
        agent.SetDestination(transform.position);
        anim.SetBool("ToTarget", false);

        if (!alreadyAttacked)
        {
            switch(enemyAtkType)
            {
                case EnemyAtkType.longRange:
                    isAtk = true;
                    Rigidbody rb = Instantiate(projectile, BulletPosition.position, Quaternion.identity).GetComponent<Rigidbody>();
                    rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
                    anim.CrossFade("Attack", 0.05f);
                    break;
                case EnemyAtkType.shortRange:
                    isAtk = true;
                    anim.CrossFade("Attack", 0.05f);
                    break;
            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public void EffectOn()
    {
        visualEffect.Play();
        atkCollider.enabled = true;
    }

    public void EffectOff()
    {
        atkCollider.enabled = false;
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        StartCoroutine(Hit());
        if (health <= 0)
        {
            anim.CrossFade("Death", 0.05f);
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }

    private IEnumerator Hit()
    {
        _renderer.material.color = Color.red;
        audio.clip = GameManager.Instance.randomHitSound();
        audio.Play();
        yield return new WaitForSeconds(0.1f);
        hpTxt.SetHpText((int)health, (int)MaxHealth);
        _renderer.material.color = OriginColor;
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
            TakeDamage(GameManager.Instance.StarDmg);
        }
        if(other.CompareTag("MasBall")&&isballDmg == false)
        {
            TakeDamage(GameManager.Instance.BallDmg);
            isballDmg = true;
        }
    }

    private IEnumerator Freeze()
    {
        stop = true;
        yield return new WaitForSeconds(GameManager.Instance.FreezeTime);
        stop = false;
    }
}
