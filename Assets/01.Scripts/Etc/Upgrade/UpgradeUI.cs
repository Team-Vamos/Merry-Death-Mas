using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    private UpgradeInfoPanel infoPanel;
    public TreeScriptable UpgradeData;
    public Image UpgradeImage;

    [Header("====능력====")]
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

    public void CandyCane() //됏다
    {
        GameManager.Instance.MultiplyShovelDmg(value1);
        if(GameManager.Instance.getCandy >= upgradeInfo.UpgradeLevel[currentLevel].BuyCost)
        GameManager.Instance.AddCandy(-upgradeInfo.UpgradeLevel[currentLevel].BuyCost);

        if (currentLevel + 1 == upgradeInfo.MaxUpgrade)
        {
            infoPanel.text_ItemStat.text = $"공격력 {value1}";
        }
        else
        {
            infoPanel.text_ItemStat.text = $"공격력 {value1} -> {upgradeInfo.UpgradeLevel[currentLevel+1].value1}";
        }
    }
    
    void UpdateStatInfo()
    {
        switch(currentIndex)
        {
            case 0: //사탕 - 공격력
                infoPanel.text_ItemStat.text = $"공격력 {value1}";
                break;
            case 1: //선물 - 공격속도
                infoPanel.text_ItemStat.text = $"공격속도 {value1}";
                break;
            case 2: //크리스마스 볼 - 공격력, 회전 속도, 볼 갯수
                infoPanel.text_ItemStat.text = $"공격력 + {value1}\n볼 개수 + {value3}";
                break;
            case 3: //양초 - 눈덩이 데미지
                infoPanel.text_ItemStat.text = $"공격력 + {value1}%\n";
                break;
            case 4: //따뜻한 마음- 체력
                infoPanel.text_ItemStat.text = $"체력 + {value1}";
                break;
            case 5: //별 - 범위 공격
                infoPanel.text_ItemStat.text = $"트리가 범위 공격\n 공격력 + {value1}\n 공격 주기 {value2}초 마다";
                break;
            case 6: //스노우 맨 - 포탑 설치
                infoPanel.text_ItemStat.text = $"눈사람 추가\n 공격력 + {value1}\n 공격 주기 {value2}초 마다\n 공격범위 +{value3}";
                break;
        }
    }

    public void Candle() //됏다
    {
        GameManager.Instance.SnowBallDmg += (int)value1;
        if (GameManager.Instance.getCandy >= upgradeInfo.UpgradeLevel[currentLevel].BuyCost)
            GameManager.Instance.AddCandy(-upgradeInfo.UpgradeLevel[currentLevel].BuyCost);

        if (currentLevel + 1 == upgradeInfo.MaxUpgrade)
        {
            infoPanel.text_ItemStat.text = $"공격력 + {value1}%";
        }
        else
        {
            infoPanel.text_ItemStat.text = $"공격력 + {value1}% -> {upgradeInfo.UpgradeLevel[currentLevel + 1].value1}%\n";
        }
        

    }

    public void HotHeart() //됏다
    {
        GameManager.Instance.playerMaxHp += (int)value1;
        GameManager.Instance.playerHp++;
        if (GameManager.Instance.getCandy >= upgradeInfo.UpgradeLevel[currentLevel].BuyCost)
            GameManager.Instance.AddCandy(-upgradeInfo.UpgradeLevel[currentLevel].BuyCost);

        if (currentLevel+1==upgradeInfo.MaxUpgrade)
        {
            infoPanel.text_ItemStat.text = $"체력 + {value1}%";
        }
        else
        {
            infoPanel.text_ItemStat.text = $"체력 + {value1}% -> {upgradeInfo.UpgradeLevel[currentLevel + 1].value1}%\n";
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
                $"공격력 + {value1}\n" +
                $"볼 개수 + {value3}";
        }
        else
        {
            infoPanel.text_ItemStat.text =
                $"공격력 + {value1} -> {upgradeInfo.UpgradeLevel[currentLevel + 1].value1}\n" +
                $"볼 개수 + {value3} -> {upgradeInfo.UpgradeLevel[currentLevel + 1].value3}";
        }


    }

    public void SnowMan() //됏다
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
            infoPanel.text_ItemStat.text = $"공격력 + {value1}\n발사주기 {value2}초 마다\n공격범위 + {value3}";
        }
        else
        {
            infoPanel.text_ItemStat.text =
                $"공격력 + {value1} -> 공격력 + {upgradeInfo.UpgradeLevel[currentLevel + 1].value1}\n" +
                $"발사주기 {value2}초 마다 -> 발사주기 {upgradeInfo.UpgradeLevel[currentLevel + 1].value2}초 마다\n" +
                $"공격범위 {value3} -> 공격범위 + {upgradeInfo.UpgradeLevel[currentLevel +1].value3}";
        }

    }

    public void Gift() //됏ㄷ다
    {
        //공격 속도 조정 업그레이드
        GameManager.Instance.playerAtkSpd = value1;
        if (GameManager.Instance.getCandy >= upgradeInfo.UpgradeLevel[currentLevel].BuyCost)
            GameManager.Instance.AddCandy(-upgradeInfo.UpgradeLevel[currentLevel].BuyCost);

        if (currentLevel + 1 == upgradeInfo.MaxUpgrade)
        {
            infoPanel.text_ItemStat.text = $"공격속도 {value1}";
        }
        else
        {
            infoPanel.text_ItemStat.text = $"공격속도 {value1} -> {upgradeInfo.UpgradeLevel[currentLevel + 1].value1}";
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
            infoPanel.text_ItemStat.text = $"트리가 범위 공격\n 공격력 + {value1}\n공격 주기 {upgradeInfo.UpgradeLevel[currentLevel].value2}";
        }
        else
        {
            infoPanel.text_ItemStat.text = $"공격력 + {value1} -> {upgradeInfo.UpgradeLevel[currentLevel + 1].value1}\n공격 주기 {value2} -> {upgradeInfo.UpgradeLevel[currentLevel+1].value2}";
        }
    }
}