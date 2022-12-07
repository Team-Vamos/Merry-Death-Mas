using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeTurret : MonoBehaviour
{
    public ParticleSystem StarParticle;

    public float AttackDelay;

    private CapsuleCollider TreeAttackRange;
    
    public Transform firePoint;

    public string EnemyTag = "Enemy";

    private float fireCountDown = 0f;

    private void Start()
    {
        TreeAttackRange = GetComponent<CapsuleCollider>();
    }
    private void Update()
    {
        if (fireCountDown <= 0f)
        {
            TreeAttackRange.enabled = true;
            Instantiate(StarParticle,transform.position,Quaternion.identity);
            fireCountDown = AttackDelay;
            StartCoroutine(EnabledTrigger());
        }

        fireCountDown -= Time.deltaTime;
    }

    IEnumerator EnabledTrigger()
    {
        yield return new WaitForSeconds(1f);
        TreeAttackRange.enabled = false;
    }

}
