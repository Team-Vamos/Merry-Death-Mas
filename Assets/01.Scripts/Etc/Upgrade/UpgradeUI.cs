using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TreeUpgrade
{
    public class UpgradeUI : MonoBehaviour
    {
        public TreeScriptable UpgradeData;
        public Image UpgradeImage;
        public Text ItemLevel, ItemName, UpgradeCostText;
        public Button BuyButton;

        public int currentIndex = 0;
        private void Start()
        {
            ItemName.text = UpgradeData.TreeUpgrade[currentIndex].UpName;
            ItemLevel.text = "Level : "+UpgradeData.TreeUpgrade[currentIndex].NowUpgrade;
            UpgradeCostText.text = "Candy : " + UpgradeData.TreeUpgrade[currentIndex].UpgradeLevel[UpgradeData.TreeUpgrade[currentIndex].NowUpgrade].BuyCost;
        }
    }

}

