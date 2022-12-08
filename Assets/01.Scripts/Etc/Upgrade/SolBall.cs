using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolBall : MonoBehaviour
{
    public float AttackDelay = 0.5f;
    private float FireCount = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FireCount <= 0f)
        {
            
        }

        FireCount -= Time.deltaTime;
    }
    void Attack()
    {

    }
}
