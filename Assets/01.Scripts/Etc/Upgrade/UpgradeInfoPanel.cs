using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeInfoPanel : MonoBehaviour
{
    private UpgradeUI upgradeUI;

    [SerializeField]
    private GameObject upgradeInfoPanel;

    private Collider InfoCollider;


    private Camera MainCamera;

    public Text text_ItemName;

    public Text text_ItemDesc;

    public Text text_ItemStat;


    private bool IsMouseOver;

    // Start is called before the first frame update
    void Start()
    {
        MainCamera = FindObjectOfType<Camera>();
        InfoCollider = GetComponent<Collider>();
        upgradeUI = GetComponent<UpgradeUI>();
    }

    // Update is called once per frame
    void Update()
    {
        //var ray = Input.mousePosition;
        //RaycastHit hit;
        //Physics.Raycast(ray,transform.forward*100f,out hit);

        

        //if(hit.collider!=null && hit.collider == InfoCollider&&!IsMouseOver)
        //{
        //    IsMouseOver = true;
        //    OnMouseEnter();
        //}
        //else if(hit.collider !=InfoCollider && IsMouseOver)
        //{
        //    IsMouseOver = false;
        //    OnMouseExit();
        //}
    }


    public void OnMouseEnter()
    {
        upgradeInfoPanel.SetActive(true);
    }

    public void OnMouseExit()
    {
        upgradeInfoPanel.SetActive(false);
    }
    private void OnDrawGizmos()
    {        
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Input.mousePosition,transform.forward*100f);
    }
}
