using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private int snows = 0;

    public int getSnow
    {
        get => snows;
    }

    public void AddSnow(int amount)
    {
        snows += amount;
    }    
}
