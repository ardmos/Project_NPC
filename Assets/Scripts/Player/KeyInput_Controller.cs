using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInput_Controller : MonoBehaviour
{
    //키인풋 컨트롤 만들기! 아무 오브젝트에나 이거 붙이면 방향키 입력 받아서 움직임! 
    //타일맵의 셀 사이즈를 1로 했기 때문에,  이동도 1만큼 시키면 딱 딱 맞음. <-- 바람의나라 방식
    //스타듀밸리방식은 translate * 속도 이용



        //메인캐릭터만 쓰는 스크립트가 필요하겠다!! NPC는 NPC.cs로 제작. 
        //혹은, 다른애들은 이동만 가능하게끔 만들어주던가.
        //현재 레이캐스트 문제가 있다 .



    //현재 컨트롤할 대상 선택
    public bool isControllable;


    public float movespeed = 5f;

    public Rigidbody2D rb;
    public Animator animator;

    public Vector2 movement, rayDir;
    public GameObject scanObject;


    //리모트무브
    public bool isrm;
    public Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData animData;
    public Vector2 originalPos, destinationPos;
    public bool isArrived;

    // Update is called once per frame
    void Update()
    {
        //컨트롤체크 안되어있는 뇨속은 컨트롤 불가!
 
        //리모트컨트롤 구현부분
        if (isrm)
        {
            Vector2 curpos = gameObject.transform.position;         
            switch (animData.dir)
            {
                //도착했는지 확인.
                case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.Up:
                    if (curpos.y >= destinationPos.y)
                        isArrived = true;
                    break;
                case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.Down:
                    if (curpos.y <= destinationPos.y)
                        isArrived = true;
                    break;
                case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.Left:
                    if (curpos.x <= destinationPos.x)
                        isArrived = true;
                    break;
                case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.Right:
                    if (curpos.x >= destinationPos.x)
                        isArrived = true;
                    break;
                case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.UpRight:
                    if (curpos.x >= destinationPos.x && curpos.y >= destinationPos.y)
                        isArrived = true;
                    break;
                case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.UpLeft:
                    if (curpos.x <= destinationPos.x && curpos.y >= destinationPos.y)
                        isArrived = true;
                    break;
                case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.DownRight:
                    if (curpos.x >= destinationPos.x && curpos.y <= destinationPos.y)
                        isArrived = true;
                    break;
                case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.DownLeft:
                    if (curpos.x <= destinationPos.x && curpos.y <= destinationPos.y)
                        isArrived = true;
                    break;
                default:
                    break;
            }
            if (!isArrived)
            {
                //아직 도착한게 아니면 계속 가.
                switch (animData.dir)
                {
                    case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.Up:
                        movement = Vector2.up;
                        break;
                    case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.Down:
                        movement = Vector2.down;
                        break;
                    case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.Left:
                        movement = Vector2.left;
                        break;
                    case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.Right:
                        movement = Vector2.right;
                        break;
                    case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.UpRight:
                        movement = new Vector2(1, 1);
                        break;
                    case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.UpLeft:
                        movement = new Vector2(-1, 1);
                        break;
                    case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.DownRight:
                        movement = new Vector2(1, -1);
                        break;
                    case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.DownLeft:
                        movement = new Vector2(-1, -1);
                        break;
                    default:
                        movement = Vector2.zero;
                        break;
                }
            }            
            else
            {
                //리모트 이동 도착.
                isrm = false;                

                return;
            }
            
        }

        
        if(isControllable && !isrm)
        {
            //사용자 입력값 수집.
            movement.x = DialogueManager.Instance.isDialogueActive ? 0 : Input.GetAxisRaw("Horizontal");
            movement.y = DialogueManager.Instance.isDialogueActive ? 0 : Input.GetAxisRaw("Vertical");
        }

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        //대각선 이동 속도 조절 
        if (Mathf.Abs(movement.x) == Mathf.Abs(movement.y))
            movespeed = 4f;
        else
            movespeed = 5f;

        //Idle방향
        if (movement.y == -1)
        {
            animator.SetInteger("Direction", 0);
            rayDir = Vector2.down;
        }
        else if (movement.y == 1)
        {
            animator.SetInteger("Direction", 1);
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


        //스캔 발동. 스페이스 감지 처리 부분. Dialogue가 실행중일땐 감지 불가.
        if (Input.GetButtonDown("Jump") && scanObject != null)
        {
            print("scanObject: " + scanObject);
            //다이얼로그 발동시키자.
            scanObject.GetComponent<Object>().TriggerDialogue();
        }
    }

    private void FixedUpdate()
    {
        //실질적 이동
        rb.MovePosition(rb.position + movement * movespeed * Time.fixedDeltaTime);        

        //Ray
        Debug.DrawRay(rb.position, rayDir, Color.green, 0.7f);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(rb.position, rayDir, 0.7f, LayerMask.GetMask("Object"));

        if (raycastHit2D.collider != null)
            scanObject = raycastHit2D.collider.gameObject;
        else
            scanObject = null;
    }

    public void MoveAnimStart(Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData animData)
    {
        isrm = true;
        isArrived = false;
        this.animData = animData;
        originalPos = gameObject.transform.position;

        switch (animData.dir)
        {
            case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.Up:
                destinationPos = originalPos + Vector2.up*animData.distance;
                break;
            case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.Down:
                destinationPos = originalPos + Vector2.down * animData.distance;
                break;
            case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.Left:
                destinationPos = originalPos + Vector2.left * animData.distance;
                break;
            case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.Right:
                destinationPos = originalPos + Vector2.right * animData.distance;
                break;
            case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.UpRight:
                destinationPos = originalPos + new Vector2(1, 1) * animData.distance;
                break;
            case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.UpLeft:
                destinationPos = originalPos + new Vector2(-1, 1) * animData.distance;
                break;
            case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.DownRight:
                destinationPos = originalPos + new Vector2(1, -1) * animData.distance;
                break;
            case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.DownLeft:
                destinationPos = originalPos + new Vector2(-1, -1) * animData.distance;
                break;
            default:
                break;
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //충돌체크.
    //print(collision.gameObject.name + "here");
    //}
}



