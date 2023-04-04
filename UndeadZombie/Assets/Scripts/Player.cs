using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
 
    public float speed = 3f;
    public Scanner Scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;

    [HideInInspector]public Vector2 inputVec;
    private Rigidbody2D rigid;
    private SpriteRenderer sprite;
    private Animator animator;
    private PhotonView photonView;

    private void Awake()
    {
        Scanner = GetComponent<Scanner>();
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        photonView = GetComponent<PhotonView>();
        hands = GetComponentsInChildren<Hand>(true);
    }

    void OnEnable()
    {
        speed *= Character.Speed;
        animator.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.instance.isLive) //시간 멈춤
            return;
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        //rigid.AddForce(inputVec);

        //rigid.velocity = inputVec;

        if(!GameManager.instance.isLive) //시간 멈춤
            return;
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    private void LateUpdate()
    {
        if(!GameManager.instance.isLive) //시간 멈춤
            return;
        animator.SetFloat("Speed", inputVec.magnitude);
        flip();
        photonView.RPC("filp", RpcTarget.All);
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    [PunRPC]
    private void flip()
    {
        if (inputVec.x != 0)
        {
            sprite.flipX = inputVec.x < 0;
        }
    }


    void OnCollisionStay2D(Collision2D collision) { // 캐릭터 피격 판정

        if(!GameManager.instance.isLive)
            return;

        GameManager.instance.health -= Time.deltaTime * 10;  // 체력 감소

        if(GameManager.instance.health < 0){
            for(int index =2; index < transform.childCount; index++){
                transform.GetChild(index).gameObject.SetActive(false); // 스포너등 비활성화(플레이어 2번째 이후 자식오브젝트 비활성화)
            }

            animator.SetTrigger("Dead"); // 플레이어 죽음 에니메이터 실행
            GameManager.instance.GameOver();
        }
        
    }
}
