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
    }

    private void Start()
    {
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
}
