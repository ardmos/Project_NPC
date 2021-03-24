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
    public Vector2 originalPos, destinationPos, curpos;
    public bool isArrived;


    //제자리돌기 위한. 한번만 none에서 속도 주기 위한.
    public bool isJustTurnCompleted;

    //복합이동 끝났는지 체크
    public bool isMoveSetOn;
    //복합이동 위한 큐
    Queue<Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveSet> moveSets;
    //복합이동 중 현 moveSet
    Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveSet curMoveSet;

    //Fallow모드
    public bool followMode, isItMiro, isItFromDialog;
    public GameObject desObj;
    public float xDis, yDis;
    public Vector2 desPos;
    public float 도착범위;
    public float 따라가기위치생성거리;
    public float 이동속도;
    //public float 메인캐리터의이전위치;
    public Vector2 총이동량;
    //다이얼로그 호출 처리
    public int 다이얼로그호출마지막방향;

    private void Awake()
    {
        moveSets = new Queue<Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveSet>();
    }

    public void Start()
    {
        desObj = FindObjectOfType<KeyInput_Controller>().gameObject;
        도착범위 = 0.1f;
        이동속도 = 1f;
        따라가기위치생성거리 = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        //컨트롤체크 안되어있는 뇨속은 컨트롤 불가!

        //리모트컨트롤 구현부분
        if (isrm)
        {
            curpos = gameObject.transform.position;


            //MoveSet 복함이동 처리 : 시작은 MoveAnimStart()
            //1. 순서대로 줄세움(큐 사용)
            //2. 순서대로 차례꺼 이동 처리
            //3. 끝났으면 끝 처리. 안끝났으면 2번
            //복합이동중인지 확인 변수 isMoveSetOn
            if (isMoveSetOn)
            {
                //이동 처리
                MoveMaker(curMoveSet);
            }
        }

        //Following 구현 부분

        // 미로맵일 경우! 멀어지면 FollowMode 켜주는 부분.
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Miro") isItMiro = true;

        if (isItMiro)
        {
            //Debug.Log("이번프레임이동량:" + desObj.GetComponent<KeyInput_Controller>().이번프레임이동량 + "총이동량:" + 총이동량 + ", followMode:" + followMode + "나의이동량:"+movement);
            //이동량 일정 이상 되는지 체크.
            //사실상 거리 0.5f
            if (Mathf.Abs(총이동량.x) >= 따라가기위치생성거리 || Mathf.Abs(총이동량.y) >= 따라가기위치생성거리)
            {
                //플레이어가 NPC쪽으로 이동한것인지? 그게 아닌 경우에만 이동 시작!
                if (IsItFar2(desObj.transform.position))
                {
                    //Debug.Log(desObj.name + ".pos: " + desObj.transform.position);
                    //이동!
                    if (IsItFar((Vector2)desObj.transform.position - 총이동량)) followMode = true;
                    else
                    {
                        //도착!
                        movement = Vector2.zero;

                        //followMode = false;
                    }
                }
                else
                {
                    //Debug.Log("플레이어가 NPC쪽으로 이동했어요!");
                    followMode = false;
                    movement = Vector2.zero;
                    MakeWalkingAnimation();
                }

                총이동량 = Vector2.zero;
            }
            else
            {
                총이동량 += desObj.GetComponent<KeyInput_Controller>().이번프레임이동량;
            }
                      
            if (followMode) KeepGoing_Follow();
        }

        //다이얼로그 호출 이동일 경우!
        if (isItFromDialog)
        {
            if (IsItFar_ForDialog()) followMode = true;
            if (followMode) KeepGoing_Follow_ForDialog();
        }


        //대각선 이동 속도 조절 
        if (Mathf.Abs(movement.x) == Mathf.Abs(movement.y))
            movespeed = 4f;
        else
            movespeed = 5f;
    }

    private void FixedUpdate()
    {
        //실질적 이동
        rb.MovePosition(rb.position + movement * movespeed * Time.fixedDeltaTime);
    }

    #region For RemoteMove
    public void MoveAnimStart(Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData animData)
    {
        isMoveSetOn = true;

        this.animData = animData;

        moveSets.Clear();

        //여기서 moveSet을 차례차례 큐에 넣기. 
        foreach (Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveSet moveSet in animData.moveSet)
        {
            moveSets.Enqueue(moveSet);
        }

        //시작부터 등록되어있는 큐가 없으면?  등록을 안한것이니, 그냥 패스 ~!
        if (moveSets.Count == 0)
        {
            Debug.Log("moveSet 큐가 비어있습니다.");
        }
        else
        {
            StartNextMove();
        }
    }

    public void SetDestinationPos(Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveSet moveSet)
    {
        originalPos = gameObject.transform.position;
        switch (moveSet.dir)
        {
            case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.Up:
                destinationPos = originalPos + Vector2.up * moveSet.distance;
                break;
            case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.Down:
                destinationPos = originalPos + Vector2.down * moveSet.distance;
                break;
            case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.Left:
                destinationPos = originalPos + Vector2.left * moveSet.distance;
                break;
            case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.Right:
                destinationPos = originalPos + Vector2.right * moveSet.distance;
                break;
            case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.UpRight:
                destinationPos = originalPos + new Vector2(1, 1) * moveSet.distance;
                break;
            case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.UpLeft:
                destinationPos = originalPos + new Vector2(-1, 1) * moveSet.distance;
                break;
            case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.DownRight:
                destinationPos = originalPos + new Vector2(1, -1) * moveSet.distance;
                break;
            case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.DownLeft:
                destinationPos = originalPos + new Vector2(-1, -1) * moveSet.distance;
                break;
            case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.None:
                Debug.Log("제자리 돌기");
                break;
            default:
                Debug.Log("최종목적지 설정 실패");
                destinationPos = Vector2.zero;
                break;
        }
    }

    public void MoveMaker(Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveSet moveSet)
    {
        if (IsArrivedChecker(moveSet))      //도착여부 확인 
        {
            //이동 도착 처리.
            Arrived();
        }
        else
        {
            //아직 도착한게 아니면 계속 이동 진행
            KeepGoing(moveSet);
        }

        //걷기 애니메이션 처리
        MakeWalkingAnimation();
    }

    public bool IsArrivedChecker(Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveSet moveSet)
    {
        switch (moveSet.dir)
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
            case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.None:
                JustTurn();
                break;
            default:
                isArrived = false;
                break;
        }
        return isArrived;
    }

    public void KeepGoing(Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveSet moveSet)
    {
        switch (moveSet.dir)
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
            case Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveDir.None:
                //부드러운 회전을 위해.  EndDir 방향에 맞춰서 movement를 설정해준다. 안그러면movement가 0이어서 무조건 회전시 왼쪽을 한 번 바라보고 회전하게된다.
                if (animData.endDir == Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.EndDir.Down) movement = Vector2.down;
                else if (animData.endDir == Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.EndDir.Up) movement = Vector2.up;
                else if (animData.endDir == Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.EndDir.Right) movement = Vector2.right;
                else if (animData.endDir == Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.EndDir.Left) movement = Vector2.left;
                Debug.Log("제자리돌기 킵고잉 movement:" + movement);
                break;
            default:
                Debug.Log("KeepGoing MoveDir Set Error");
                break;
        }
    }

    public void Arrived()
    {
        //print("도착!");
        isrm = false;
        movement = Vector2.zero;

        //남은 큐가 있는지 확인해서 처리
        if (moveSets.Count == 0)
        {
            //남은 큐가 없음.  그럼 이동 완전 끝~!
            //print("!!완전 도착!!");
            isMoveSetOn = false;
            //최종 이동 끝나면, 
            //Idle방향 따로 설정해줄 수 있음.
            SettingIdleDirection();
        }
        //다시 출발. 딜레이타임 고려해서 출발해야한다. 
        else
        {
            //print("다시 출발!");
            if (curMoveSet.delayTime != 0f) StartCoroutine(StartNextMoveAfterDelayTime(curMoveSet.delayTime));
            else StartNextMove();
        }
    }

    public void MakeWalkingAnimation()
    {
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        if (!isJustTurnCompleted) animator.SetFloat("Speed", movement.sqrMagnitude);    //제자리 회전이 아닐 시에만 무브먼트 기반으로 스피드 설정.  
    }

    public void JustTurn()
    {
        if (!isJustTurnCompleted)
        {
            isJustTurnCompleted = true;
            animator.SetFloat("Speed", 1f);
            //Debug.Log("회전!! Speed = 1f");
        }
        else
        {
            isArrived = true;
            //isrm = false;
            animator.SetFloat("Speed", 0f);
            //Debug.Log("회전 완료. Speed = 0f");
        }
    }

    public void SettingIdleDirection()
    {
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

    public void StartNextMove()
    {
        isrm = true;    //리모트이동 시작 스위치 ON
        isArrived = false;
        isJustTurnCompleted = false;
        curMoveSet = moveSets.Dequeue();
        SetDestinationPos(curMoveSet);
    }

    IEnumerator StartNextMoveAfterDelayTime(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        StartNextMove();
    }
    #endregion

    #region ForFollow

    //따라갈 캐릭터와 현 엔피씨의 거리 체커 - 미로 전용
    public bool IsItFar(Vector2 pos)
    {             
        desPos = pos;
        //desPos = desObj.GetComponent<KeyInput_Controller>().이전위치;        
        xDis = desPos.x - transform.position.x;
        yDis = desPos.y - transform.position.y;

            if (Mathf.Abs(xDis) > 도착범위 || Mathf.Abs(yDis) > 도착범위) return true;           
            else return false;        
    }

    //따라갈 캐릭터와 현 엔피씨의 거리 체커 - NPC가 밀치는거 수정하려고. 
    public bool IsItFar2(Vector2 pos)
    {
        desPos = pos;
        //desPos = desObj.GetComponent<KeyInput_Controller>().이전위치;        
        xDis = desPos.x - transform.position.x;
        yDis = desPos.y - transform.position.y;

        //Debug.Log(desPos+", "+transform.position);

        if (Mathf.Abs(xDis) > 1f || Mathf.Abs(yDis) > 1f)
        {
            //Debug.Log(xDis + ", " + yDis);
            //Debug.Log("플레이어가 NPC가 아닌 쪽으로 이동했어요! xDis:" + xDis + ", yDis:" + yDis + ", true, desPos:" + pos + ", npcPos:" + transform.position);

            return true;
        }
        else return false;
    }

    //따라갈 캐릭터와 현 엔피씨의 거리 체커 - 다이얼로그 호출 처리 전용
    public bool IsItFar_ForDialog()
    {
        xDis = desPos.x - transform.position.x;
        yDis = desPos.y - transform.position.y;

        if (Mathf.Abs(xDis) > 도착범위 || Mathf.Abs(yDis) > 도착범위) return true;
        else return false;
    }

    //따르기 시작 - 다이얼로그로 호출
    public void StartFollowMode(Vector2 vector2, float 범위, Dialogue.DialogueSet.Details.NewAnimationSettings.EndDir endDir)
    {
        isItFromDialog = true;
        desPos = vector2;

        animator.SetInteger("Direction", (int)endDir);

        도착범위 = 범위;

        //출발 보고 처리 
        DialogueManager.Instance.isEndedMoveAnimation_ForNew = false;
    }

    //실제 이동, SetIdleDir
    public void KeepGoing_Follow()
    {
        xDis = desPos.x - transform.position.x;
        yDis = desPos.y - transform.position.y;

        float x, y;        

        //Left
        if (xDis < 0)
        {
            if (xDis > -도착범위) x = 0f;
            else x = -이동속도;
            //animator.SetInteger("Direction", 3);
        }
        //Right
        else if (xDis > 0)
        {
            if (xDis < 도착범위) x = 0f;
            else x = 이동속도;
            //animator.SetInteger("Direction", 2);
        }
        else
        {
            //xDis == 0 인 경우.                
            x = 0f;
        }
        //Down
        if (yDis < 0)
        {
            if (yDis > -도착범위) y = 0f;
            else y = -이동속도;
            //animator.SetInteger("Direction", 0);
        }
        //Up
        else if (yDis > 0)
        {
            if (yDis < 도착범위) y = 0f;
            else y = 이동속도;
            //animator.SetInteger("Direction", 1);
        }
        else
        {
            //yDis == 0 인 경우.                
            y = 0f;
        }

        if (movement.y == -1)
        {
            animator.SetInteger("Direction", 0);            
        }
        else if (movement.y == 1)
        {
            animator.SetInteger("Direction", 1);            
        }
        else if (movement.x == 1)
        {
            animator.SetInteger("Direction", 2);            
        }
        else if (movement.x == -1)
        {
            animator.SetInteger("Direction", 3);            
        }

        movement = new Vector2(x, y);

        MakeWalkingAnimation();
    }

    //실제 이동, SetIdleDir - 다이얼로그 호출 전용
    public void KeepGoing_Follow_ForDialog()
    {
        xDis = desPos.x - transform.position.x;
        yDis = desPos.y - transform.position.y;

        float x, y;

        //Left
        if (xDis < 0)
        {
            if (xDis > -도착범위) x = 0f;
            else x = -이동속도;
        }
        //Right
        else if (xDis > 0)
        {
            if (xDis < 도착범위) x = 0f;
            else x = 이동속도;
        }
        else
        {
            //xDis == 0 인 경우.                
            x = 0f;
        }
        //Down
        if (yDis < 0)
        {
            if (yDis > -도착범위) y = 0f;
            else y = -이동속도;
        }
        //Up
        else if (yDis > 0)
        {
            if (yDis < 도착범위) y = 0f;
            else y = 이동속도;
        }
        else
        {
            //yDis == 0 인 경우.                
            y = 0f;
        }

        //도착 보고 처리 
        if (x == 0f && y == 0f)
        {
            DialogueManager.Instance.isEndedMoveAnimation_ForNew = true;
        }

        movement = new Vector2(x, y);

        MakeWalkingAnimation();
    }

    #endregion
}
