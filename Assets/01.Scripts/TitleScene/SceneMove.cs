using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoSingleton<SceneMove>
{
    public void Play()
    {
        Cursor.visible = false;
        BGM_Manager.Instance.isFirst = false;
        SceneManager.LoadScene("Map");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
