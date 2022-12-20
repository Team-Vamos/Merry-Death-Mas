using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (GetComponentInParent<EnemyAI>().isAtk)
            {
                Debug.Log("PlayerAtk");
                other.GetComponent<PlayerController>().GetDmg();
                GetComponentInParent<EnemyAI>().isAtk = false;
            }
        }
        if (other.gameObject.CompareTag("Tree"))
        {
            if (GetComponentInParent<EnemyAI>().isAtk)
            {
                Debug.Log("TreeAtk");
                other.GetComponent<TreeHp>().getDmg();
                GetComponentInParent<EnemyAI>().isAtk = false;
            }
        }
        Destroy(gameObject);
    }
}
