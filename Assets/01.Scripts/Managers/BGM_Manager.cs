using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGM_Manager : MonoBehaviour
{
    [SerializeField]
    private Transform record;

    [SerializeField]
    private Image recordImage;

    [SerializeField]
    private Image elbumImage;

    [SerializeField]
    private Sprite[] ImageSprites;



    private void Update()
    {
        SpinRecord();
    }

    private void SpinRecord()
    {
        record.Rotate(0, 0, -5f);
    }
}
