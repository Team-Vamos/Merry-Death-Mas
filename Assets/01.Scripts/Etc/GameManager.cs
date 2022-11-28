using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public int SnowBallDmg = 10; //´«µ¢ÀÌ µ¥¹ÌÁö
    public int ShovelDmg { get; set; }//»ð µ¥¹ÌÁö

    public float FreezeTime = 3f; //Àû ºù°á ½Ã°£

    public float snowPileTime = 10f; //´« 1´Ü°è ½×ÀÌ´Â ½Ã°£

    private int snows = 0; //ÇÃ·¹ÀÌ¾î°¡ Áö´Ñ ´« ¼ö

    public int playerHp = 10;
    public int playeAtk = 2;

    public int getSnow
    {
        get => snows;
    }

    private void Awake()
    {
        ShovelDmg = 5;
    }

    public int multiply = 1;

    public void AddSnow(int amount) => snows += amount * multiply;

    public void UseSnow() => --snows;

    public void MultiplyShovelDmg(int amount) => ShovelDmg *= ((100 + amount) / 100);
}
