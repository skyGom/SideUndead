using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int Id;
    public int PrefabId;
    public int Count;
    public float Damage;
    public float Speed;

    private float timer;
    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void Start()
    {
        Init();
    }

    void Update()
    {
        switch (Id)
        {
            case 0:
                transform.Rotate(Vector3.back * Speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;

                if (timer > Speed)
                {
                    timer = 0;
                    Fire();
                }
                break;
        }
    }

    public void Init()
    {
        switch (Id)
        {
            case 0:
                Speed = 150;
                SetWeapon();
                break;
            default:
                Speed = 0.3f;
                break;
        }
    }

    private void SetWeapon()
    {
        for (int i = 0; i < Count; i++)
        {
            Transform bullet;

            if (i < transform.childCount)
            {
                bullet = transform.GetChild(i);
            }
            else
            {
                bullet = GameManager.instance.PoolManager.GetPool(PrefabId).transform;
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * i / Count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);

            bullet.GetComponent<Bullet>().Init(Damage, -1, Vector3.zero);
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.Damage = damage;
        this.Count += count;

        if (Id == 0)
        {
            SetWeapon();
        }
    }

    private void Fire()
    {
        if (player.Scanner.NearestTarget == null)
            return;

        Vector3 targetPos = player.Scanner.NearestTarget.transform.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.PoolManager.GetPool(PrefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(Damage, Count, dir);
    }
}
