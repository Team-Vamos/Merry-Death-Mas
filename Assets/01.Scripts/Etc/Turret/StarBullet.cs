using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBullet : MonoBehaviour
{
private Rigidbody m_rigidbody = null;

    private Turret turret;

    private EnemyAI enemy;

    [SerializeField]
    private float explotion;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        transform.SetParent(null);
        m_rigidbody.AddForce(transform.forward * explotion, ForceMode.Impulse);
    }


    private void OnCollisionEnter(Collision collision)
    {
        enemy.TakeDamage(turret.attackDmg);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            enemy.TakeDamage(turret.attackDmg);
            Destroy(gameObject);
        }

    }
}
