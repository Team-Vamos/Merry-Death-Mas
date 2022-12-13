using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAtk : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(GetComponentInParent<EnemyAI>().isAtk)
            {
                other.GetComponent<PlayerController>().GetDmg();
                GetComponentInParent<EnemyAI>().isAtk = false;
            }
        }
        if(other.gameObject.CompareTag("Tree"))
        {
            if(GetComponentInParent<EnemyAI>().isAtk)
            {
                other.GetComponent<TreeHp>().getDmg();
                GetComponentInParent<EnemyAI>().isAtk = false;
            }
        }
    }
}
