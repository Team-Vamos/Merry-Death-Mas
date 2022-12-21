using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    private UpgradeInfoPanel infoPanel;
    public TreeScriptable UpgradeData;
    public Image UpgradeImage;

    [Header("====�ɷ�====")]
    private TreeTurret treeTurret;
    



    public Text ItemLevel, ItemName, UpgradeCostText;
    public Button BuyButton;
    [SerializeField]
    private bool IsBuy;

    private float value1 ,value2, value3;

    public int currentIndex = 0;
    public int currentLevel = 0;

    public Transform Deco;
    private List<GameObject> Decoration = new List<GameObject>();

    private TreeUpgrade upgradeInfo { get => UpgradeData.TreeUpgrade[currentIndex]; }
    private void Start()
    {
        if(Deco != null)
        {
            for (int i = 0; i < Deco.childCount; i++)
            {
                Decoration.Add(Deco.GetChild(i).gameObject);
            }
        }

        infoPanel = GetComponent<UpgradeInfoPanel>();
        UpdateValues();
        UpdateLevelInfo();
        UpdateStatInfo();
    }

    private void Update()
    {
        if (GameManager.Instance.getCandy < upgradeInfo.UpgradeLevel[currentLevel].BuyCost)
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

    public void CandyCane() //�Ѵ�
    {
        GameManager.Instance.MultiplyShovelDmg(value1);
        if(GameManager.Instance.getCandy >= upgradeInfo.UpgradeLevel[currentLevel].BuyCost)
        GameManager.Instance.AddCandy(-upgradeInfo.UpgradeLevel[currentLevel].BuyCost);

        if (currentLevel + 1 == upgradeInfo.MaxUpgrade)
        {
            infoPanel.text_ItemStat.text = $"���ݷ� {value1}";
        }
        else
        {
            infoPanel.text_ItemStat.text = $"���ݷ� {value1} -> {upgradeInfo.UpgradeLevel[currentLevel+1].value1}";
        }
    }
    
    void UpdateStatInfo()
    {
        switch(currentIndex)
        {
            case 0: //���� - ���ݷ�
                infoPanel.text_ItemStat.text = $"���ݷ� {value1}";
                break;
            case 1: //���� - ���ݼӵ�
                infoPanel.text_ItemStat.text = $"���ݼӵ� {value1}";
                break;
            case 2: //ũ�������� �� - ���ݷ�, ȸ�� �ӵ�, �� ����
                infoPanel.text_ItemStat.text = $"���ݷ� + {value1}\n�� ���� + {value3}";
                break;
            case 3: //���� - ������ ������
                infoPanel.text_ItemStat.text = $"���ݷ� + {value1}%\n";
                break;
            case 4: //������ ����- ü��
                infoPanel.text_ItemStat.text = $"ü�� + {value1}";
                break;
            case 5: //�� - ���� ����
                infoPanel.text_ItemStat.text = $"Ʈ���� ���� ����\n ���ݷ� + {value1}\n ���� �ֱ� {value2}�� ����";
                break;
            case 6: //����� �� - ��ž ��ġ
                infoPanel.text_ItemStat.text = $"����� �߰�\n ���ݷ� + {value1}\n ���� �ֱ� {value2}�� ����\n ���ݹ��� +{value3}";
                break;
        }
    }

    public void Candle() //�Ѵ�
    {
        GameManager.Instance.SnowBallDmg += (int)value1;
        if (GameManager.Instance.getCandy >= upgradeInfo.UpgradeLevel[currentLevel].BuyCost)
            GameManager.Instance.AddCandy(-upgradeInfo.UpgradeLevel[currentLevel].BuyCost);

        if (currentLevel + 1 == upgradeInfo.MaxUpgrade)
        {
            infoPanel.text_ItemStat.text = $"���ݷ� + {value1}%";
        }
        else
        {
            infoPanel.text_ItemStat.text = $"���ݷ� + {value1}% -> {upgradeInfo.UpgradeLevel[currentLevel + 1].value1}%\n";
        }
        

    }

    public void HotHeart() //�Ѵ�
    {
        GameManager.Instance.playerMaxHp += (int)value1;
        GameManager.Instance.playerHp++;
        if (GameManager.Instance.getCandy >= upgradeInfo.UpgradeLevel[currentLevel].BuyCost)
            GameManager.Instance.AddCandy(-upgradeInfo.UpgradeLevel[currentLevel].BuyCost);

        if (currentLevel+1==upgradeInfo.MaxUpgrade)
        {
            infoPanel.text_ItemStat.text = $"ü�� + {value1}%";
        }
        else
        {
            infoPanel.text_ItemStat.text = $"ü�� + {value1}% -> {upgradeInfo.UpgradeLevel[currentLevel + 1].value1}%\n";
        }

    }

    public void ChristmasBall()
    {
        if(currentLevel<=0)
        {
            GameManager.Instance.BallSize = 3;
        }
        GameManager.Instance.BallDmg = value1;
        GameManager.Instance.BallAtkDelay = value2;
        GameManager.Instance.BallSize = (int)value3;
        if (GameManager.Instance.getCandy >= upgradeInfo.UpgradeLevel[currentLevel].BuyCost)
            GameManager.Instance.AddCandy(-upgradeInfo.UpgradeLevel[currentLevel].BuyCost);

        if (currentLevel + 1 == upgradeInfo.MaxUpgrade)
        {
            infoPanel.text_ItemStat.text =
                $"���ݷ� + {value1}\n" +
                $"�� ���� + {value3}";
        }
        else
        {
            infoPanel.text_ItemStat.text =
                $"���ݷ� + {value1} -> {upgradeInfo.UpgradeLevel[currentLevel + 1].value1}\n" +
                $"�� ���� + {value3} -> {upgradeInfo.UpgradeLevel[currentLevel + 1].value3}";
        }


    }

    public void SnowMan() //�Ѵ�
    {
        if(currentLevel<=0)
        {
            GameManager.Instance.SnowManObj.SetActive(true);
        }

        GameManager.Instance.TurretDmg += value1;
        GameManager.Instance.TurrentDelay = value2;
        GameManager.Instance.TurretRange = value3;
        if (GameManager.Instance.getCandy >= upgradeInfo.UpgradeLevel[currentLevel].BuyCost)
            GameManager.Instance.AddCandy(-upgradeInfo.UpgradeLevel[currentLevel].BuyCost);

        if (currentLevel + 1 == upgradeInfo.MaxUpgrade)
        {
            infoPanel.text_ItemStat.text = $"���ݷ� + {value1}\n�߻��ֱ� {value2}�� ����\n���ݹ��� + {value3}";
        }
        else
        {
            infoPanel.text_ItemStat.text =
                $"���ݷ� + {value1} -> ���ݷ� + {upgradeInfo.UpgradeLevel[currentLevel + 1].value1}\n" +
                $"�߻��ֱ� {value2}�� ���� -> �߻��ֱ� {upgradeInfo.UpgradeLevel[currentLevel + 1].value2}�� ����\n" +
                $"���ݹ��� {value3} -> ���ݹ��� + {upgradeInfo.UpgradeLevel[currentLevel +1].value3}";
        }

    }

    public void Gift() //�Ѥ���
    {
        //���� �ӵ� ���� ���׷��̵�
        GameManager.Instance.playerAtkSpd = value1;
        if (GameManager.Instance.getCandy >= upgradeInfo.UpgradeLevel[currentLevel].BuyCost)
            GameManager.Instance.AddCandy(-upgradeInfo.UpgradeLevel[currentLevel].BuyCost);

        if (currentLevel + 1 == upgradeInfo.MaxUpgrade)
        {
            infoPanel.text_ItemStat.text = $"���ݼӵ� {value1}";
        }
        else
        {
            infoPanel.text_ItemStat.text = $"���ݼӵ� {value1} -> {upgradeInfo.UpgradeLevel[currentLevel + 1].value1}";
        }
    }

    public void Star()
    {
        if(currentLevel<=0)
        {
            GameManager.Instance.StarUpgrade.SetActive(true);
        }
        GameManager.Instance.StartDelay = value2;
        GameManager.Instance.StarDmg = value1;
        if (GameManager.Instance.getCandy >= upgradeInfo.UpgradeLevel[currentLevel].BuyCost)
            GameManager.Instance.AddCandy(-upgradeInfo.UpgradeLevel[currentLevel].BuyCost);
        if (currentLevel + 1 == upgradeInfo.MaxUpgrade)
        {
            infoPanel.text_ItemStat.text = $"Ʈ���� ���� ����\n ���ݷ� + {value1}\n���� �ֱ� {upgradeInfo.UpgradeLevel[currentLevel].value2}";
        }
        else
        {
            infoPanel.text_ItemStat.text = $"���ݷ� + {value1} -> {upgradeInfo.UpgradeLevel[currentLevel + 1].value1}\n���� �ֱ� {value2} -> {upgradeInfo.UpgradeLevel[currentLevel+1].value2}";
        }
    }
}