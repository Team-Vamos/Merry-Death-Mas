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
        public int currentLevel = 0;
        private void Start()
        {
            currentLevel = UpgradeData.TreeUpgrade[currentIndex].NowUpgrade;
            ItemName.text = UpgradeData.TreeUpgrade[currentIndex].UpName;
            ItemLevel.text = "Level : "+currentLevel;
            UpgradeCostText.text =  UpgradeData.TreeUpgrade[currentIndex].UpgradeLevel[currentLevel].BuyCost.ToString();
        }

        public void NextLevel()
        {
            if (currentLevel < UpgradeData.TreeUpgrade[currentIndex].MaxUpgrade)
            {
                currentLevel++;
                ItemName.text = UpgradeData.TreeUpgrade[currentIndex].UpName;
                ItemLevel.text = "Level : " + currentLevel;
                UpgradeCostText.text = UpgradeData.TreeUpgrade[currentIndex].UpgradeLevel[currentLevel].BuyCost.ToString();
            }
            else
            {
                BuyButton.interactable = false;
            }
        }
    }

}

