using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentBox : PoolingObj
{
    public override Transform poolManager => GameManager.Instance.presentPoolManager;

    public override int poolChilds => poolManager.childCount;

    public override void whenDestroy()
    {
        int candy = GameManager.Instance.RandomCandy(3, 10);
        GameManager.Instance.AddCandy(candy);
        transform.SetParent(poolManager);
        transform.localPosition = Vector3.zero;

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            whenDestroy();
        }
    }
}
