using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("# Game Control")]
    public float GameTime;
    public float MaxGameTime;

    [Header("# Player Info")]
    public int Health;
    public int MaxHealth = 100;
    public int Level;
    public int Kill;
    public int Exp;
    public int[] NextExp = {10, 30, 60, 100, 150, 210, 280, 360, 3450, 600};

    [Header("# Game Object")]
    public PoolManager PoolManager;
    public Player Player;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        GameTime += Time.deltaTime;

        if (GameTime > MaxGameTime)
        {
            GameTime = MaxGameTime;
        }
    }

    private void Start()
    {
        Health = MaxHealth;
    }

    public void GetExp()
    {
        Exp++;

        if (Exp == NextExp[Level])
        {
            Level++;
            Exp = 0;
        }
    }
}
