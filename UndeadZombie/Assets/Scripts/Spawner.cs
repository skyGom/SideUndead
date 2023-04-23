using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] SpawnPoints;
    public SpawnData[] SpawnDatas;
    public float levelTime;

    private float timer;
    private int level;

    private void Awake()
    {
        SpawnPoints = GetComponentsInChildren<Transform>();
        levelTime = GameManager.instance.MaxGameTime / SpawnDatas.Length;
    }

    void Update()
    {   
        if(!GameManager.instance.isLive) //시간 멈춤
            return;
        
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.GameTime / levelTime),SpawnDatas.Length-1);

        if (timer > SpawnDatas[level].spawnTime)
        {
            timer = 0f;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.GetPool(0);
        enemy.transform.position = SpawnPoints[Random.Range(1, SpawnPoints.Length)].transform.position;
        enemy.GetComponent<Enemy>().Init(SpawnDatas[level]);
    }
}

[System.Serializable]
public class SpawnData
{
    public int spriteType;
    public int health;
    public float speed;
    public float spawnTime;
}
