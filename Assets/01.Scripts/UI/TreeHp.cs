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

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        hpText.SetHpText((int)GameManager.Instance.TreeHp, (int)GameManager.Instance.TreeMaxHp);
        TreeMaterial.color = Color.white;
        InvokeRepeating("AutoHealing", GameManager.Instance.playerAutoHealingCoolTime, GameManager.Instance.playerAutoHealingCoolTime);
    }

    void Health()
    {
        HpBar.value = GameManager.Instance.TreeHp / GameManager.Instance.TreeMaxHp;
    }

    public void getDmg()
    {
        GameManager.Instance.TreeHp--;
        hpText.SetHpText((int)GameManager.Instance.TreeHp, (int)GameManager.Instance.TreeMaxHp);
        if (GameManager.Instance.TreeHp < (GameManager.Instance.TreeMaxHp / 10) * 3)
        {
            audioSource.Play();
            GameManager.Instance.DangerOn(true);
        }

        Health();
        StartCoroutine(Blink());
    }

    public void AutoHealing()
    {
        if (GameManager.Instance.TreeHp < GameManager.Instance.TreeMaxHp)
        {
            GameManager.Instance.TreeHp += GameManager.Instance.TreeHeal;
        }
    }

    IEnumerator Blink()
    {
        TreeMaterial.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        TreeMaterial.color = Color.white;

        if (GameManager.Instance.TreeHp < 1)
        {
            GameManager.Instance.Ending(0);
        }
    }
}
