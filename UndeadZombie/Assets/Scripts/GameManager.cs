using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("# Game Control")]
    public bool isLive;
    public float GameTime;
    public float MaxGameTime;

    [Header("# Player Info")]
    public int health;
    public int maxHealth = 100;
    public int level;
    public int Kill;
    public int Exp;
    public int[] NextExp = {10, 30, 60, 100, 150, 210, 280, 360, 3450, 600, 800};

    [Header("# Game Object")]
    public PoolManager pool;
    public Player Player;
    public LevelUp uiLevelUP;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if(!isLive)
            return;
        
        GameTime += Time.deltaTime;

        if (GameTime > MaxGameTime)
        {
            GameTime = MaxGameTime;
        }
    }

    private void Start()
    {
        health = maxHealth;
        //임시스크립트
        uiLevelUP.Select(1);
    }

    public void GetExp()
    {
        Exp++;
        if (Exp == NextExp[Mathf.Min(level,NextExp.Length-1)])
        {
            level++;
            Exp = 0;
            uiLevelUP.Show();
        }

    }

    public void Stop(){
        isLive = false;
        Time.timeScale = 0; // 유니티 시간 배율
    }

    public void Resume(){
        isLive = true;
        Time.timeScale = 1; // 기본값 1
    }
}
