using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    private UpgradeInfoPanel infoPanel;
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
        infoPanel = GetComponent<UpgradeInfoPanel>();
        UpdateValues();
        UpdateLevelInfo();
        UpdateStatInfo();
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
        infoPanel.text_ItemName.text = upgradeInfo.UpName;
        infoPanel.text_ItemDesc.text = upgradeInfo.itemDesc;

    }

    public void CandyCane()
    {
        GameManager.Instance.MultiplyShovelDmg(value1);
        Debug.Log(-upgradeInfo.UpgradeLevel[currentLevel].BuyCost);
        if(GameManager.Instance.getCandy > upgradeInfo.UpgradeLevel[currentLevel].BuyCost)
        GameManager.Instance.AddCandy(-upgradeInfo.UpgradeLevel[currentLevel].BuyCost);

        if (currentLevel + 1 == upgradeInfo.MaxUpgrade)
        {
            infoPanel.text_ItemStat.text = $"���ݷ� +{upgradeInfo.UpgradeLevel[currentLevel].value1}%";
        }
        else
        {
            infoPanel.text_ItemStat.text = $"���ݷ� +{upgradeInfo.UpgradeLevel[currentLevel].value1}% -> {upgradeInfo.UpgradeLevel[currentLevel + 1].value1}%";
        }
    }
    
    void UpdateStatInfo()
    {
        switch(currentIndex)
        {
            case 0: //���� - ���ݷ�
                infoPanel.text_ItemStat.text = $"���ݷ� +{upgradeInfo.UpgradeLevel[currentLevel].value1}%";
                break;
            case 1: //���� - ���ݼӵ�
                infoPanel.text_ItemStat.text = $"���ݼӵ� +{upgradeInfo.UpgradeLevel[currentLevel].value1}%";
                break;
            case 2: //ũ�������� �� - ���ݷ�, ȸ�� �ӵ�, �� ����
                infoPanel.text_ItemStat.text = $"���ݷ� +{upgradeInfo.UpgradeLevel[currentLevel].value1}%\nȸ�� �ӵ� + { upgradeInfo.UpgradeLevel[currentLevel].value2}%\nȸ�� �ӵ� + { upgradeInfo.UpgradeLevel[currentLevel].value3}%";
                break;
            case 3: //���� - ������ ������
                infoPanel.text_ItemStat.text = $"���ݷ� +{upgradeInfo.UpgradeLevel[currentLevel].value1}%\n";
                break;
            case 4: //������ ����- ü��
                infoPanel.text_ItemStat.text = $"ü�� +{upgradeInfo.UpgradeLevel[currentLevel].value1}%";
                break;
            case 5:
                infoPanel.text_ItemStat.text = $"Ʈ���� ���� ����\n ���ݷ� +{upgradeInfo.UpgradeLevel[currentLevel].value1}\n���� �ֱ� +{upgradeInfo.UpgradeLevel[currentLevel].value1}";
                break;
            case 6:
                infoPanel.text_ItemStat.text = $"����� �߰�\n ���ݷ� +{upgradeInfo.UpgradeLevel[currentLevel].value1}";
                break;
        }
    }

    public void Candle()
    {
        infoPanel.text_ItemStat.text = $"���ݷ� +{upgradeInfo.UpgradeLevel[currentLevel].value1}% -> {upgradeInfo.UpgradeLevel[currentLevel + 1].value1}%\n";

    }

    public void HotHeart()
    {
        infoPanel.text_ItemStat.text = $"ü�� +{upgradeInfo.UpgradeLevel[currentLevel].value1}% -> {upgradeInfo.UpgradeLevel[currentLevel + 1].value1}%\n";

    }

    public void ChristmasBall()
    {
        infoPanel.text_ItemStat.text = $"���ݷ� +{upgradeInfo.UpgradeLevel[currentLevel].value1}% -> {upgradeInfo.UpgradeLevel[currentLevel + 1].value1}%\nȸ�� �ӵ� + { upgradeInfo.UpgradeLevel[currentLevel].value2}% -> {upgradeInfo.UpgradeLevel[currentLevel + 1].value2}%\nȸ�� �ӵ� + { upgradeInfo.UpgradeLevel[currentLevel].value3}% -> {upgradeInfo.UpgradeLevel[currentLevel + 1].value3}%";

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

        infoPanel.text_ItemStat.text = $"���ݷ� +{upgradeInfo.UpgradeLevel[currentLevel].value1} -> ���ݷ� +{upgradeInfo.UpgradeLevel[currentLevel+1].value1}";
    }

    public void Gift()
    {
        //���� �ӵ� ���� ���׷��̵�
        infoPanel.text_ItemStat.text = $"���ݼӵ� +{upgradeInfo.UpgradeLevel[currentLevel].value1}% -> {upgradeInfo.UpgradeLevel[currentLevel + 1].value1}%";

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