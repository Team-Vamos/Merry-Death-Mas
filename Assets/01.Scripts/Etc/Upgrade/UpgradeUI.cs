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
    public GameObject[] Decoration;


    private void Start()
    {
        UpdateValues();
        UpdateLevelInfo();
    }

    private void Update()
    {
        if (GameManager.Instance.candy <= UpgradeData.TreeUpgrade[currentIndex].UpgradeLevel[currentLevel].BuyCost)
        {
            BuyButton.interactable = false;
        }
        else
        {
            BuyButton.interactable = true;
        }

        if (currentLevel == UpgradeData.TreeUpgrade[currentIndex].MaxUpgrade)
        {
            UpgradeCostText.text = "MAX".ToString();
        }
        Debug.Log("CurIndx: "+currentIndex + "        " + "CurLv: "+currentLevel+"     "+"Value1: "+value1);
    }

    public void DecoEnable()
    {
        Decoration[currentLevel].SetActive(true);
    }

    public void NextLevel()
    {
        if (currentLevel < UpgradeData.TreeUpgrade[currentIndex].MaxUpgrade)
        {
            currentLevel++;
            UpdateValues();
            UpdateLevelInfo();
        }
    }

    void UpdateValues()
    {
        value1 = UpgradeData.TreeUpgrade[currentIndex].UpgradeLevel[currentLevel].value1;
        value2 = UpgradeData.TreeUpgrade[currentIndex].UpgradeLevel[currentLevel].value2;
        value3 = UpgradeData.TreeUpgrade[currentIndex].UpgradeLevel[currentLevel].value3;
        if (currentLevel >= UpgradeData.TreeUpgrade[currentIndex].MaxUpgrade)
        {

            BuyButton.interactable = false;
            ItemLevel.text = "Level : " + currentLevel;
            UpgradeData.TreeUpgrade[currentIndex].isMaxUp = true;
        }
    }

    void UpdateLevelInfo()
    {
        UpgradeImage.sprite = UpgradeData.TreeUpgrade[currentIndex].ItemImage;
        ItemName.text = UpgradeData.TreeUpgrade[currentIndex].UpName;
        ItemLevel.text = "Level : " + currentLevel;
        UpgradeCostText.text = UpgradeData.TreeUpgrade[currentIndex].UpgradeLevel[currentLevel].BuyCost.ToString();


    }

    public void CandyCane()
    {
        Debug.Log("Value 1 : " + value1);
        if (currentLevel <= 0)
        {
            Debug.Log("������ ����");
            GameManager.Instance.MultiplyShovelDmg(value1);
            Debug.Log("�� ���ݷ� : "+GameManager.Instance.ShovelDmg);
        }
        else
        {
            GameManager.Instance.MultiplyShovelDmg(value1);
            Debug.Log("ĵ�� ��ȭ");
            Debug.Log("�� ���ݷ� : " + GameManager.Instance.ShovelDmg);
        }

    }

    public void Turr(int value1)
    {

    }

    public void Star()
    {

    }
}