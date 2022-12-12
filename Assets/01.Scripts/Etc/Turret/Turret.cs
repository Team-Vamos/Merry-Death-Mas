using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;

    public float attackRange { get => GameManager.Instance.TurretRange; }

    public GameObject BulletPre;
    public float AttackDelay { get => GameManager.Instance.TurrentDelay; }

    public Transform firePoint;

    public float turnSpeed;

    private string EnemyTag = "Enemy";

    public Transform partToRotate;

    private float fireCountDown = 0f;

    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);

    }
    private void Update()
    {
        if (target != null)
        {
            LockOnTarget();
            if (fireCountDown <= 0f)
            {
                Shoot();
                fireCountDown = AttackDelay;
            }
            fireCountDown -= Time.deltaTime;
        }
    }
    void LockOnTarget()
    {
        partToRotate.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
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
        rb.AddForce(firePoint.forward * 10f, ForceMode.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
