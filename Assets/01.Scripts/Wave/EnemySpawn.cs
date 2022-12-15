using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SpawnMonsters
{
    public GameObject[] spawnMonsters;
    public int[] SpawnMonsterPersent;
    public int SpawnMonsterBetweenTime;
}



public class EnemySpawn : MonoBehaviour
{
    public Transform Tree;

    public int radius;

    public int NightTime;
    public int AfternoonTime;

    private int WaveCnt = 0;
    
    public bool night;

    public Text timeText;
    public Text waveText;
    public Text NowTimeText;
    private float time;


    public SpawnMonsters[] Wave;

    private void Update()
    {
        if(time>0)
        {
            time -= Time.deltaTime;
        }
        else
        {
            if(night==true) // 아침 시작
            {
                time = AfternoonTime;
                night = false;
            }
            else if(night==false) //밤이 시작
            {
                time = NightTime;
                night = true;
                WaveCnt++;
                waveText.text = WaveCnt.ToString();
            }
        }
        timeText.text = Mathf.Ceil (time).ToString();
    }

    Vector3 Return_RandomPosition()
    {
        Vector3 treePosition = Tree.position;

        float a = treePosition.x;
        float b = treePosition.z;

        float x = Random.Range(-radius + a, radius + b);
        float z_b = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(x - a, 2));
        z_b *= Random.Range(0, 2) == 0 ? -1 : 1;
        float z = z_b + b;

        Vector3 randomPosition = new Vector3(x, 0, z);

        return randomPosition;
    }

    // 소환할 Object
    private void Start()
    {
        waveText.text = WaveCnt.ToString();
        night = false;
        time = AfternoonTime;
        StartCoroutine(RandomRespawn_Coroutine());
    }

    IEnumerator RandomRespawn_Coroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Wave[WaveCnt].SpawnMonsterBetweenTime);
            RandomValue();
        }
    }

    int MaxPersent()
    {
        int a = 0;
        for(int i = 0;i<Wave[WaveCnt].SpawnMonsterPersent.Length;i++)
        {
            a += Wave[WaveCnt].SpawnMonsterPersent[i];
        }
        return a;
    }

    void RandomValue()
    {
        int a = Random.Range(1, MaxPersent()+1);
        for(int i =0;i<Wave[WaveCnt].SpawnMonsterPersent.Length;i++)
        {
            a -= Wave[WaveCnt].SpawnMonsterPersent[i];
            
            if(a<=0 && night == true)
            {
                Instantiate(Wave[WaveCnt].spawnMonsters[i], Return_RandomPosition(), Quaternion.identity);
                break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Tree.position, radius);
    }
}