using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TreeHp : MonoBehaviour
{
    public Text HpText;
    public Slider HpBar;
    [SerializeField]
    private Material TreeMaterial;


    private void Start()
    {
        TreeMaterial.color = Color.white;
    }

    void Health()
    {
        HpBar.value = GameManager.Instance.TreeHp / GameManager.Instance.TreeMaxHp;
    }

    public void getDmg()
    {
        GameManager.Instance.TreeHp--;
        HpText.text = $"{GameManager.Instance.TreeHp}";
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
