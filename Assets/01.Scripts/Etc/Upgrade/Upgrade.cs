using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public Transform Tree;

    public Canvas UpgradeCanvas;

    public Image GuideImage;

    public float radius;

    public UpgradeData[] Data;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] col = Physics.OverlapSphere(Tree.position, radius);
        
        foreach(Collider co in col)
        {
            if(co.CompareTag("Player"))
            {
                GuideImage.enabled = true;
                if(Input.GetKeyDown(KeyCode.E))
                {
                    UpgradeCanvas.enabled = true;
                }
            }
            else
            {
                GuideImage.enabled = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Tree.position, radius);
    }
}
