using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DecoObj
{
    public GameObject[] DecorationObj;
}


public class DecorationEnabled : MonoBehaviour
{
    public UpgradeUI upgradeUI;
    public DecoObj[] decorationObj;

    public void DecorationEnable()
    {
        decorationObj[upgradeUI.currentIndex].DecorationObj[upgradeUI.currentLevel].SetActive(true);
    }
}
