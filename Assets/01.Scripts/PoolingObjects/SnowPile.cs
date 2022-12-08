using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowPile : PoolingObj
{
    private MeshRenderer meshRenderer = null;
    private float snowStack = 0f;
    
    public override Transform poolManager => GameManager.Instance.snowPoolManager;

    public override int poolChilds => poolManager.childCount;

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
        StartCoroutine(SnowStack());
    }

    private IEnumerator SnowStack()
    {
        while (snowStack < 5f)
        {
            meshRenderer.material.SetFloat("_Height", snowStack);
            snowStack += 1f;
            yield return new WaitForSeconds(GameManager.Instance.snowPileTime);
        }
    }

    public override void whenDestroy()
    {
        snowStack = 0f;
        StopAllCoroutines();
        transform.SetParent(GameManager.Instance.snowPoolManager);
        gameObject.SetActive(false);
    }
}
