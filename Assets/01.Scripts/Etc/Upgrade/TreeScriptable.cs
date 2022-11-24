using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TreeUpgrade
{
    [CreateAssetMenu(fileName ="UpData",menuName ="Scriptable/Create UpData")]
    public class TreeScriptable : ScriptableObject
    {
        public TreeUpgrade[] TreeUpgrade;
    }

    [System.Serializable]
    public class TreeUpgrade
    {

        public string UpName;
        public bool isMaxUp;
        public int NowUpgrade = 0;
        public int MaxUpgrade = 5;
        public UpgradeInfo[] UpgradeLevel;
    }

    [System.Serializable]
    public class UpgradeInfo
    {
        public int BuyCost;
        public int value1;
        public int value2;
        public int value3;
    }
}

