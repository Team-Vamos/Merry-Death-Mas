using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sol : MonoBehaviour
{
    public Transform YSP;
    private List<GameObject> YS = new List<GameObject>();
    public int objSize { get => GameManager.Instance.BallSize; }
    public float circleR;
    public float deg;
    public float objSpeed { get => GameManager.Instance.BallSpd; }

    void Update()
    {
        deg += Time.deltaTime * objSpeed;
        if (deg < 360)
        {
            for (int i = 0; i < objSize; i++)
            {
                var rad = Mathf.Deg2Rad * (deg + (i * (360 / objSize)));
                var x = circleR * Mathf.Sin(rad);
                var z = circleR * Mathf.Cos(rad);
                YS[i].transform.position = transform.position + new Vector3(x, 3f ,z);
                YS[i].transform.rotation = Quaternion.Euler(0, (deg + (i * (360 / objSize))) * -1, 0);
            }
        }
        else
        {
            deg = 0;
        }
    }

    private void OnValidate()
    {
        for (int i = 0; i < YSP.childCount; i++)
        {
            YS.Add(YSP.GetChild(i).gameObject);
        }
    }

    public void AddObj()
    {
        for (int i =0;i<objSize;i++)
        {
            YS[i].SetActive(true);
        }

    }
}
