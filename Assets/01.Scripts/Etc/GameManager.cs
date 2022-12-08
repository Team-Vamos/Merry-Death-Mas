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

    private int candy = 1231; //플레이어가 지닌 사탕 수
    private int snows = 0; //플레이어가 지닌 눈 수
    private float shovelDmg = 1f; //삽 데미지 @@@@@@

    public int santaSpawnRate = 4; //산타 스폰 주기

    public float multiplySnow = 1; // 얻는 눈 개수 배율
    public float multiplyCandy = 1; // 얻는 사탕 개수 배율
    public int snowSpawnRate = 8; //눈 소환 주기
    public float snowPileTime = 60f; //눈 1단계 쌓이는 시간

    public int respawnTime = 3;    //부활 시간
    public int playerHp = 10;   //플레이어 체력 @@@@@@
    public float playerAtkSpd = 1f; //(60 프레임 * playerAtkSpd)에 한번 휘두름 @@@@@@
    public float FreezeTime = 3f; //적 빙결 시간
    public int SnowBallDmg = 10; //눈덩이 데미지 @@@@@@

    [Header("====== Upgarde Info ======")]
    public float StarDmg = 1f; //별 터질때 데미지 @@@@@@
    public float StartDelay = 1f;

    public float TurretDmg = 1f; //터렛 데미지 @@@@@@
    public float TurrentDelay = 1f;

    public int BallSize = 3;
    public float BallSpd = 50f;
    public float BallDmg = 3f;
    public float BallAtkDelay = 0.5f;



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

    public bool Enabled = false;

    public int presents
    {
        get => presentPoolManager.childCount;
    }

    private void Start()
    {
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

    public void AddSnow(int amount) => snows += (int)(amount * multiplySnow);
    public void AddCandy(int amount)
    {
        if(amount > 0)AddCandyPool(amount);
        candy += (int)(amount * multiplyCandy);
        candyTxt.text = $"Candy: {candy}";
    }

    public void UseSnow() => --snows;

    public void MultiplyShovelDmg(float amount) => shovelDmg += (shovelDmg * (amount / 100));

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
            RespawnTxt.text = $"성탄절의 힘으로\n 부활합니다.\n{i}";
            yield return new WaitForSeconds(1f);
        }
        RespawnPanel.SetActive(false);
        //부활 이팩트?
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
}
