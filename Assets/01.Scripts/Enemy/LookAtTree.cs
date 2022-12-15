using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTree : MonoBehaviour
{
    private EnemyAI target;

    private void Awake()
    {
        target = GetComponentInParent<EnemyAI>();  
    }

    void Update()
    {
        transform.LookAt(target.Target);       
    }
}
