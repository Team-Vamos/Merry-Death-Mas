using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : MonoBehaviour
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


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Ground"))
        {
            
            Debug.Log("맞았다!"+GameManager.Instance.TurretDmg +"의 데미지로");
            enemy.TakeDamage(GameManager.Instance.TurretDmg);
            Destroy(gameObject);
        }
    }
}
