using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private GameObject SnowPile;

    public GameObject StarUpgrade;

    public GameObject SnowManObj;

    [Header("====== Player Info ======")]
    [SerializeField]
    private GameObject player;

    private int candy = 1231; //�÷��̾ ���� ���� ��
    private int snows = 0; //�÷��̾ ���� �� ��
    private float shovelDmg = 4f; //�� ������ @@@@@@

    public int santaSpawnRate = 4; //��Ÿ ���� �ֱ�

    public float multiplySnow = 1; // ��� �� ���� ����
    public float multiplyCandy = 1; // ��� ���� ���� ����
    public int snowSpawnRate = 8; //�� ��ȯ �ֱ�
    public float snowPileTime = 60f; //�� 1�ܰ� ���̴� �ð�

    public int respawnTime = 3;    //��Ȱ �ð�
    public int playerHp;   //�÷��̾� ü�� @@@@@@
    public int playerMaxHp = 10;
    public float playerAutoHealingCoolTime = 10f;
    public float playerAtkSpd = 1f; //(60 ������ * playerAtkSpd)�� �ѹ� �ֵθ� @@@@@@
    public float FreezeTime = 3f; //�� ���� �ð�
    public int SnowBallDmg = 10; //������ ������ @@@@@@

    [Header("====== Upgarde Info ======")]
    public float StarDmg = 1f; //�� ������ ������ @@@@@@
    public float StartDelay = 1f;

    public float TurretDmg = 1f; //�ͷ� ������ @@@@@@
    public float TurrentDelay = 1f;
    public float TurretRange = 1f;

    public int BallSize = 3;
    public float BallSpd = 50f;
    public float BallDmg = 3f;
    public float BallAtkDelay = 0.5f;

    [Header("====== Tree Info ======")]
    public float TreeMaxHp = 10f;
    public float TreeHp;
    public float TreeHeal = 10f;

    #region ====== Getters ======
    public int getCandy
    {
        get => candy;
    }

    public int getSnow
    {
        get => snows;
    }

    public float ShovelDmg
    {
        get => shovelDmg;
    }
    #endregion

    [Header("====== Pools ======")]
    public Transform snowPoolManager;

    public Transform presentPoolManager;

    public Transform addCandyPool;

    [Header("====== UIs ======")]
    [SerializeField]
    private Text candyTxt;

    [SerializeField]
    private Text addCcandyTxt;

    [SerializeField]
    private Transform textPanel;

    [SerializeField]
    private Text RespawnTxt;

    [SerializeField]
    private GameObject RespawnPanel;

    public GameObject Hert;

    public bool Enabled = false;

    [SerializeField]
    private GameObject outOfSnow;

    [SerializeField]
    private GameObject Danger;

    public int presents
    {
        get => presentPoolManager.childCount;
    }

    private void Start()
    {
        playerHp = playerMaxHp;
        TreeHp = TreeMaxHp;
        StartCoroutine(CreateSnow());
        AddCandy(0);
    }

    IEnumerator CreateSnow()
    {
        while (true)
        {
            SpawnSnow();
            yield return new WaitForSeconds(snowSpawnRate);
        }
    }

    private void SpawnSnow()
    {
        if (snowPoolManager.childCount > 1)
        {
            snowPoolManager.GetChild(0).position = RandomPos();
            snowPoolManager.GetChild(0).gameObject.SetActive(true);
            snowPoolManager.GetChild(0).SetParent(null);
        }
    }


    public void GetTreeDmg(int amount) => TreeHp -= amount;
    public void AddSnow(int amount) => snows += (int)(amount * multiplySnow);
    public void AddCandy(int amount)
    {
        if(amount > 0)AddCandyPool(amount);
        candy += (int)(amount * multiplyCandy);
        candyTxt.text = $"{candy}";
    }

    public void UseSnow() => --snows;

    public void MultiplyShovelDmg(float amount) => shovelDmg = amount;

    private Vector3 RandomPos()
    {
        float randomZ = Random.Range(10f, 30f);
        float randomX = Random.Range(10f, 30f);
        Vector3 pos = new Vector3(randomX, -1.48f, randomZ);
        return pos;
    }

    public int RandomCandy(int min, int max)
    {
        int Candy = Random.Range(min, max + 1);
        return Candy;
    }

    public void RespawnPlayer()
    {
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        RespawnPanel.SetActive(true);
        for (int i = respawnTime; i > 0; i--)
        {
            RespawnTxt.text = $"��ź���� ������\n ��Ȱ�մϴ�.\n{i}";
            yield return new WaitForSeconds(1f);
        }
        RespawnPanel.SetActive(false);
        playerHp = playerMaxHp;
        player.SetActive(true);
    }

    public void ResetSanta(GameObject santa)
    {
        float randomX = Random.Range(-30f, 30f);

        santa.transform.position = new Vector3(randomX, 25f, -100f);
        santa.SetActive(false);
        StartCoroutine(spawnSanta(randomX, santa));
    }

    IEnumerator spawnSanta(float randomTime, GameObject santa)
    {
        yield return new WaitForSeconds((randomTime/3) * santaSpawnRate);
        santa.SetActive(true);
    }

    public void AddCandyPool(int candyCnt)
    {
        Text text;
        string sign = "";
        sign = candyCnt > 0 ? "+" : "";
        Debug.Log(addCandyPool.childCount);
        if (addCandyPool.childCount < 1)
        {
            text = Instantiate(addCcandyTxt).GetComponent<Text>();
            text.transform.SetParent(textPanel);
            text.text = $"{sign}{candyCnt}";
        }
        else
        {
            text = addCandyPool.GetChild(0).GetComponent<Text>();
            text.gameObject.SetActive(true);
            text.transform.SetParent(textPanel);
            text.text = $"{sign}{candyCnt}";
        }
    }

    public void OutOfSnow()
    {
        outOfSnow.SetActive(true);
    }

    public void DangerOn(bool on)
    {
        Danger.SetActive(on);
    }
}