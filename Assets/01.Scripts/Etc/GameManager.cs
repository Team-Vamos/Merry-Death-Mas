using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private GameObject SnowPile;

    public int SnowBallDmg = 10; //눈덩이 데미지

    public float ShovelDmg
    {
        get => shovelDmg;
    }//삽 데미지

    private float shovelDmg = 1f; //삽 데미지

    public float TurretDmg = 1f; //터렛 데미지

    public float StarDmg = 1f; //별 터질때 데미지

    public float FreezeTime = 3f; //적 빙결 시간

    public float snowPileTime = 10f; //눈 1단계 쌓이는 시간

    private int snows = 0; //플레이어가 지닌 눈 수

    public int playerHp = 10;
    public int playeAtk = 2;

    public int snowSpawnRate = 8;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Text RespawnTxt;

    [SerializeField]
    private GameObject RespawnPanel;

    private int candy = 10;

    public int getCandy
    {
        get => candy;
    }


    public int getSnow
    {
        get => snows;
    }

    public float multiplySnow = 1;
    public float multiplyCandy = 1;

    public int santaSpawnRate = 4;

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
    
    public bool Enabled = false;

    public int presents
    {
        get => presentPoolManager.childCount;
    }

    private void Start()
    {
        InvokeRepeating("SpawnSnow", 0f, snowSpawnRate);
        candyTxt.text = $"Candy: {candy}";
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
        AddCandyPool(amount);
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
        RespawnTxt.text = "성탄절의 힘으로\n 부활합니다.\n 3";
        yield return new WaitForSeconds(1f);
        RespawnTxt.text = "성탄절의 힘으로\n 부활합니다.\n 2";
        yield return new WaitForSeconds(1f);
        RespawnTxt.text = "성탄절의 힘으로\n 부활합니다.\n 1";
        yield return new WaitForSeconds(1f);
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
