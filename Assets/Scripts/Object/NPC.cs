using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    //NPC 전용 Controller


    public float movespeed = 5f;

    public Rigidbody2D rb;
    public Animator animator;

    public Vector2 movement;    


    //리모트무브
    public bool isrm;
    public Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData animData;
    public Vector2 originalPos, destinationPos;
    public bool isArrived;

    //제자리돌기 위한. 한번만 none에서 속도 주기 위한.
    public bool isdid;

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
                //print("on");
                //리모트 이동 도착.
                isrm = false;
                movement = Vector2.zero;                
            }


            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
            //제자리돌기 처리. 진행방향 none인 경우.
            if (animData.dir == Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.None)
            {
                if (!isdid)
                {
                    isdid = true;
                    animator.SetFloat("Speed", 1f);
                }
                else
                {
                    isArrived = true;
                    isrm = false;
                    animator.SetFloat("Speed", 0f);
                }
            }

            //Idle방향 따로 설정해줄 수 있음.
            if (animData.endDir == Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.EndDir.Down)
            {
                animator.SetInteger("Direction", 0);
            }
            else if (animData.endDir == Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.EndDir.Up)
            {
                animator.SetInteger("Direction", 1);
            }
            else if (animData.endDir == Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.EndDir.Right)
            {
                animator.SetInteger("Direction", 2);
            }
            else if (animData.endDir == Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.EndDir.Left)
            {
                animator.SetInteger("Direction", 3);
            }
        }
    }

    private void FixedUpdate()
    {
        //실질적 이동
        rb.MovePosition(rb.position + movement * movespeed * Time.fixedDeltaTime);
    }

    public void MoveAnimStart(Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData animData)
    {
        isrm = true;
        isArrived = false;
        isdid = false;
        this.animData = animData;
        originalPos = gameObject.transform.position;

        switch (animData.dir)
        {
            case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.Up:
                destinationPos = originalPos + Vector2.up * animData.distance;
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

  
}
