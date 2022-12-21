using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour
{
    public int Health; 
    public int MaxHealth; 

    private List<Image> Heart = new List<Image>();
    public Sprite FullHeart;
    public Sprite EmptyHeart;

    public Transform HeartObj;

    private void Start()
    {
        Health = GameManager.Instance.playerHp;
        MaxHealth = GameManager.Instance.playerMaxHp;
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
        for (int i = 0; i < Heart.Count; i++)
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

    private void OnValidate()
    {
        if (HeartObj != null)
        {
            for (int i = 0; i < HeartObj.childCount; i++)
            {
                Heart.Add(HeartObj.GetChild(i).GetComponent<Image>());
            }
        }
    }
}
