using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInput_Controller : MonoBehaviour
{
    //키인풋 컨트롤 만들기! 아무 오브젝트에나 이거 붙이면 방향키 입력 받아서 움직임! 
    //타일맵의 셀 사이즈를 1로 했기 때문에,  이동도 1만큼 시키면 딱 딱 맞음. <-- 바람의나라 방식
    //스타듀밸리방식은 translate * 속도 이용

    public float movespeed = 5f;

    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement;

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }


    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * movespeed * Time.fixedDeltaTime);
    }


    /// <summary>
    /// 아, 지금 이동을 쑥 쑥 해버려서, 충돌체크 하기도 전에 쑥 통과해버리는거지!!!  
    /// 이동방식 변경 ㄱㄱ !
    /// </summary>

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print(collision.gameObject.name);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.gameObject.name);
    }
}



