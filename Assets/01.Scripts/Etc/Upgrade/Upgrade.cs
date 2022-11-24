using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public Transform Tree;

    public Canvas UpgradeCanvas;

    public Canvas GuideImage;

    private bool Enabled = false;

    private bool IsEnabledRange;

    public float radius;

    public LayerMask whatIsPlayer;

    public UpgradeData[] Data;

    // Start is called before the first frame update
    void Awake()
    {
        UpgradeCanvas.enabled = false;
        GuideImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        IsEnabledRange = Physics.CheckSphere(transform.position, radius, whatIsPlayer);

        if (IsEnabledRange)
        {
            Debug.Log("EOejfoi");
            GuideImage.enabled = true;
            if(Input.GetKeyDown(KeyCode.E)&&Enabled==false)
            {
                UpgradePanelEnabled(0f, true, true);

            }
            else if(Enabled==true&&Input.GetKeyDown(KeyCode.E))
            {
                UpgradePanelEnabled(1f, false, false);
            }
        }
        else
        {
            GuideImage.enabled = false;
        }
    }

    void UpgradePanelEnabled(float timeScale,bool UpgradeCanvasenabled,bool Enabledcheck)
    {
        Time.timeScale = timeScale;

        UpgradeCanvas.enabled = UpgradeCanvasenabled;
        Enabled = Enabledcheck;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Tree.position, radius);
    }
}
