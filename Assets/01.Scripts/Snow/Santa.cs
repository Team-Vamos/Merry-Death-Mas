using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Santa : MonoBehaviour
{
    [SerializeField]
    private float spd = 1f;

    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * spd;
        if(transform.position.z > 90)
        {
            GameManager.Instance.ResetSanta(gameObject);
        }
    }
}
