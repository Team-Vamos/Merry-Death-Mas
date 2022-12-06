using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeTurret : MonoBehaviour
{
    [SerializeField]
    private Transform target;


    public float attackRange;



    public GameObject BulletPre;
    public float AttackDelay;

    

    public Transform firePoint;

    public string EnemyTag = "Enemy";

    private float fireCountDown = 0f;

    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }
    private void Update()
    {
        LockOnTarget();
        if (fireCountDown <= 0f)
        {
            Shoot();
            fireCountDown = AttackDelay;
        }
        fireCountDown -= Time.deltaTime;
    }
    void LockOnTarget()
    {
        firePoint.LookAt(new Vector3(target.position.x, transform.position.y - 10f, target.position.z));
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(EnemyTag);
        float shortestDis = Mathf.Infinity;
        GameObject nearesetEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDis)
            {
                shortestDis = distanceToEnemy;
                nearesetEnemy = enemy;
            }
        }
        if (nearesetEnemy != null && shortestDis <= attackRange)
        {
            target = nearesetEnemy.transform;
            Debug.Log("FindTarget");
        }
        else
        {
            target = null;
        }
    }

    void Shoot()
    {
        Rigidbody rb = Instantiate(BulletPre, firePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
