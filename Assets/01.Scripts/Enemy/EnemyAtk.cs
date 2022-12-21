using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAtk : MonoBehaviour
{
    public bool isBullet = false;
    private bool atkAble = true;
    private void OnTriggerEnter(Collider other)
    {
        if(!isBullet)
        {
                isBullet = false;
                atkAble = GetComponentInParent<EnemyAI>().isAtk;
        }
        
        if (other.gameObject.CompareTag("Player"))
        {
            if(GetComponentInParent<EnemyAI>().isAtk)
            {
                Debug.Log("PlayerAtk");
                other.GetComponent<PlayerController>().GetDmg();
                if(!isBullet)GetComponentInParent<EnemyAI>().isAtk = false;
            }
        }
        if(other.gameObject.CompareTag("Tree"))
        {
            if(!isBullet) atkAble = GetComponentInParent<EnemyAI>().isAtk;
            if (atkAble)
                {
                Debug.Log("TreeAtk");
                other.GetComponent<TreeHp>()?.getDmg();
                if (isBullet) Destroy(gameObject);
                else GetComponentInParent<EnemyAI>().isAtk = false;
            }
        }
    }
}
