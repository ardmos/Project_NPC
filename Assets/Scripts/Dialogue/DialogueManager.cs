using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : DontDestroy<DialogueManager>
{
    public Dialogue.DialogueSet[] dialogueSets;
    public Animator dialog_animator;
    public RuntimeAnimatorController stable, slide;
    //dialogSentence와 dialogMonologSentence. Name속성값을 기준으로 curDialogBox에 적절한 텍스트박스를 넣어준다.
    public Text dialogObjName, curDialogBox, dialogSentence, dialogMonologSentence;
    public Image dialogPortrait_Left, dialogPortrait_Right, dialogPortrait_InDialogue, cutSceneImage;
    public ChoiceBox choiceBox;
    public AudioSource audioSource;
    public AudioSystem audioSystem;
    //Continue버튼
    public GameObject continueBtn;
    //경찰1 오브젝트
    public GameObject 경찰1;

    //다이얼로그 열려있는지 체크 
    [HideInInspector]
    public bool isDialogueActive;
    [HideInInspector]
    //ctrl키 눌렸는지 체크. 
    public bool isSpaceKeyDowned = false;
    [HideInInspector]
    //문자출력 도중인지, 끝났는지. 
    public bool isDuringTyping = false;

    [Header("- 대화 보따리 저장소."), Space(20)]
    Dictionary<int, Dialogue> dialogueData_Dic;
    [Header("- 초상화 저장소. 사용될 초상화들을 모두 이곳에 저장해주세요.~  김탐정, 천지현, 백강, 베라, 플라키, 청마, 대학생4, 대학생5, 오상식, 흰강아지수인, 흰상아지수인_후드, 경찰1, 경찰2")]
    public Sprite[] portraits;
    [Header("- 타이핑 효과음 저장소. 마찬가지로 사용될 효과음들을 모두 저장해주세요. ^^")]
    public AudioClip[] typingSounds;


    Dialogue dialogue;
    Queue<Dialogue.DialogueSet> dialogueSetsQue;

    [HideInInspector]
    public Dialogue.DialogueSet curDialogSet, curDialogSet_ForChoiceBox;
    public int curDialogSetCountNumber;
    public int curStoryId;
    public string curStorySmallTitle;

    bool activeChoiceBox;
    //선택상자에대한 응답에서 문장 빨리넘기기 했을 시 사용할 문장.  이미 도도도 찍고있던 문장을 담고있다.
    string printingSentence;
    //다이얼로그 문장 없이 애니메이션만 실행시키기 위한 부분.    
    public bool isDuringMoveAnimation, isDuringMoveAnimation_ForNew;
    public bool isEndedMoveAnimation, isEndedMoveAnimation_ForNew;
    public List<NPC> nPCs = new List<NPC>();
    public List<KeyInput_Controller> keyInput_Controllers = new List<KeyInput_Controller>();
    //다이얼로그 딜레이 타임 
    bool isdelayTimeCompleted = false;
    bool isStartedWatingDelayTime = false;
    //다이얼로그 사운드이펙트만!! LifeTime 으로 재생중인지!
    bool isSFXDialogLifeTime = false;
    //첫 다이얼로그 열리고 0.5초 지났는지 체크
    public bool is05Sec = false;

    //Sentence 없이 다른것만 진행할 때 true되는 변수
    public bool goreturn;

    //다이얼로그 넘기는 효과음 여러번 나오는걸 막기 위한 변수
    private bool dialogueFlipSFXBlocker;


    #region For Signleton <<-- DontDestory 사용해서 구현., OnAwake()
    //기존 싱글턴 <<-- DontDestory 사용해서 구현했기때문에 주석 처리.
    //public static DialogueManager instance;

    override protected void OnAwake()
    {
        //싱글턴
        //instance = this;
        dialogueData_Dic = new Dictionary<int, Dialogue>();
    }
    #endregion

    // Start is called before the first frame update
    override protected void OnStart()
    {
        dialogueSetsQue = new Queue<Dialogue.DialogueSet>();

        if (SceneManager.GetActiveScene().name == "Field1_Obstacle" || SceneManager.GetActiveScene().name == "Field1_Obstacle2")
        {
            Debug.Log("장애물맵에서는 다이얼로그가 필요 없지!");
            EndDialogue();
        }
    }

    private void OnEnable()
    {
        //Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("OnSceneLoaded: " + scene.name);
        //Debug.Log(mode);
        //새롭게 읽어오기.
        AddDialogueData();
    }

    void OnDisable()
    {
        //Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && is05Sec == true) isSpaceKeyDowned = true;

        if (isSpaceKeyDowned && isDialogueActive)
        {
            //Debug.Log("넘기기버튼 눌림");

            //Continue버튼 비활성화
            continueBtn.SetActive(false);

            //빨리넘기기 처리
            if (isDuringTyping) PrintAtOnce(curDialogSet);
            else
            {
                if (!isStartedWatingDelayTime && !choiceBox.isChoiceBox && !goreturn && !dialogueFlipSFXBlocker)
                {
                    //다이얼로그 넘기는 효과음
                    //Debug.Log("newlog: isStartedWatingDelayTime: " + isStartedWatingDelayTime);
                    AudioSystem.Instance.PlayDialogueFlipSFX();

                }

                DisplayNextSentence();
            }
        }

        #region 다이얼로그는 이동 애니메이션 끝나길 기다렸다가 출력하기.
        //애니메이션 중이면 대기. 
        //애니메이션이 끝났으면 DisplayNextSentence(); 진행.
        if (isDuringMoveAnimation)
        {
            //string objname = null;

            foreach (NPC npc in nPCs)
            {
                //objname = npc.gameObject.name;
                if (npc.isMoveSetOn == false)
                {
                    //print(objname + "Arrived");
                    isEndedMoveAnimation = true;
                }
                else
                {
                    //print(objname + "Not Arrived  이동애니메이션 진행중.");
                    isEndedMoveAnimation = false;
                    return;
                }
            }
            foreach (KeyInput_Controller player in keyInput_Controllers)
            {
                //objname = player.gameObject.name;
                if (player.isMoveSetOn == false)
                {
                    isEndedMoveAnimation = true;
                    //print(objname + "플레이어 Arrived, isEndedMoveAnimation=" + isEndedMoveAnimation);
                }
                else
                {
                    //print(objname + "플레이어 Not Arrived  이동애니메이션 진행중.");                    
                    isEndedMoveAnimation = false;
                    return;
                }
            }

            if (isEndedMoveAnimation)
            {
                //Debug.Log("isEndedMoveAnimation is True!");
                //혹~시! 사운드LifeTime중인지?
                if (!isSFXDialogLifeTime)
                {
                    //print(objname + " 이동애니메이션 도착.끝");
                    isDuringMoveAnimation = false;
                    DisplayNextSentence();
                }
            }
            else
            {
                //Debug.Log("isEndedMoveAnimation is False!");
            }
        }

        //New Anim
        if (isDuringMoveAnimation_ForNew)
        {
            //isEndedMoveAnimation_ForNew 각각의 오브젝트에서 호출해줌. New는 동시에 여럿이 움직일 가능성이 없기 때문에 개별 처리.
            if (isEndedMoveAnimation_ForNew)
            {

                //혹~시! 사운드LifeTime중인지?
                if (!isSFXDialogLifeTime)
                {
                    isDuringMoveAnimation_ForNew = false;
                    DisplayNextSentence();
                }
            }
            else
            {
                //Debug.Log("isEndedMoveAnimation_ForNew is False!");
            }
        }
        #endregion
    }

    public void AddDialogueData()
    {
        foreach (AttachThis attachThis in FindObjectsOfType<AttachThis>())
        {
            foreach (Dialogue item in attachThis.dialogues)
            {
                //딕셔너리에 넣는 과정
                //이미 id값이 존재할경우, 스킵
                if (dialogueData_Dic.ContainsKey(item.storyId))
                {
                    //스킵
                    Debug.Log("id값이 이미 존재" + item.storyId + ", " + item.smallTitle_);
                }
                else
                {
                    dialogueData_Dic.Add(item.storyId, item);
                }
            }
        }
    }

    public void OnBtnClickedByMouse()
    {
        isSpaceKeyDowned = true;
    }

    //다이얼로그 시작
    public void StartDialogue(int objid)    //다이얼로그의 다양한 부분을 초기화. 
    {
        //해당 아이디값 개체 검색       
        if (dialogueData_Dic.ContainsKey(objid))    //해당 id값을 가진 개체가 존자한다면
        {
            //그 개체의 Dialogue 클래스를 꺼내와서
            dialogue = dialogueData_Dic[objid];
            curStorySmallTitle = dialogue.smallTitle_;
            curStoryId = dialogue.storyId;
            curDialogSetCountNumber = 0;    //넘버 초기화. 

            //그 중 dialogueSet 배열에 접근한다.  배열을 큐로 변환시키기 위함. 편의를 위해서. 
            dialogueSets = dialogue.dialogue;

            //해당 dialogueSet 배열의 문자열들 길이 확인 후 처리.  일단 보류!
            //dialogueSets = SentenceLengthChecker(dialogueSets);

            //얻어낸 dialogueSet을 Queue에 순차적으로 Enqueue 한다.
            dialogueSetsQue.Clear();
            foreach (Dialogue.DialogueSet item in dialogueSets) dialogueSetsQue.Enqueue(item);
        }
        else Debug.Log("정보가 없는 녀석입니다.");

        //말풍선팝업일수도 있으니.  id넘버 팔십만 이상인지?  아니면 그냥 다이얼로그 진행.
        if (objid >= 800000)
        {
            FindObjectOfType<TalkBalloonManager>().ConnectTB(objid, dialogueSetsQue);
            return;
        }
        else DisplayNextSentence();


        isDialogueActive = true;

        //Continue버튼 비활성화
        continueBtn.SetActive(false);
        //다이얼로그 처음 대화 시작하면, 0.5초정도 있다가 빨리넘기기 작동 가능하게끔.
        StartCoroutine(Count005Sec());

        //다이얼로그 시작하면 플레이어 컨트롤 권한 뺏음.       
        foreach (KeyInput_Controller item in GameObject.FindObjectsOfType<KeyInput_Controller>())
        {
            //컨트롤권한도 뺐고
            item.isControllable = false;

            //진행중이던 무빙 애니메이션도 끝냄
            item.movement = Vector2.zero;
            item.animator.SetFloat("Horizontal", item.movement.x);
            item.animator.SetFloat("Vertical", item.movement.y);
            item.animator.SetFloat("Speed", item.movement.sqrMagnitude);
        }
    }

    //문자 출력 들어감.
    public void DisplayNextSentence()
    {
        dialogueFlipSFXBlocker = true;

        isSpaceKeyDowned = false;

        //혹시 딜레이 대기중이라면 다음 다이얼로그 가면 안됨.
        if (isStartedWatingDelayTime)
        {
            print("딜레이타임 기다리는중!");
            return;
        }

        ///
        //선택지 흐름
        //1.팝업 오픈 , 2.답안 선택, 3.결과문 출력, 4.결과문에 엔피씨가 응답 (분기점 스탯 적용), 5.정해진 루트 진행
        ///
        if (choiceBox.isChoiceBox)
        {
            //초이스박스 열려있는 상태에서는 다음 다이얼로그 가면 안됨. 
            return;
        }

        if (dialogueSetsQue.Count == 0)
        {
            EndDialogue();
            return;
        }

        //curDialogSet 갱신.
        curDialogSet = dialogueSetsQue.Dequeue();
        //dialogueSetName을 설정해주자.  Story아이디_카운트
        curDialogSet.dialogueSetName = curStoryId.ToString() + "_" + curDialogSetCountNumber.ToString();
        curDialogSetCountNumber++;  //1씩 더해줌. 
        //선택상자 전용 현재 다이얼로그. (점프때문에 얘네 전용이 필요해짐)
        curDialogSet_ForChoiceBox = curDialogSet;

        BatchService(curDialogSet);
    }

    //배치서비스 시작합니다.   스플릿서비스는 안녕!  일괄 처리.
    void BatchService(Dialogue.DialogueSet dialogueSet)
    {
        //여기서 dialogueSet에 입력된 정보에 따라 새 다이얼로그창의 정보를 설정.

        //Name값에 따라 알맞은 텍스트박스 설정해주기.
        if (dialogueSet.name == Dialogue.DialogueSet.Names.빈칸)
        {
            //독백인 경우. 
            curDialogBox = dialogMonologSentence;
            dialogSentence.text = "";
            dialogObjName.text = "";
        }
        else
        {
            //독백이 아닌 경우 
            curDialogBox = dialogSentence;
            dialogMonologSentence.text = "";
        }

        //다이얼로그 시작 딜레이 타임 부여
        if (dialogueSet.detail.delayTime != 0f)
        {
            //0이 아니면, 딜레이타임을 설정한것. 설정한 시간만큼 기다렸다가 실행시켜준다. 
            //설정한시간 다 기다렸는지
            if (isdelayTimeCompleted)
            {
                isdelayTimeCompleted = false;
                //다이얼로그 실행.
            }
            else
            {
                //딜레이타임 코루틴 실행, 
                StartCoroutine(WaitTime(dialogueSet.detail.delayTime));
                //return.
                return;
            }
        }

        //기본 글자 속도.
        if (dialogueSet.detail.letterSpeed == 0f) dialogueSet.detail.letterSpeed = 0.94f;

        //Sentence 비어있을경우, 발동할 이동 애니메이션 or 기타 애니메이션 or 사운드가 있는지 확인 후 없으면 그냥 패스! 
        if (dialogueSet.sentence == "")
        {
            //얘네들 sentence가 비어있을 때, 애니메이션이든 뭐든 있으면 그것만 하고 되돌아가기...있다 true 변수 
            goreturn = false;

            //발동할 애니메이션이 있는지? 
            if (dialogueSet.detail.oldAnimationSettings.activateObjAnimate && !dialogueSet.detail.newAnimationSettings.activeNewAnimate)
            {
                //발동할 애니메이션이 존재하면, 해당 애니메이션이 끝나길 기다렸다가  return.                
                foreach (Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData objAnimData in dialogueSet.detail.oldAnimationSettings.objectAnimationDatas)
                {
                    //NPC.cs가 있는 경우.(NPC인 경우) or KeyInput_Controller가 있는 경우.(Player인 경우) 알아서 처리. 
                    if (objAnimData.objToMakeMove.TryGetComponent<NPC>(out NPC nPC))
                    {
                        //print("it's NPC moving");                      
                        nPCs.Add(nPC);
                        nPC.MoveAnimStart(objAnimData);
                    }
                    else if (objAnimData.objToMakeMove.TryGetComponent<KeyInput_Controller>(out KeyInput_Controller keyInput_Controller))
                    {
                        //print("it's Player moving");
                        keyInput_Controllers.Add(keyInput_Controller);
                        keyInput_Controller.MoveAnimStart(objAnimData);
                    }
                }
                isDuringMoveAnimation = true;
                goreturn = true;
            }
            if (!dialogueSet.detail.oldAnimationSettings.activateObjAnimate && dialogueSet.detail.newAnimationSettings.activeNewAnimate)
            {
                //New
                foreach (Dialogue.DialogueSet.Details.NewAnimationSettings.NewAnimData newAnimData in dialogueSet.detail.newAnimationSettings.newAnimDatas)
                {
                    //New
                    if (newAnimData.objToMove.TryGetComponent<NPC>(out NPC nPC))
                    {
                        nPC.StartFollowMode(newAnimData.destinationPos, 0.1f, newAnimData.endDir);
                    }
                    else if (newAnimData.objToMove.TryGetComponent<KeyInput_Controller>(out KeyInput_Controller keyInput_Controller))
                    {
                        keyInput_Controller.StartFollowMode(newAnimData.destinationPos, 0.1f, newAnimData.endDir);
                    }
                }
                isDuringMoveAnimation_ForNew = true;
                goreturn = true;
            }

            //Object 기타 애니메이션
            if (dialogueSet.detail.etcAnimationSettings.etcAnimSets.Length > 0)   //실행/정지 시킬 오브젝트 갯수를 0보다 큰 수로 설정했을 경우
            {
                foreach (Dialogue.DialogueSet.Details.EtcAnimationSettings.EtcAnimSet etcAnimSet in dialogueSet.detail.etcAnimationSettings.etcAnimSets)
                {
                    if (etcAnimSet.activateOrDeActiveObjAnimate == Dialogue.DialogueSet.Details.EtcAnimationSettings.EtcAnimSet.TOTO.실행)
                    {
                        //실행
                        etcAnimSet.objectAnimationData.obj.SetBool(etcAnimSet.objectAnimationData.paramName, true);
                    }
                    else if (etcAnimSet.activateOrDeActiveObjAnimate == Dialogue.DialogueSet.Details.EtcAnimationSettings.EtcAnimSet.TOTO.정지)
                    {
                        //정지
                        etcAnimSet.objectAnimationData.obj.SetBool(etcAnimSet.objectAnimationData.paramName, false);
                    }
                }
                goreturn = true;
            }


            //발동할 감정표현 애니메이션이 있는지?
            if (dialogueSet.detail.emotionSettings.activeEmotion)
            {
                Dialogue.DialogueSet.Details.EmotionSettings emotionSettings = dialogueSet.detail.emotionSettings;

                foreach (Animator animator in emotionSettings.object_.GetComponentsInChildren<Animator>())
                {
                    if (animator.gameObject.name == "EmotionSprite")
                    {
                        Debug.Log(animator + ", " + emotionSettings.animation.ToString());
                        animator.SetTrigger(emotionSettings.animation.ToString());
                    }
                }
                goreturn = true;
            }


            //발동할 사운드 이펙트가 있는지?
            if (dialogueSet.detail.sFXSettings.enableSFX && dialogueSet.detail.sFXSettings.dialogueLifeTime != 0)
            {
                //사운드 재생 후 
                audioSystem.DialogSFXHelper(dialogueSet);
                //설정한 LifeTime만큼 기다렸다가 다음 다이얼로그 호출. 
                StartCoroutine(WaitLifeTime(dialogueSet.detail.sFXSettings.dialogueLifeTime));
                goreturn = true;
            }

            //심장박동이펙트~~~?
            if (dialogueSet.detail.HeartBeatSettings.activeBeat)
            {
                if (GameObject.FindObjectOfType<HeartBeater>() != null)
                {
                    print("박동박동~!");
                    FindObjectOfType<HeartBeater>().beatType = dialogueSet.detail.HeartBeatSettings.beatType;
                }
                else
                {
                    print("하트비터가 없습니다~");
                }
            }

            if (goreturn)
            {
                goreturn = false;
                return;
            }
            else
            {
                DisplayNextSentence();
                return;
            }
        }

        //이름
        switch (dialogueSet.name)
        {
            case Dialogue.DialogueSet.Names.김탐정:
                dialogObjName.text = "김승훈 탐정";
                break;
            case Dialogue.DialogueSet.Names.천형사:
                dialogObjName.text = "천지현 형사";
                break;
            case Dialogue.DialogueSet.Names.경찰1:
                dialogObjName.text = "경찰1";
                break;
            case Dialogue.DialogueSet.Names.경찰2:
                dialogObjName.text = "경찰2";
                break;
            case Dialogue.DialogueSet.Names.익명:
                dialogObjName.text = "??";
                break;
            case Dialogue.DialogueSet.Names.경비병:
                dialogObjName.text = "경비병";
                break;
            case Dialogue.DialogueSet.Names.농부:
                dialogObjName.text = "농부";
                break;
            case Dialogue.DialogueSet.Names.데릭:
                dialogObjName.text = "데릭";
                break;
            case Dialogue.DialogueSet.Names.뱅크:
                dialogObjName.text = "뱅크";
                break;
            case Dialogue.DialogueSet.Names.학생1:
                dialogObjName.text = "학생1";
                break;
            case Dialogue.DialogueSet.Names.학생2:
                dialogObjName.text = "학생2";
                break;
            case Dialogue.DialogueSet.Names.학생3:
                dialogObjName.text = "학생3";
                break;
            case Dialogue.DialogueSet.Names.학생4:
                dialogObjName.text = "학생4";
                break;
            case Dialogue.DialogueSet.Names.학생5:
                dialogObjName.text = "학생5";
                break;
            case Dialogue.DialogueSet.Names.빈칸:
                dialogObjName.text = "";
                break;
            case Dialogue.DialogueSet.Names.하얀개1:
                dialogObjName.text = "하얀개1";
                break;
            case Dialogue.DialogueSet.Names.하얀개2:
                dialogObjName.text = "하얀개2";
                break;
            case Dialogue.DialogueSet.Names.하얀개3:
                dialogObjName.text = "하얀개3";
                break;
            case Dialogue.DialogueSet.Names.수상한개:
                dialogObjName.text = "수상한 개";
                break;
            case Dialogue.DialogueSet.Names.GM청마:
                dialogObjName.text = "GM청마";
                break;
            case Dialogue.DialogueSet.Names.베라:
                dialogObjName.text = "베라";
                break;
            default:
                break;
        }


        #region Details

        //스타일
        StartAnimByStyle((int)dialogueSet.detail.styles);

        //초상화 세팅
        //초상화(좌)
        if (dialogueSet.detail.portraitSettings.showLeftPortrait)
        {
            //Debug.Log("Here");
            dialogPortrait_Left.color = new Color(1, 1, 1, 1);
            dialogPortrait_Left.sprite = portraits[(int)dialogueSet.detail.portraitSettings.leftPortraitNumber]; //초상화(좌) 표정            
        }
        else
            dialogPortrait_Left.color = new Color(1, 1, 1, 0);

        //초상화(우)
        if (dialogueSet.detail.portraitSettings.showRightPortrait)
        {
            dialogPortrait_Right.color = new Color(1, 1, 1, 1);
            dialogPortrait_Right.sprite = portraits[(int)dialogueSet.detail.portraitSettings.rightPortraitNumber]; //초상화(우) 표정            
        }
        else
            dialogPortrait_Right.color = new Color(1, 1, 1, 0);


        //작은초상화 세팅
        //좌
        if (dialogueSet.detail.portraitSettings.smallPortraitR != true && dialogueSet.detail.portraitSettings.smallPortraitL)
            ShowInsideDialoguePortrait(false, portraits[(int)dialogueSet.detail.portraitSettings.leftPortraitNumber]);
        else if (!dialogueSet.detail.portraitSettings.smallPortraitR && !dialogueSet.detail.portraitSettings.smallPortraitL)
            HideInsideDialoguePortrait();
        //우
        if (dialogueSet.detail.portraitSettings.smallPortraitL != true && dialogueSet.detail.portraitSettings.smallPortraitR)
            ShowInsideDialoguePortrait(true, portraits[(int)dialogueSet.detail.portraitSettings.rightPortraitNumber]);
        else if (!dialogueSet.detail.portraitSettings.smallPortraitL && !dialogueSet.detail.portraitSettings.smallPortraitR)
            HideInsideDialoguePortrait();

        //선택팝업
        if (dialogueSet.detail.selectionPopupSettings.activateSelectionPopup)
        {
            //선택지 발동 조건 미리 만족시켜둠.  실행은 타이핑이 끝난 시점에 됨. 
            activeChoiceBox = true;
        }

        //Object 이동 애니메이션
        if (dialogueSet.detail.oldAnimationSettings.activateObjAnimate && !dialogueSet.detail.newAnimationSettings.activeNewAnimate)
        {
            //Old
            foreach (Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData objAnimData in dialogueSet.detail.oldAnimationSettings.objectAnimationDatas)
            {
                //objAnimData.objToMakeMove.MoveAnimStart(objAnimData);

                //NPC.cs가 있는 경우.(NPC인 경우) or KeyInput_Controller가 있는 경우.(Player인 경우) 알아서 처리. 

                //if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 2) return; //지금 미로(씬빌드인덱스2)씬 테스트 목적으로, 잠시 막아두기 

                if (objAnimData.objToMakeMove.TryGetComponent<NPC>(out NPC nPC))
                {
                    //print("it's NPC moving");
                    nPC.MoveAnimStart(objAnimData);
                }
                else if (objAnimData.objToMakeMove.TryGetComponent<KeyInput_Controller>(out KeyInput_Controller keyInput_Controller))
                {
                    //print("it's Player moving");
                    keyInput_Controller.MoveAnimStart(objAnimData);
                }
            }
        }
        if (!dialogueSet.detail.oldAnimationSettings.activateObjAnimate && dialogueSet.detail.newAnimationSettings.activeNewAnimate)
        {
            foreach (Dialogue.DialogueSet.Details.NewAnimationSettings.NewAnimData newAnimData in dialogueSet.detail.newAnimationSettings.newAnimDatas)
            {
                //New
                if (newAnimData.objToMove.TryGetComponent<NPC>(out NPC nPC))
                {
                    nPC.StartFollowMode(newAnimData.destinationPos, 0.1f, newAnimData.endDir);
                }
                else if (newAnimData.objToMove.TryGetComponent<KeyInput_Controller>(out KeyInput_Controller keyInput_Controller))
                {
                    keyInput_Controller.StartFollowMode(newAnimData.destinationPos, 0.1f, newAnimData.endDir);
                }
            }
        }


        //Object 기타 애니메이션
        EtcAnimation(dialogueSet);

        //Object 감정표현 애니메이션
        EmotionAnimation(dialogueSet);


        //효과음 실행(얘는 audioSystem에서 실행할지안할지 확인함... 이러면 안되는데 ㅠㅠ)
        audioSystem.DialogSFXHelper(dialogueSet);


        //배경음악 변경
        if (dialogueSet.detail.bGMSettings.audioClip != null)
        {
            audioSystem.ActivateBGM(dialogueSet.detail.bGMSettings.audioClip);
        }
        else Debug.Log("브금을 바꾸려했지만...  오디오클립이 없는걸요 ㅠㅠ!");

        //배경음악 볼륨 변경
        if (dialogueSet.detail.bGMSettings.changeVolume)
        {
            audioSystem.SetBGMVolume(dialogueSet.detail.bGMSettings.volumeValue);
        }



        //컷씬 출력 
        if (dialogueSet.detail.cutSceneSettings.activateCutScene)
        {
            cutSceneImage.color = new Color(1, 1, 1, 1);
            cutSceneImage.sprite = dialogueSet.detail.cutSceneSettings.image;
            cutSceneImage.SetNativeSize();
        }
        else cutSceneImage.color = new Color(1, 1, 1, 0);


        //글자 색깔 설정.
        if (dialogueSet.detail.fontColorSettings.changeColor)
        {
            //혹시 실수로 글씨 투명으로 설정했으면 불투명하게 처리 
            if (dialogueSet.detail.fontColorSettings.fontColor.a == 0f)
                curDialogBox.color = new Color(dialogueSet.detail.fontColorSettings.fontColor.r, dialogueSet.detail.fontColorSettings.fontColor.g, dialogueSet.detail.fontColorSettings.fontColor.b, 1);
            else
                curDialogBox.color = dialogueSet.detail.fontColorSettings.fontColor;
        }
        else curDialogBox.color = Color.white;    //기본은 검은색

        //글자 크기 설정
        if (dialogueSet.detail.fontSizeSettings.changeSize)
        {
            curDialogBox.fontSize = dialogueSet.detail.fontSizeSettings.fontSize;
        }
        else curDialogBox.fontSize = 30;

        #endregion


        //문장 도도도 출력하는 부분
        //StopAllCoroutines();
        StartCoroutine(TypeSentence(dialogueSet));
    }

    //3.결과 출력
    public void PrintTheChoiceResult(Dialogue.DialogueSet.Details.Choices choice)
    {
        //선택 결과들은 딕셔너리 형태로 저장될것임. 추후 엔딩 다르게 할 때 필요.  
        //Key값은 string으로 선택상자의 question을 넣어주고, Value값은 이후 진행된 스토리id로 어떤 선택을 했는지 번호를 저장.        
        GameManager.Instance.AddChoiceResults(curDialogSet_ForChoiceBox.detail.selectionPopupSettings.selectionPopupData.question, choice.linkedStoryDialogueIdNumber);



        //만약, 해당 선택지가 정답이라면. GameManager의 메인 스토리 id도 해당 스토리 id로 변경.
        if (choice.isItCorrectAnswer)
        {
            //GameManager.Instance.storyNumber = choice.linkedStoryDialogueIdNumber;
            GameManager.Instance.SetStoryNumber(choice.linkedStoryDialogueIdNumber);
            Debug.Log("정답! 해당 스토리 넘버로 메인 스토리가 진행됩니다.");
        }
        else Debug.Log("오답! 다시 해보세요~");

        activeChoiceBox = false;

        //5. 선택 결과에 따른 스토리다이얼로그 정보 읽어와서 열어주기. 
        StartDialogue(choice.linkedStoryDialogueIdNumber);
    }

    //기타 애니메이션 실행
    public void EtcAnimation(Dialogue.DialogueSet dialogueSet)
    {
        if (dialogueSet.detail.etcAnimationSettings.etcAnimSets.Length > 0)   //실행/정지 시킬 오브젝트 갯수를 0보다 큰 수로 설정했을 경우
        {
            foreach (Dialogue.DialogueSet.Details.EtcAnimationSettings.EtcAnimSet etcAnimSet in dialogueSet.detail.etcAnimationSettings.etcAnimSets)
            {
                if (etcAnimSet.activateOrDeActiveObjAnimate == Dialogue.DialogueSet.Details.EtcAnimationSettings.EtcAnimSet.TOTO.실행)
                {
                    //실행
                    etcAnimSet.objectAnimationData.obj.SetBool(etcAnimSet.objectAnimationData.paramName, true);
                }
                else if (etcAnimSet.activateOrDeActiveObjAnimate == Dialogue.DialogueSet.Details.EtcAnimationSettings.EtcAnimSet.TOTO.정지)
                {
                    //정지
                    etcAnimSet.objectAnimationData.obj.SetBool(etcAnimSet.objectAnimationData.paramName, false);
                }
            }
        }
    }

    //감정표현 애니메이션 실행
    public void EmotionAnimation(Dialogue.DialogueSet dialogueSet)
    {
        if (dialogueSet.detail.emotionSettings.activeEmotion)
        {
            Dialogue.DialogueSet.Details.EmotionSettings emotionSettings = dialogueSet.detail.emotionSettings;

            foreach (Animator animator in emotionSettings.object_.GetComponentsInChildren<Animator>())
            {
                if (animator.gameObject.name == "EmotionSprite")
                {
                    string triggerName;

                    switch (emotionSettings.animation.ToString())
                    {
                        case "물음표_띠용":
                            triggerName = "물음표(띠용)";
                            break;
                        case "느낌표_떠오름":
                            triggerName = "느낌표(떠오름)";
                            break;
                        case "점점점_황당":
                            triggerName = "점점점(황당)";
                            break;
                        case "빠직_화남":
                            triggerName = "빠직(화남)";
                            break;
                        case "보글보글_복잡":
                            triggerName = "보글보글(복잡)";
                            break;
                        case "느낌표_놀람":
                            triggerName = "느낌표(놀람)";
                            break;
                        case "느낌표_엄청놀람":
                            triggerName = "느낌표(엄청놀람)";
                            break;
                        default:
                            triggerName = null;
                            Debug.Log("감정애니메이션 실행하려는 부분에서 문제가 발생되었습니다.");
                            break;
                    }
                    Debug.Log(animator + ", " + triggerName);

                    animator.SetTrigger(triggerName);
                }
            }
        }
    }

    //다이얼로그 출현 방식에 따른 애니메이션 실행. 
    void StartAnimByStyle(int a)
    {
        switch (a)
        {
            case 0:
                dialog_animator.runtimeAnimatorController = stable as RuntimeAnimatorController;
                break;
            case 1:
                dialog_animator.runtimeAnimatorController = slide as RuntimeAnimatorController;
                break;
            default:
                Debug.Log("please make sure the dialogueStyle is correct");
                return;
        }
        dialog_animator.SetBool("isOpen", true);
    }

    //다이얼로그 내부 초상화
    private void ShowInsideDialoguePortrait(bool dirIsRight, Sprite portraitSprite)
    {
        //Debug.Log("Show Inside Man~");
        //내부 초상화 표현시.
        dialogPortrait_InDialogue.color = new Color(1, 1, 1, 1);
        dialogPortrait_InDialogue.sprite = portraitSprite; //초상화(내부) 스프라이트

        //내부초상화 존재시 Sentence 위치정보
        RectTransform sentenceRectTransform = curDialogBox.gameObject.GetComponent<RectTransform>();
        //Left
        sentenceRectTransform.offsetMin = new Vector2(210f, 27f);
        sentenceRectTransform.sizeDelta = new Vector2(sentenceRectTransform.sizeDelta.x, 110f);
        //내부초상화 존재시 Name 위치정보        
        //PosX, PosY
        dialogObjName.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(200f, -25f);        

        //초상화 좌우변경
        Quaternion quaternion = dialogPortrait_InDialogue.gameObject.GetComponent<RectTransform>().localRotation;

        //dog2_5, dog5 초상화는 기본 이미지 방향이 다른애들과는 반대이기 때문에, 구분하여 처리해주어야함
        if (portraitSprite.name.Contains("dog"))
        {
            if (dirIsRight)
                quaternion = Quaternion.Euler(quaternion.x, 180f, quaternion.z);
            else
                quaternion = Quaternion.Euler(quaternion.x, 0f, quaternion.z);
        }
        else
        {
            if (dirIsRight)
            {
                //Debug.Log("좌우바꾸기도전.오른쪽사진.");
                quaternion = Quaternion.Euler(quaternion.x, 0f, quaternion.z);
            }

            else
            {
                //Debug.Log("좌우바꾸기도전.왼쪽사진.");
                quaternion = Quaternion.Euler(quaternion.x, 180f, quaternion.z);
            }
        }

        dialogPortrait_InDialogue.gameObject.GetComponent<RectTransform>().localRotation = quaternion;

    }
    private void HideInsideDialoguePortrait()
    {

        //Debug.Log("Hide Inside Man~");
        //내부 초상화 미표현시.  
        dialogPortrait_InDialogue.color = new Color(1, 1, 1, 0);

        //내부초상화 존재시 Sentence 위치정보
        RectTransform sentenceRectTransform = curDialogBox.gameObject.GetComponent<RectTransform>();
        //Left
        if (dialogObjName.text == "")
        {
            //이름이 빈칸이었을 경우 문장 위치 올려주기. 
            sentenceRectTransform.sizeDelta = new Vector2(sentenceRectTransform.sizeDelta.x, 160f);
            sentenceRectTransform.offsetMin = new Vector2(60f, 27f);
            //dialogSentence.rectTransform.SetAllDirty();
        }
        else
        {
            sentenceRectTransform.sizeDelta = new Vector2(sentenceRectTransform.sizeDelta.x, 110f);
            sentenceRectTransform.offsetMin = new Vector2(60f, 27f);
        }

        //내부초상화 존재시 Name 위치정보        
        //PosX, PosY        
        dialogObjName.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(50f, -25f);
    }

    //한글자씩 도도도 찍기
    IEnumerator TypeSentence(Dialogue.DialogueSet dialogueSet)
    {
        ///출력중       
        isDuringTyping = true;

        //선택상자 팝업에서 빨리넘기기 할 때 쓰이는 변수. 
        printingSentence = dialogueSet.sentence;

        curDialogBox.text = "";
        foreach (char letter in dialogueSet.sentence.ToCharArray())
        {
            curDialogBox.text += letter;

            //타이핑 효과음 출력 부분.  스페이스는 거른다.  turnOffTypingSound 체크해제 되어있는지도 확인
            if (letter != System.Convert.ToChar(32) && dialogueSet.detail.sFXSettings.turnOffTypingSound == false)
            {
                //audioSource.PlayOneShot(typingSounds[dialogueSet.soundNumber]);
                audioSource.PlayOneShot(typingSounds[(int)dialogueSet.soundType]);
            }
            //else
            //print("스페이스 감지!");

            yield return new WaitForSeconds(1f - dialogueSet.detail.letterSpeed); //letterSpeed 받아서 처리하게끔 하자
        }

        ///출력 끝났을 때
        isDuringTyping = false;

        //선택상자 실행
        if (activeChoiceBox)
        {
            choiceBox.isChoiceBox = true;
            choiceBox.InitChioceBox(dialogueSet.detail.selectionPopupSettings.selectionPopupData.question, dialogueSet.detail.selectionPopupSettings.selectionPopupData.choices);
        }
        else
        {
            //Continue버튼 활성화 (비활성화는 다이얼로그 넘길 때, 처음 시작 때.     활성화는 여기랑 한번에 출력부분.) 
            continueBtn.SetActive(true);
            //Continue버튼 활성활될 때 dialogueFlipSFXBlocker = false 처리 
            dialogueFlipSFXBlocker = false;
        }
    }

    //다이얼로그 딜레이타임 체크
    IEnumerator WaitTime(float time)
    {
        isStartedWatingDelayTime = true;
        yield return new WaitForSeconds(time);
        isStartedWatingDelayTime = false;
        isdelayTimeCompleted = true;
        BatchService(curDialogSet);
    }

    //다이얼로그 Sentence 없이 사운드만 실행 시 LifeTime 계산.
    IEnumerator WaitLifeTime(float time)
    {
        isSFXDialogLifeTime = true;
        yield return new WaitForSeconds(time);
        isSFXDialogLifeTime = false;
        DisplayNextSentence();
    }

    //첫 다이얼로그 띄우고나서, 0.5초 카운트
    IEnumerator Count005Sec()
    {
        is05Sec = false;
        //print("CountStarted , is05Sec=" + is05Sec);
        yield return new WaitForSeconds(0.5f);
        is05Sec = true;
        //print("CountEnded , is05Sec=" + is05Sec);
    }

    //글자 출력 한방에 뙇 
    void PrintAtOnce(Dialogue.DialogueSet dialogueSet)
    {
        StopAllCoroutines();
        isSpaceKeyDowned = false;

        curDialogBox.text = printingSentence;

        isDuringTyping = false;

        //선택상자 실행
        if (activeChoiceBox)
        {
            choiceBox.isChoiceBox = true;
            choiceBox.InitChioceBox(dialogueSet.detail.selectionPopupSettings.selectionPopupData.question, dialogueSet.detail.selectionPopupSettings.selectionPopupData.choices);
        }
        else
        {
            //Continue버튼 활성화
            continueBtn.SetActive(true);
            dialogueFlipSFXBlocker = false;
        }
    }

    //다이얼로그 문자열 길이가 너무 길지는 않은지 확인하고 처리 << 보수 필요. 일단 보류
    /*
    Dialogue.DialogueSet[] SentenceLengthChecker(Dialogue.DialogueSet[] dialogueSets)
    {
        //문장 최대 길이 설정
        int maxLength = 50;

        //배열을 List로.
        List<Dialogue.DialogueSet> dialogueSetsList = new List<Dialogue.DialogueSet>();
        dialogueSetsList.AddRange(dialogueSets);

        for(int currindex = 0; currindex<dialogueSetsList.Count; currindex++)
        //foreach (Dialogue.DialogueSet dialogueSet in dialogueSetsList)  >>  forEach사용하니.  중간에 길이 바뀌는경우 .  처리 불가.  for문으로 바꿔서 처리했음. 
        {
            //List에서 몇 번째 요소인지 확인해서.  나는 몫 값을 더해준다.  그렇게 해서 추가!
            //int currindex = dialogueSetsList.IndexOf(dialogueSet);
            Dialogue.DialogueSet dialogueSet = dialogueSetsList[currindex];
            //만약 해당 다이얼로그셋의 sentence의 길이가 30보다 길면! 
            if (dialogueSet.sentence.Length > maxLength)
            {
                Debug.Log("길이가 "+ maxLength + " 이상이다. 몇인가? : " + dialogueSet.sentence.Length);
                //전체 문장 임시 저장
                string tempStr = dialogueSet.sentence;
                Debug.Log("tempStr: " + tempStr);

                //현재 다이얼로그셋 정보 임시 저장(복사를 위한)
                //Dialogue.DialogueSet tmpDialogueSet = dialogueSet;
                Dialogue.DialogueSet tmpDialogueSet = new Dialogue.DialogueSet();
                tmpDialogueSet.detail = dialogueSet.detail;
                tmpDialogueSet.soundType = dialogueSet.soundType;
                tmpDialogueSet.name = dialogueSet.name;


                int splitcount = tempStr.Length / maxLength;
                int splitcountRemainder = tempStr.Length % maxLength;
                Debug.Log("splitcount:" + splitcount);


                for (int n=0; n<=splitcount; n++)

                ///////////////////////////////// 여기부터...!    왜 0,1 두 개가?  dialogueSetsList.Insert(currindex + n, tmpDialogueSet); 요 과정에서 문제가!!!! <<<< 역시... 
                /// //Dialogue.DialogueSet tmpDialogueSet = dialogueSet; 요 과정에서 참조가 되어서...그랬다. 
                /// 그래서 일단 Dialogue.DialogueSet tmpDialogueSet = new Dialogue.DialogueSet(); 요롷게 해결
                {
                    //dialogueSet.sentence = "";
                    //Debug.Log("For문 시작!");
                    //리스트 insert를 해줘야하는데... 지금 currindex의 다이얼로그셋 정보를 그대로 갖고있는 애들을 복사해서 insert해줘야한다. <<<<< 여기부터 ! 
                    //n이 0인 경우. 는 현재 다이얼로그셋.  이니까. 이미 있으니까. 따로 insert 해줄 필요 없고, 나머지 경우만!해주자
                    if (n != 0)
                    {
                        //Debug.Log("dialogueSetsList.Count: " + dialogueSetsList.Count);
                        dialogueSetsList.Insert(currindex + n, tmpDialogueSet);
                        //Debug.Log("복사 완료 n: "+n);
                        //Debug.Log("dialogueSetsList.Count: " + dialogueSetsList.Count);
                    }
                    else Debug.Log("n이 0인 경우는 그냥 패스!");

                    //복사 다 해줬으니까 얘네의 sentence값만 각각에 알맞게 바꿔주면 된다!

                    //n이 splitcount(몫)과 같으면!  마지막이니까.  sn의 최댓값 + 를 29 대신 나머지값으로 하면 되고, 아닌경우는 30*n*29 ! 
                    int num = currindex + n;
                    //Debug.Log("n == splitcount인 상황. n?" + n);
                    //Debug.Log("dialogueSetsList["+0+"].sentence : " + dialogueSetsList[0].sentence);
                    //Debug.Log("dialogueSetsList[" + 1 + "].sentence : " + dialogueSetsList[1].sentence);
                    dialogueSetsList[num].sentence = "";
                    //Debug.Log("dialogueSetsList[" + 0 + "].sentence : " + dialogueSetsList[0].sentence);
                    //Debug.Log("dialogueSetsList[" + 1 + "].sentence : " + dialogueSetsList[1].sentence);
                    if (n == splitcount)
                    {
                        
                        for (int sn = maxLength * n; sn < maxLength * n + splitcountRemainder; sn++)
                        { 
                            //Debug.Log("currindex + n:" + num);
                            dialogueSetsList[num].sentence += tempStr[sn];
                            //Debug.Log("n:" + n + ", sn:" + sn + ", tempStr[sn]:" + tempStr[sn] + "\n" + num + " sentence:" + dialogueSetsList[num].sentence);
                            //Debug.Log("n:" + n + ", sn:" + sn + ","+ " sentence:" + dialogueSetsList[0].sentence);
                            //Debug.Log("n:" + n + ", sn:" + sn + "," + " sentence:" + dialogueSetsList[1].sentence);
                        }
                    }
                    else
                    {
                        for (int sn = maxLength * n; sn <= maxLength * n + (maxLength-1); sn++)
                        {
                            dialogueSetsList[num].sentence += tempStr[sn];
                            //Debug.Log("n:"+n + ", sn:"+sn + ", tempStr[sn]:"+ tempStr[sn] + "\n" + num + " sentence:" + dialogueSetsList[num].sentence);
                            //Debug.Log("n:" + n + ", sn:" + sn + "," + " sentence:" + dialogueSetsList[num].sentence);
                        }
                    }
                }
            }
        }

        return dialogueSetsList.ToArray();
    }
    */

    //다이얼로그 종료 
    void EndDialogue()
    {
        cutSceneImage.color = new Color(1, 1, 1, 0);
        isDialogueActive = false;
        dialog_animator.SetBool("isOpen", false);

        //다이얼로그 끝나면 플레이어 컨트롤 권한 돌려줌.       
        foreach (KeyInput_Controller item in GameObject.FindObjectsOfType<KeyInput_Controller>())
        {
            item.isControllable = true;
            print("컨트롤 권한 돌려줌!");
        }

        //특정 이벤트 연계 위한 부분. 선택상자 정답시 처리도 여기서. ^-^
        switch (curStoryId)
        {
            case 1:
                //씬1 다이얼로그 모두 끝난 시점 
                //씬2_취조실로 씬 전환 작업 시작.
                GameManager.Instance.SetStoryNumber(2);
                GameManager.Instance.StartStoryEvent();
                break;
            case 3:
                //씬2_취조실 다이얼로그 끝난 시점.
                //씬2_검은화면 발동!
                GameManager.Instance.SetStoryNumber(4);
                GameManager.Instance.StartStoryEvent();
                break;
            case 5:
                //씬2_검은화면 다이얼로그 끝난 시점.
                //씬2_다시 취조실로 이동작업 시작.
                GameManager.Instance.SetStoryNumber(6);
                GameManager.Instance.StartStoryEvent();
                break;
            case 7:
                //씬2_다시 취조실 다이얼로그 끝난 시점.
                //씬3_지하창고로 씬 전환 직업 시작
                GameManager.Instance.SetStoryNumber(8);
                GameManager.Instance.StartStoryEvent();
                break;
            case 9:
                //씬3_지하창고 다이얼로그 끝난 시점.
                //씬4_베릴성으로 씬 전환 작업 시작
                GameManager.Instance.SetStoryNumber(10);
                GameManager.Instance.StartStoryEvent();
                break;

            case 31:
                //멧돼지게임
                GameManager.Instance.SetStoryNumber(31); // 31?? 맞나 확인 필요
                GameManager.Instance.StartStoryEvent();
                break;
            case 10003:
                //Scene1. 첫 등장 - 경찰 제지 선택상자 정답 결과.
                //경찰1 오브젝트의 ID값을 10000에서 10005로 변경시켜준다. (정답을 맞춘 후 경찰1에게 다시 말을 걸었을 때를 위한 처리.)
                경찰1.GetComponent<Object>().id = 10005;
                //브금도 실행해준다
                LevelChanger_ForScene1.instance.ActivateBGM_ForScene1();
                break;
            case 15500:
                //Scene1. 책상 정보설명 끝나고 말풍선 시작해주는부분.
                StartDialogue(800000);
                break;
            default:
                break;
        }
    }


}
