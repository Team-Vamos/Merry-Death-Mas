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

    public int SnowBallDmg = 3;
    public int ShovelDmg = 4;
    public int ShovelKnockBack = 3;
    public int minSnow = 1;
    public int maxSnow = 5;

    public void AddSnow(int amount)
    {
        snows += amount;
    }    
}
