using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Vector2 movement, rayDir, rayPosition;
    public GameObject scanObject;


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
    public bool followMode, isItFromDialog;
    public GameObject desObj;
    public float xDis, yDis;
    public Vector2 desPos;
    public float 도착범위;
    public float 이동속도;
    //public Vector2 이전위치;
    public Vector2 이번프레임이동량;

    //피격여부
    public bool isGetHit, is3SecPassed, isDied;
    public Vector3 getHitJumpDesPos;
    //피격효과음
    public AudioClip sfxClip;

    //플레이어 캐릭터 정보
    public PlayerStat playerStat;



    private void Awake()
    {
        moveSets = new Queue<Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveSet>();
    }

    public void Start()
    {
        도착범위 = 0.8f;
        이동속도 = 1f;

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
            if (isMoveSetOn)
            {
                //이동 처리
                MoveMaker(curMoveSet);
            }
        }
        else
        {
            //Following 구현 부분
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

            if (!isControllable) return;

            //다이얼로그가 끝나면 조작권 줌. isControllable. <-- 요고. 

            //사용자 입력값 수집.
            movement.x = DialogueManager.Instance.isDialogueActive ? 0 : Input.GetAxisRaw("Horizontal");
            movement.y = DialogueManager.Instance.isDialogueActive ? 0 : Input.GetAxisRaw("Vertical");

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

            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);

            //스캔 발동. Space키 감지 처리 부분. Dialogue가 실행중일땐 감지 불가.
            if (Input.GetKeyDown(KeyCode.Space) && scanObject != null && DialogueManager.Instance.isDialogueActive == false)
            {
                print("scanObject: " + scanObject);
                //다이얼로그 발동시키자.
                scanObject.GetComponent<Object>().TriggerDialogue();
            }
        }
}

