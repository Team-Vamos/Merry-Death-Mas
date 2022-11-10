using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHP_NPC : MonoBehaviour
{
    private Slider sliderHpBar;

    public float MinHp
    {
        get => sliderHpBar.minValue;
        set
        {
            sliderHpBar.minValue = value;
        }
    }

    public float MaxHp
    {
        get => sliderHpBar.maxValue;
        set
        {
            sliderHpBar.maxValue = value;
        }
    }

    public float Value
    {
        get => sliderHpBar.value;
        set
        {
            sliderHpBar.value = value;
        }
    }

    private void Awake()
    {
        sliderHpBar = gameObject.GetComponentInChildren<Slider>();
    }

    private void setOnEnable(bool flag)
    {
        GetComponent<Canvas>().enabled = flag;
    }
}