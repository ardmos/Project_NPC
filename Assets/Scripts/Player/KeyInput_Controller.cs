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

    Vector2 movement, rayDir;
    GameObject scanObject;

    // Update is called once per frame
    void Update()
    {
        //사용자 입력값 수집.
        movement.x = DialogueManager.instance.isDialogueActive ? 0 : Input.GetAxisRaw("Horizontal");
        movement.y = DialogueManager.instance.isDialogueActive ? 0 : Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        
        //Idle방향
        if(movement.y == -1)
        {
            animator.SetInteger("Direction", 0);
            rayDir = Vector2.down;
        }
        else if (movement.y == 1)
        {
            animator.SetInteger("Direction",1);
            rayDir = Vector2.up;
        }
        else if (movement.x == 1)
        {
            animator.SetInteger("Direction", 2);
            rayDir = Vector2.right;
        }
        else if (movement.x == -1)
        {
            animator.SetInteger("Direction", 3);
            rayDir = Vector2.left;
        }

        //대각선 이동 속도 조절 
        if (Mathf.Abs(movement.x) == Mathf.Abs(movement.y))
            movespeed = 4f;
        else
            movespeed = 5f;

        //스캔 발동. 스페이스 감지 처리 부분. Dialogue가 실행중일땐 감지 불가.
        if (Input.GetButtonDown("Jump") && scanObject != null)
        {
            //다이얼로그 발동시키자.
            scanObject.GetComponent<Object>().TriggerDialogue();
        }
    }

    private void FixedUpdate()
    {
        //실질적 이동.
            rb.MovePosition(rb.position + movement * movespeed * Time.fixedDeltaTime);

        //Ray
        Debug.DrawRay(rb.position, rayDir, Color.green, 0.7f);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(rb.position, rayDir, 0.7f, LayerMask.GetMask("Object"));

        if (raycastHit2D.collider != null)
            scanObject = raycastHit2D.collider.gameObject;
        else
            scanObject = null;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        //충돌체크.
        //print(collision.gameObject.name + "here");
    }
}



