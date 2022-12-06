using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("¼±¹° È¹µæ");
            int candy = GameManager.Instance.RandomCandy(3, 10);
            GameManager.Instance.AddCandy(candy);
            
            gameObject.SetActive(false);
        }
    }
}
