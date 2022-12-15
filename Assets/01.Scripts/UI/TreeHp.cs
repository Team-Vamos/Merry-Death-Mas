using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TreeHp : MonoBehaviour
{
    public Slider HpBar;
    [SerializeField]
    private Material TreeMaterial;
    [SerializeField]
    private HpText hpText;


    private void Start()
    {
        hpText.SetHpText((int)GameManager.Instance.TreeHp, (int)GameManager.Instance.TreeMaxHp);
        TreeMaterial.color = Color.white;
    }
 
    void Health()
    {
        HpBar.value = GameManager.Instance.TreeHp / GameManager.Instance.TreeMaxHp;
    }

    public void getDmg()
    {
        GameManager.Instance.TreeHp--;
        hpText.SetHpText((int)GameManager.Instance.TreeHp, (int)GameManager.Instance.TreeMaxHp);
        Health();
        StartCoroutine(Blink());
    }

    public void AutoHealing()
    {
        GameManager.Instance.TreeHp += GameManager.Instance.TreeHeal;
    }

    IEnumerator Blink()
    {
        TreeMaterial.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        TreeMaterial.color = Color.white;

        if(GameManager.Instance.TreeHp<0)
        {
            //°ÔÀÓ ³¡
        }
    }
}
