using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowPile : PoolingObj
{
    private MeshRenderer meshRenderer = null;
    private float snowStack = 0f;
    
    public override Transform poolManager => GameManager.Instance.snowPoolManager;

    public override int poolChilds => poolManager.childCount;
    private bool isEnable = true;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();    
        meshRenderer.material.SetFloat("_Height", snowStack);
        StartCoroutine(SnowStack());
    }

    private void OnEnable()
    {
        snowStack = 0f;
        meshRenderer.material.SetFloat("_Height", snowStack);
        if(isEnable)StartCoroutine(SnowStack());
    }

    private IEnumerator SnowStack()
    {
        isEnable = false;
        while (snowStack < 5f)
        {
            snowStack += 1f;
            meshRenderer.material.SetFloat("_Height", snowStack);
            yield return new WaitForSeconds(GameManager.Instance.snowPileTime);
        }
        isEnable = true;
    }

    public override void whenDestroy()
    {
        snowStack = 0f;
        StopAllCoroutines();
        transform.SetParent(GameManager.Instance.snowPoolManager);
        gameObject.SetActive(false);
    }
}
