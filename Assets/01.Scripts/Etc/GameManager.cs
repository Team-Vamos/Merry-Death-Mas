using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private GameObject SnowPile;

    public int SnowBallDmg = 10; //������ ������

    public float ShovelDmg
    {
        get => shovelDmg;
    }//�� ������

    private float shovelDmg = 5; //�� ������

    public float FreezeTime = 3f; //�� ���� �ð�

    public float snowPileTime = 10f; //�� 1�ܰ� ���̴� �ð�

    private int snows = 0; //�÷��̾ ���� �� ��

    public int playerHp = 10;
    public int playeAtk = 2;

<<<<<<< Updated upstream
    public int snowSpawnRate = 8;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Text RespawnTxt;

    [SerializeField]
    private GameObject RespawnPanel;
=======
    public int candy = 10;
>>>>>>> Stashed changes

    public int getSnow
    {
        get => snows;
    }

    private void Awake()
    {
        shovelDmg = 5;
    }

    public int multiply = 1;

    public void AddSnow(int amount) => snows += amount * multiply;

    public void UseSnow() => --snows;

    public void MultiplyShovelDmg(float amount) => shovelDmg += (shovelDmg * (amount/100));

    [SerializeField]
    private Transform PoolManager;

    private void Start()
    {
        InvokeRepeating("SpawnSnow", 0f, snowSpawnRate);
    }

    private void SpawnSnow()
    {
        if(PoolManager.childCount > 1)
        {
            PoolManager.GetChild(0).position = RandomPos();
            PoolManager.GetChild(0).gameObject.SetActive(true);
            PoolManager.GetChild(0).SetParent(null);
        }
    }

    private Vector3 RandomPos()
    {
        float randomZ = Random.Range(10f, 30f);
        float randomX = Random.Range(10f, 30f);
        Vector3 pos = new Vector3(randomX, -1.48f,randomZ);
        return pos;
    }

    public void RespawnPlayer()
    {
        StartCoroutine(Respawn());
    }

    public void DeSpawnSnow(GameObject Snow)
    {
        Snow.SetActive(false);
        Snow.transform.SetParent(PoolManager);
    }

    private IEnumerator Respawn()
    {
        RespawnPanel.SetActive(true);
        RespawnTxt.text = "��ź���� ������\n ��Ȱ�մϴ�.\n 3";
        yield return new WaitForSeconds(1f);
        RespawnTxt.text = "��ź���� ������\n ��Ȱ�մϴ�.\n 2";
        yield return new WaitForSeconds(1f);
        RespawnTxt.text = "��ź���� ������\n ��Ȱ�մϴ�.\n 1";
        yield return new WaitForSeconds(1f);
        RespawnPanel.SetActive(false);
        //��Ȱ ����Ʈ?
        player.SetActive(true);
    }

    public void ResetSanta(GameObject santa)
    {
        float randomX = Random.Range(10f, 30f);

        santa.transform.position = new Vector3(randomX, 25f, -100f);
        santa.SetActive(false);
    }
}
