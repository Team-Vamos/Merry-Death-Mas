using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField]
    private Transform target;


    public float attackRange;

    public GameObject BulletPre;
    public float AttackDelay;

    public Transform firePoint;

    public float turnSpeed;

    public string EnemyTag = "Enemy";

    public Transform partToRotate;

    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }
    private void Update()
    {
        if (target != null)
        {

        }
        LockOnTarget();
    }
    void LockOnTarget()
    {
        transform.LookAt(target);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(EnemyTag);
        float shortestDis = Mathf.Infinity;
        GameObject nearesetEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if(distanceToEnemy<shortestDis)
            {
                shortestDis = distanceToEnemy;
                nearesetEnemy = enemy;
            }
        }
        if(nearesetEnemy!=null&&shortestDis<=attackRange)
        {
            target = nearesetEnemy.transform;
            Debug.Log("FindTarget");
        }
        else
        {
            target = null;
        }
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(AttackDelay);
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
