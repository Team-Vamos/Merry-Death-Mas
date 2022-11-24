using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : MonoBehaviour
{
    private Rigidbody m_rigidbody = null;

    [SerializeField]
    private float explotion = 2.0f;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        transform.SetParent(null);
        m_rigidbody.AddForce(transform.forward * explotion, ForceMode.Impulse);
    }


    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
