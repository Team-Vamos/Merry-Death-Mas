using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public TreeScriptable UpgradeData;
    public Image UpgradeImage;


    public Text ItemLevel, ItemName, UpgradeCostText;
    public Button BuyButton;
    [SerializeField]
    private bool IsBuy;

    private int value1, value2, value3;

    public int currentIndex = 0;
    public int currentLevel = 0;

    public Transform Deco = null;
    private List<GameObject> Decoration = new List<GameObject>();

    private TreeUpgrade upgradeInfo { get => UpgradeData.TreeUpgrade[currentIndex]; }
    private void Start()
    {
        UpdateValues();
        UpdateLevelInfo();
    }

    private void OnValidate()
    {
        if (Deco != null)
        {
            for (int i = 0; i < Deco.childCount; i++)
            {
                Decoration.Add(Deco.GetChild(i).gameObject);
            }
        }
    }

    private void Update()
    {
        if (GameManager.Instance.getCandy <= upgradeInfo.UpgradeLevel[currentLevel].BuyCost)
        {
            BuyButton.interactable = false;
        }
        else
        {
            BuyButton.interactable = true;
        }

        if (currentLevel == upgradeInfo.MaxUpgrade)
        {
            BuyButton.enabled = false;
            UpgradeCostText.text = "MAX".ToString();
        }
    }

    public void DecoEnable()
    {
        Decoration[currentLevel].SetActive(true);
    }

    public void NextLevel()
    {
        if (currentLevel < upgradeInfo.MaxUpgrade)
        {
            currentLevel++;
            UpdateValues();
            UpdateLevelInfo();
        }
    }

    void UpdateValues()
    {
        value1 = upgradeInfo.UpgradeLevel[currentLevel].value1;
        value2 = upgradeInfo.UpgradeLevel[currentLevel].value2;
        value3 = upgradeInfo.UpgradeLevel[currentLevel].value3;
        if (currentLevel >= upgradeInfo.MaxUpgrade)
        {

            BuyButton.interactable = false;
            ItemLevel.text = "Level : " + currentLevel;
            upgradeInfo.isMaxUp = true;
        }
    }

    void UpdateLevelInfo()
    {
        UpgradeImage.sprite = upgradeInfo.ItemImage;
        ItemName.text = upgradeInfo.UpName;
        ItemLevel.text = "Level : " + currentLevel;
        UpgradeCostText.text = upgradeInfo.UpgradeLevel[currentLevel].BuyCost.ToString();
    }

    public void CandyCane()
    {
        GameManager.Instance.MultiplyShovelDmg(value1);
        Debug.Log(-upgradeInfo.UpgradeLevel[currentLevel].BuyCost);
        if(GameManager.Instance.getCandy > upgradeInfo.UpgradeLevel[currentLevel].BuyCost)
        GameManager.Instance.AddCandy(-upgradeInfo.UpgradeLevel[currentLevel].BuyCost);
    }

    public void Turr(int value1)
    {

    }

    public void SnowMan()
    {
        if(currentLevel<=0)
        {
            GameManager.Instance.SnowManObj.SetActive(true);
        }
        GameManager.Instance.TurretDmg = value1;
        if (GameManager.Instance.getCandy > upgradeInfo.UpgradeLevel[currentLevel].BuyCost)
            GameManager.Instance.AddCandy(-upgradeInfo.UpgradeLevel[currentLevel].BuyCost);
    }

    public void Star()
    {
        if(currentLevel<=0)
        {
            GameManager.Instance.StarUpgrade.SetActive(true);
        }
        GameManager.Instance.StarDmg = value1;
        if (GameManager.Instance.getCandy > upgradeInfo.UpgradeLevel[currentLevel].BuyCost)
            GameManager.Instance.AddCandy(-upgradeInfo.UpgradeLevel[currentLevel].BuyCost);

    }
}