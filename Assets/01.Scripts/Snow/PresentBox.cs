using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("���� ȹ��");
            int candy = GameManager.Instance.RandomCandy(3, 10);
            GameManager.Instance.AddCandy(candy);
            
            gameObject.SetActive(false);
        }
    }
}
