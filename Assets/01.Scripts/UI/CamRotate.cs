using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    [SerializeField]
    float spd = 0.02f;
    void Update()
    {
        transform.Rotate(0, spd, 0);    
    }
}