private void FixedUpdate()
    {
        //만약 미로맵일 경우! 
        //쳐다보는쪽으로 랜턴 비추기. 
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Miro")
        {
            GameObject handLight = gameObject.GetComponentInChildren<HandLight>().gameObject;
            Transform transform = handLight.transform;

            if (rayDir == Vector2.down)
            {
                transform.rotation = Quaternion.Euler(0, 0, -180f);
            }
            else if(rayDir == Vector2.up)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if(rayDir == Vector2.left)
            {
                transform.rotation = Quaternion.Euler(0, 0, 90f);
            }
            else if (rayDir == Vector2.right)
            {
                transform.rotation = Quaternion.Euler(0, 0, -90f);
            }
        }

        //이전위치 = transform.position;
        이번프레임이동량 = movement * movespeed * Time.fixedDeltaTime;

        /*
        //죽었는가 ㅠ 
        if (PlayerStat.instance.hP <= 0&& !isDied)
        {
            PlayerStat.instance.hP = 0f;
            //쭈금!!!
            print("쭈금");
            //눕기!
            //gameObject.transform.rotation = new Quaternion(0f, 0f, 90f);
            gameObject.transform.Rotate(0f, 0f, 90f);
            isDied = true;
        }
        */

        //가장먼저, 피격당했는지? 
        if (isGetHit)
        {
            transform.position = Vector3.Slerp(transform.position, getHitJumpDesPos, 0.05f);

            //print(Mathf.Floor(transform.position.x) + " vs " + Mathf.Floor(getHitJumpDesPos.x));
            if (Mathf.Floor(transform.position.x) == Mathf.Floor(getHitJumpDesPos.x) && !isDied)
            {
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                isGetHit = false;
                isControllable = true;
                StopCoroutine(ThreeSecChecker());
                is3SecPassed = false;
            }
            //어떠한 예외적인 상황으로 인해, 피격모션에서 못빠져나오고있을경우를 위해서 2초가 지나면 강제로 피격모션 종료시켜주는 부분.
            else if (is3SecPassed && !isDied)
            {
                print("is3Sec passed");
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                isGetHit = false;
                isControllable = true;
            }
        }
        else
        {
            //실질적 이동
            rb.MovePosition(rb.position + movement * movespeed * Time.fixedDeltaTime);
        }




        //Ray
        rayPosition = new Vector2(rb.position.x, rb.position.y+0.4f);
        Debug.DrawRay(rayPosition, rayDir, Color.green, 0.7f);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(rayPosition, rayDir, 0.7f, LayerMask.GetMask("Object"));

        if (raycastHit2D.collider != null)
            scanObject = raycastHit2D.collider.gameObject;
        else
            scanObject = null;
    }


    #region For RemoteMode
    public void MoveAnimStart(Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData animData)
    {
        isMoveSetOn = true;

        this.animData = animData;

        moveSets.Clear();

        //Debug.Log("animData.moveSet[] Length: " + animData.moveSet.Length);

        //여기서 moveSet을 차례차례 큐에 넣기. 
        foreach (Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.MoveSet moveSet in animData.moveSet)
        {
            moveSets.Enqueue(moveSet);
        }
        //시작부터 등록되어있는 큐가 없으면?  등록을 안한것이니, 그냥 패스 ~!
        if (moveSets.Count == 0)
        {
        //    Debug.Log("moveSet 큐가 비어있습니다.");
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
                //Debug.Log("제자리 돌기");                
                break;
            default:
                Debug.Log("최종목적지 설정 실패");
                destinationPos = Vector2.zero;
                break;
        }
        //Debug.Log(moveSet.dir+", "+moveSet.distance+"목적지:"+destinationPos);
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
        //print(curMoveSet.dir+", "+curMoveSet.distance+" 도착!");
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
            //print("다시 출발! 딜레이타임: " + curMoveSet.delayTime + ", 방향: " +curMoveSet.dir+ ", 거리: " + curMoveSet.distance);
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
           // Debug.Log("회전!! Speed = 1f");
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
            //Debug.Log("마지막 바라볼 곳 : Down");
        }
        else if (animData.endDir == Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.EndDir.Up)
        {
            animator.SetInteger("Direction", 1);
            //Debug.Log("마지막 바라볼 곳 : Up");
        }
        else if (animData.endDir == Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.EndDir.Right)
        {
            animator.SetInteger("Direction", 2);
            //Debug.Log("마지막 바라볼 곳 : Right");
        }
        else if (animData.endDir == Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData.EndDir.Left)
        {
            animator.SetInteger("Direction", 3);
            //Debug.Log("마지막 바라볼 곳 : Left");
        }
    }

    public void StartNextMove()
    {        
        isrm = true;    //리모트이동 시작 스위치 ON
        isArrived = false;
        isJustTurnCompleted = false;
        curMoveSet = moveSets.Dequeue();
        SetDestinationPos(curMoveSet);
        //print("출발! 딜레이타임: " + curMoveSet.delayTime + ", 방향: " + curMoveSet.dir + ", 거리: " + curMoveSet.distance);
    }

    IEnumerator StartNextMoveAfterDelayTime(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        StartNextMove();
    }


    #endregion


    #region ForFollow

    //따라갈 캐릭터와 현 엔피씨의 거리 체커(나중에 필요시 사용) -  상대 이번 프레임 이동량, 이동 총 량 처리해서 이동하는 부분 구현 필요함. NPC스크립트 참고~!Update부분 위주로. NPC에서는 여기 FixedUpdate()에서 이번 프레임 이동량 알아갔다. 
    public bool IsItFar(GameObject desObj)
    {
        desPos = desObj.transform.position;
        xDis = desPos.x - transform.position.x;
        yDis = desPos.y - transform.position.y;

        if (Mathf.Abs(xDis) > 도착범위 || Mathf.Abs(yDis) > 도착범위) return true;
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
        도착범위 = 범위;
        animator.SetInteger("Direction", (int)endDir);

        //출발 보고 처리 
        DialogueManager.Instance.isEndedMoveAnimation_ForNew = false;
    }

    //실제 이동, SetIdleDir
    public void KeepGoing_Follow()
    {
        //print("KeepGoing_Follow");

        xDis = desPos.x - transform.position.x;
        yDis = desPos.y - transform.position.y;

        float x, y;

        //Left
        if (xDis < 0)
        {
            if (xDis > -도착범위) x = 0f;
            else x = -이동속도;
            //animator.SetInteger("Direction", 3);
            //print("3");
        }
        //Right
        else if (xDis > 0)
        {
            if (xDis < 도착범위) x = 0f;
            else x = 이동속도;
            //animator.SetInteger("Direction", 2);
            //print("2");
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
            //print("0");
        }
        //Up
        else if (yDis > 0)
        {
            if (yDis < 도착범위) y = 0f;
            else y = 이동속도;
            //animator.SetInteger("Direction", 1);
            //print("1");
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


    #region 피격
    public void GetHit(Vector3 desPos, string where)
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0.5f, 1f);
        getHitJumpDesPos = desPos;
        isGetHit = true;
        isControllable = false;        
        //효과음
        gameObject.GetComponent<AudioSource>().volume = 0.2f;
        gameObject.GetComponent<AudioSource>().PlayOneShot(sfxClip);

        //HP 변동
        //죽었는가 ㅠ 
        if ((playerStat.hP*0.1f) <= 0.2f && !isDied)
        {
            playerStat.hP = 0f;
            //쭈금!!!
            print("쭈금");
            //심박 삐이이
            FindObjectOfType<HeartBeater>().beatType = Dialogue.DialogueSet.Details.BeatBeat.BeatType.beatType0;
            //눕기!
            if (where == "Left")
                gameObject.transform.Rotate(0f, 0f, 90f);
            else if (where == "Right")
                gameObject.transform.Rotate(0f, 0f, -90f);
            else
                Debug.Log("방향설정 다시 해주세요");
            //잠시 콜라이더 없애고
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            Debug.Log(gameObject.GetComponent<CircleCollider2D>().enabled);
            gameObject.GetComponent<KeyInput_Controller>().isDied = true;            
            gameObject.GetComponent<KeyInput_Controller>().animator.SetFloat("Speed", 0f);
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);


            //사망시 팝업효과 실행
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Field1_Obstacle")
                FindObjectOfType<MiniGameManager>().StartGameOverPopup();
        }
        else
        {
            playerStat.hP -= 2;
        }
        
        print(playerStat.hP);

        //PlayerStat.instance.hP -= 0.2f;
        //만일을 위해 카운터 따로 동작. 
        StartCoroutine(ThreeSecChecker());
    }

    IEnumerator ThreeSecChecker()
    {
        is3SecPassed = false;
        //1초로 변경
        yield return new WaitForSeconds(1f);
        is3SecPassed = true;
    }
    #endregion
}



