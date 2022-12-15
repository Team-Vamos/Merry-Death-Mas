using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpText : MonoBehaviour
{
    private Text HpTxt;

    private void Awake()
    {
        HpTxt = GetComponent<Text>();
    }

    public void SetHpText(int currentHP, int MaxHP)
    {
        if(currentHP > 0) HpTxt.text = $"   {currentHP}/{MaxHP}";
        else HpTxt.text = $"   0/{MaxHP}";
    }
}
