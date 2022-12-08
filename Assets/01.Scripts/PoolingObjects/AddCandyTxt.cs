using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCandyTxt : PoolingObj
{
    TextFade textFade;

    public override Transform poolManager => GameManager.Instance.addCandyPool;

    public override int poolChilds => poolManager.childCount;

    private void Awake()
    {
        textFade = GetComponent<TextFade>();
    }

    public override void whenDestroy()
    {
        transform.SetParent(poolManager);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        textFade.StartFade(gameObject, true, 3f, whenDestroy);
    }
}
