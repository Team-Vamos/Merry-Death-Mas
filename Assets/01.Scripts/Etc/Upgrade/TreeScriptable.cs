using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "UpData", menuName = "Scriptable/Create UpData")]
public class TreeScriptable : ScriptableObject
{
    public TreeUpgrade[] TreeUpgrade;
}

[System.Serializable]
public class TreeUpgrade
{

    public string UpName;
    public Sprite ItemImage;
    public bool isMaxUp;
    public int MaxUpgrade;

    [TextArea]
    public string itemDesc;

    public UpgradeInfo[] UpgradeLevel;
}

[System.Serializable]
public class UpgradeInfo
{
    public int BuyCost;
    public int value1;
    public float value2;
    public float value3;
}