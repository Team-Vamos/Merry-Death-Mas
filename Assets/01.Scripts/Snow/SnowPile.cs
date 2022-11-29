using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowPile : MonoBehaviour
{
    private MeshRenderer meshRenderer = null;
    private float snowStack = 0f;
    
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();    
        StartCoroutine(SnowStack());
    }

    private void OnEnable()
    {
        snowStack = 0f;
        StartCoroutine(SnowStack());
    }

    private void OnDisable()
    {
        snowStack = 0f;
        StopAllCoroutines();
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
}
