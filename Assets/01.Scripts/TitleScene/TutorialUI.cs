using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    public GameObject Tutorialui;

    public Image nowImage;

    public int nowValue;

    public GameObject Back;
    public GameObject Front;
    public GameObject Play;

    public Sprite[] TutorialImage;
    void Start()
    {
        nowImage.sprite = TutorialImage[0];
    }

    private void FixedUpdate()
    {
        UpdateUI();
    }
    public void UpdateUI()
    {

        if(nowValue==0)
        {
            Back.SetActive(false);
        }
        else
        {
            Back.SetActive(true);
        }
        if(nowValue >= TutorialImage.Length-1)
        {
            Front.SetActive(false);
            Play.SetActive(true);
        }
        else
        {
            Play.SetActive(false);
            Front.SetActive(true);
        }
        nowImage.sprite = TutorialImage[nowValue];
    }

    public void NextSprite()
    {
        nowValue++;
    }

    public void BackSprite()
    {
        nowValue--;
    }

    public void TutorialUISetActive()
    {
        Tutorialui.SetActive(true);
    }

    public void TutorialUISetActiveFalse()
    {
        Tutorialui.SetActive(false);
    }

    public void CheckFirst()
    {
        if(BGM_Manager.Instance.isFirst)
        {
            TutorialUISetActive();
        }
        else
        {
            SceneMove.Instance.Play();
        }
    }
}
