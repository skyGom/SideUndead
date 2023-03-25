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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        //rigid.AddForce(inputVec);

        //rigid.velocity = inputVec;

        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    private void LateUpdate()
    {
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
}
