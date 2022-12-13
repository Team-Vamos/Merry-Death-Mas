using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTree : MonoBehaviour
{
    private Transform tree;

    private void Awake()
    {
        tree = FindObjectOfType<EnabledTree>().transform;  
    }

    void Update()
    {
        transform.LookAt(tree);       
    }
}
