using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Speed;
    public float Health;
    public float MaxHealth;
    public RuntimeAnimatorController[] AnimatorController;
    public Rigidbody2D Target;

    private bool isLive;

    private Rigidbody2D rigid;
    private Collider2D coll;
    private Animator animator;
    private SpriteRenderer sprite;
    private WaitForFixedUpdate wait;

    private void OnEnable()
    {
        Target = GameManager.instance.Player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        sprite.sortingOrder = 2;
        animator.SetBool("Dead", false);
        Health = MaxHealth;
        wait = new WaitForFixedUpdate();
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if(!GameManager.instance.isLive) //시간 멈춤
            return;
        if (!isLive || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        Vector2 dirVec = Target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * Speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if(!GameManager.instance.isLive) //시간 멈춤
            return;
        if (!isLive)
            return;

        sprite.flipX = Target.position.x < rigid.position.x;
    }

    public void Init(SpawnData spawnData)
    {
        animator.runtimeAnimatorController = AnimatorController[spawnData.spriteType];
        Speed = spawnData.speed;
        MaxHealth = spawnData.health;
        Health = MaxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        Health -= collision.GetComponent<Bullet>().Damage;
        StartCoroutine(KnockBack());

        if (Health > 0)
        {
            animator.SetTrigger("Hit");
        }
        else
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            sprite.sortingOrder = 1;
            animator.SetBool("Dead", true);
            GameManager.instance.Kill++;
            GameManager.instance.GetExp();
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait;
        Vector3 playerPos = GameManager.instance.Player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    private void Dead()
    {
        gameObject.SetActive(false);
    }
}
