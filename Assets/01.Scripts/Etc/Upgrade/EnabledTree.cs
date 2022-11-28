using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnabledTree : MonoBehaviour
{
    public Canvas WaveCanvas;
    public Canvas TreeCanvas;
    public Canvas GuideCanvas;

    public float Range;
    public float TreeCoolTime;
    private float Timer;

    private bool Enabled = false;
    private bool playerInTree;
    private bool IsOpenTree = true;

    public LayerMask PlayerLayer;

    private void Awake()
    {
        TreeCanvas.enabled = false;
        GuideCanvas.enabled = false;
        Timer = TreeCoolTime;
    }

    private void Update()
    {
        playerInTree = Physics.CheckSphere(transform.position, Range, PlayerLayer);
        EnabledUpgradeUI();

        if (Timer<=0f)
        {

            IsOpenTree = true;
        }
        else if (!IsOpenTree)
        {

            Timer -= Time.deltaTime;
        }

    }

    void EnabledUpgradeUI()
    {
        if(playerInTree)
        {
            GuideCanvas.enabled = true;
            if(Input.GetKeyDown(KeyCode.E)&&Enabled == false&&IsOpenTree)
            {
                Time.timeScale = 0f;
                WaveCanvas.enabled = false;
                TreeCanvas.enabled = true;
                Enabled = true;
                IsOpenTree = false;
                Timer = TreeCoolTime;
            }
            else if(Input.GetKeyDown(KeyCode.E)&&Enabled == true)
            {
                Time.timeScale = 1f;
                WaveCanvas.enabled = true;
                TreeCanvas.enabled = false;
                Enabled = false;
            }
        }
        else
        {
            GuideCanvas.enabled = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
