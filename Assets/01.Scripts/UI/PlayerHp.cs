using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour
{
    public int Health { get => GameManager.Instance.playerHp; }
    public int MaxHealth { get => GameManager.Instance.playerMaxHp; }

    public Image[] Heart;
    public Sprite FullHeart;
    public Sprite EmptyHeart;

    private void Start()
    {
        InvokeRepeating("AutoHealing", GameManager.Instance.playerAutoHealingCoolTime, GameManager.Instance.playerAutoHealingCoolTime);
    }

    private void Update()
    {
        if(Health> MaxHealth)
        {
            GameManager.Instance.playerHp = MaxHealth;
        }
        PlayerHpActive();
    }

    private void AutoHealing()
    {
        if (Health < MaxHealth)
        {
            Debug.Log("치유되엇습ㄴ디ㅏ 하하");
            GameManager.Instance.playerHp++;
        }

    }

    public void PlayerHpActive()
    {
        for (int i = 0; i < Heart.Length; i++)
        {
            if(i<Health)
            {
                Heart[i].sprite = FullHeart;
            }
            else
            {
                Heart[i].sprite = EmptyHeart;
            }

            if (i < MaxHealth)
            {
                Heart[i].enabled = true;
            }
            else
            {
                Heart[i].enabled = false;
            }
        }
    }
}
