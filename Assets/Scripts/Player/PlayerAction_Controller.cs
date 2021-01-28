using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction_Controller : MonoBehaviour
{
    //키인풋 컨트롤 만들기! 아무 오브젝트에나 이거 붙이면 방향키 입력 받아서 움직임! 
    //타일맵의 셀 사이즈를 1로 했기 때문에,  이동도 1만큼 시키면 딱 딱 맞음. <-- 바람의나라 방식
    //스타듀밸리방식은 translate * 속도 이용

    public float movespeed = 5f;

    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement;
    Vector3 dirVec;
    GameObject scanObject;

    // Update is called once per frame
    void Update()
    {
        //사용자 입력값 수집.
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        
        //Idle방향. Direction
        if(movement.y == -1)
        {
            animator.SetInteger("Direction", 0);
            dirVec = Vector3.down;
        }
        else if (movement.y == 1)
        {
            animator.SetInteger("Direction",1);
            dirVec = Vector3.up;
        }
        else if (movement.x == 1)
        {
            animator.SetInteger("Direction", 2);
            dirVec = Vector3.right;
        }
        else if (movement.x == -1)
        {
            animator.SetInteger("Direction", 3);
            dirVec = Vector3.left;
        }

        //Scan Object
        if (Input.GetButtonDown("Jump") && scanObject != null)
            FindObjectOfType<GameManager>().ScanAction(scanObject, "Stable");
    }

    private void FixedUpdate()
    {
        //실질적 이동
        if( Mathf.Abs(movement.x) == Mathf.Abs(movement.y))
            //대각선 이동속도 조절
            rb.MovePosition(rb.position + movement * movespeed * 0.81f * Time.fixedDeltaTime);
        else
            rb.MovePosition(rb.position + movement * movespeed * Time.fixedDeltaTime);

        //Ray
        Debug.DrawRay(rb.position, dirVec * 0.7f, new Color(0,1,0));
        RaycastHit2D raycastHit2D = Physics2D.Raycast(rb.position, dirVec, 0.7f, LayerMask.GetMask("Object"));

        if(raycastHit2D.collider != null)
            //뭔가 감지되었으면
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



