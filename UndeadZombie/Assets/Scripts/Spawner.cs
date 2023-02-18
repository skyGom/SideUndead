using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] SpawnPoints;
    public SpawnData[] SpawnDatas;

    private float timer;
    private int level;

    private void Awake()
    {
        SpawnPoints = GetComponentsInChildren<Transform>();
    }

    void Update()
    {   
        timer += Time.deltaTime;
        level = Mathf.FloorToInt(GameManager.instance.GameTime / 10f);

        if (timer > SpawnDatas[level].spawnTime)
        {
            timer = 0f;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.PoolManager.GetPool(0);
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
