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
        //사용자 입력값 수집. 
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        

        if(movement.y == -1)
        {
            animator.SetInteger("Direction", 0);
        }
        else if (movement.y == 1)
        {
            animator.SetInteger("Direction",1);
        }
        else if (movement.x == 1)
        {
            animator.SetInteger("Direction", 2);
        }
        else if (movement.x == -1)
        {
            animator.SetInteger("Direction", 3);
        }
    }

    private void FixedUpdate()
    {
        //실질적 이동
        rb.MovePosition(rb.position + movement * movespeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //충돌체크.
        print(collision.gameObject.name + "here");
    }
}



