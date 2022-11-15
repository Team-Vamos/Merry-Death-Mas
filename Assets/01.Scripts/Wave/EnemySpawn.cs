using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SpawnMonsters
{
    public GameObject[] spawnMonsters;
    public int[] spawnPersent;
    public int SpawnMonsterValue;
    public int SpawnMonsterBetweenTime;
}



public class EnemySpawn : MonoBehaviour
{
    public Transform Tree;



    public int radius;

    public int NightTime;
    public int AfternoonTime;

    

    private int WaveCnt = 0 ;
    
    public bool night;

    public Text timeText;
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
            if(night==false)
            {
                time = NightTime;
                night = true;
            }
            else if(night==true)
            {
                time = AfternoonTime;
                night = false;
                WaveCnt++;
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
    public GameObject capsul;
    private void Start()
    {
        night = true;
        time = NightTime;
        StartCoroutine(RandomRespawn_Coroutine());
    }

    IEnumerator RandomRespawn_Coroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Wave[WaveCnt].SpawnMonsterBetweenTime);

            // 생성 위치 부분에 위에서 만든 함수 Return_RandomPosition() 함수 대입
            if (night == true)
            {
                for(int i=0;i<Wave[WaveCnt].spawnMonsters.Length;i++)
                {
                    
                    Instantiate(Wave[WaveCnt].spawnMonsters[i], Return_RandomPosition(), Quaternion.identity);
                }

                //GameObject instantCapsul = Instantiate(capsul, Return_RandomPosition(), Quaternion.identity);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Tree.position, radius);
    }
}
