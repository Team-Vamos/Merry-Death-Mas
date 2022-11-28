using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public int SnowBallDmg = 10; //������ ������
    public int ShovelDmg { get; set; }//�� ������

    public float FreezeTime = 3f; //�� ���� �ð�

    public float snowPileTime = 10f; //�� 1�ܰ� ���̴� �ð�

    private int snows = 0; //�÷��̾ ���� �� ��

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
