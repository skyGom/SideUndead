using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("# Game Control")]
    public bool isLive;
    public float GameTime;
    public float MaxGameTime;

    [Header("# Player Info")]
    public int playerId;
    public float health;
    public float maxHealth = 100;
    public int level;
    public int Kill;
    public int Exp;
    public int[] NextExp = {10, 30, 60, 100, 150, 210, 280, 360, 3450, 600};

    [Header("# Game Object")]
    public PoolManager pool;
    public Player Player;
    public LevelUp uiLevelUP;
    public Result uiResult;
    public GameObject enemyCleaner;

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
            GameVictory();
        }
    }

    public void GameStart(int id)
    {
        playerId = id;
        health = maxHealth;

        Player.gameObject.SetActive(true);
        uiLevelUP.Select(playerId % 2); //무기 갯수만큼 나눔
        Resume();

        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine() // 게임오버 딜레이를 위한 코루틴
    {
        isLive = false;

        yield return new WaitForSeconds(0.5f); // 에니메이션 딜레이 기다림

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();
                
        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine() // 게임승리 딜레이를 위한 코루틴
    {
        isLive = false;
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f); // 에니메이션 딜레이 기다림

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();
                
        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }


    public void GameRetry()
    {
        SceneManager.LoadScene(0); //신 0번째 로드
    }

    public void GetExp()
    {
        if(!isLive) // 끝날떄 경험치 X
            return;

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
