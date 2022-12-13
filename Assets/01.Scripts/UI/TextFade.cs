using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFade : MonoBehaviour
{
    public Text text;
    public delegate void Func();
    Func _func = null;

    public bool DoFade = false;
    public bool isItZero = false;
    public float time;

    private void OnEnable()
    {
        if(DoFade)
        {
            StartFade(text.gameObject, isItZero, time, () => text.gameObject.SetActive(false));
        }
    }

    public void StartFade(GameObject gameObject, bool isZero, float times, Func func = null)
    {
        _func = func;
        text = gameObject.GetComponent<Text>();
        gameObject.SetActive(true);
        if (isZero) StartCoroutine(FadeTextToZero(times));
        else StartCoroutine(FadeTextToFullAlpha(times));
    }

    public IEnumerator FadeTextToFullAlpha(float times)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        while (text.color.a < 1.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / times));
            yield return null;
        }
        _func();
    }

    public IEnumerator FadeTextToZero(float times)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        while (text.color.a > 0.1f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / times));
            yield return null;
        }
        _func();
    }
}
