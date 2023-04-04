using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int Id;
    public int PrefabId;
    public int Count;
    public float Damage;
    public float Speed;

    float timer;
    Player player;

    void Awake()
    {
        player = GameManager.instance.Player;
    }

    void Update()
    {
        if(!GameManager.instance.isLive) //시간 멈춤
            return;
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

    public void Init(ItemData data)
    {
        // basic set
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        //property set
        Id = data.itemId;
        Damage = data.baseDamage * Character.Damage;
        Count = data.baseCount + Character.Count;

        for(int index=0; index < GameManager.instance.pool.Prefabs.Length; index++){
            if(data.projectile == GameManager.instance.pool.Prefabs[index]){
                PrefabId = index;
                break;
            }
        }

        switch (Id)
        {
            case 0:
                Speed = 150 * Character.WeaponSpeed;
                Batch();
                break;
            default:
                Speed = 0.5f * Character.WeaponRate;
                break;
        }
        //Hand Set
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    private void Batch()
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
                bullet = GameManager.instance.pool.GetPool(PrefabId).transform;
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
        this.Damage = damage * Character.Damage;
        this.Count += count;

        if (Id == 0)
        {
            Batch();
        }
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    private void Fire()
    {
        if (player.Scanner.NearestTarget == null)
            return;

        Vector3 targetPos = player.Scanner.NearestTarget.transform.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.GetPool(PrefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(Damage, Count, dir);
    }
}
