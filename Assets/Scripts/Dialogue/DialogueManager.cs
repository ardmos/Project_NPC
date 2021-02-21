using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : DontDestroy<DialogueManager>
{
    public Animator dialog_animator;
    public RuntimeAnimatorController stable, slide;
    public Text dialogObjName, dialogSentence;
    public Image dialogPortrait_Left, dialogPortrait_Right, cutSceneImage;
    public ChoiceBox choiceBox;
    public AudioSource audioSource;
    public AudioSystem audioSystem;
    public GameObject 스토리정리;

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
    public Dialogue.DialogueSet curDialogSet;
    public int curDialogSetCountNumber;
    public int curStoryId;
    public string curStorySmallTitle;

    bool activeChoiceBox, isNPCresponding;
    int npcResponseNum;
    //선택상자에대한 응답에서 문장 빨리넘기기 했을 시 사용할 문장.  이미 도도도 찍고있던 문장을 담고있다.
    string printingSentence;
    //다이얼로그 문장 없이 애니메이션만 실행시키기 위한 부분.    
    public bool duringAnimation_AnimateAlone;
    public bool endedAnimation_AnimateAlone;
    public List<NPC> nPCs = new List<NPC>();
    public List<KeyInput_Controller> keyInput_Controllers = new List<KeyInput_Controller>();
    //다이얼로그 딜레이 타임 
    bool isdelayTimeCompleted = false;
    bool isStartedWatingDelayTime = false;
    //다이얼로그 사운드이펙트만!! LifeTime 으로 재생중인지!
    bool isSFXDialogLifeTime = false;


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

        foreach (AttachThis attachThis in 스토리정리.GetComponentsInChildren<AttachThis>())
        {
            foreach (Dialogue item in attachThis.dialogues)
            {
                //딕셔너리에 넣는 과정 
                dialogueData_Dic.Add(item.storyId, item);
            }
        }       
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) isSpaceKeyDowned = true;

        if (isSpaceKeyDowned && isDialogueActive)
        {
            if (isDuringTyping) PrintAtOnce(curDialogSet);
            else DisplayNextSentence();
        }

        #region 문장 없을 때 애니메이션만 실행
        //애니메이션 중이면 대기. 
        //애니메이션이 끝났으면 DisplayNextSentence(); 진행.
        if (duringAnimation_AnimateAlone)
        {
            string objname = null;
            
            foreach (NPC item in nPCs)
            {
                objname = item.gameObject.name;
                if (item.isArrived)
                {
                    print(objname + "Arrived");
                    endedAnimation_AnimateAlone = true;
                }
                else
                {
                    print(objname + "Not Arrived");
                    endedAnimation_AnimateAlone = false;
                    return;
                }
            }
            foreach (KeyInput_Controller item in keyInput_Controllers)
            {
                objname = item.gameObject.name;
                if (item.isArrived)
                {
                    print(objname + "플레이어 Arrived");
                    endedAnimation_AnimateAlone = true;
                }
                else
                {
                    print(objname + "플레이어 Not Arrived");
                    endedAnimation_AnimateAlone = false;
                    return;
                }
            }
            print(objname + " 이동애니메이션 진행중.");

            if (endedAnimation_AnimateAlone)
            {
                //print("끝! end!");
                //혹~시! 사운드LifeTime중인지?
                if (!isSFXDialogLifeTime)
                {
                    print(objname + " 이동애니메이션 도착.끝");
                    duringAnimation_AnimateAlone = false;
                    DisplayNextSentence();
                }
            }
        }
        #endregion         
    }

    public void OnBtnClickedByMouse()
    {
        isSpaceKeyDowned = true;
    }

    //다이얼로그 시작
    public void StartDialogue(int objid)    //다이얼로그의 다양한 부분을 초기화. 
    {
        isDialogueActive = true;

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


        //해당 아이디값 개체 검색       
        if (dialogueData_Dic.ContainsKey(objid))    //해당 id값을 가진 개체가 존자한다면
        {
            //그 개체의 Dialogue 클래스를 꺼내와서
            dialogue = dialogueData_Dic[objid];
            curStorySmallTitle = dialogue.smallTitle_;
            curStoryId = dialogue.storyId;
            curDialogSetCountNumber = 0;    //넘버 초기화. 

            //그 중 dialogueSet 배열에 접근한다.  배열을 큐로 변환시키기 위함. 편의를 위해서. 
            Dialogue.DialogueSet[] dialogueSet = dialogue.dialogue;
            //얻어낸 dialogueSet을 Queue에 순차적으로 Enqueue 한다.
            dialogueSetsQue.Clear();
            foreach (Dialogue.DialogueSet item in dialogueSet) dialogueSetsQue.Enqueue(item);
        }
        else   Debug.Log("정보가 없는 녀석입니다.");
        DisplayNextSentence();
    }

    //문자 출력 들어감.
    public void DisplayNextSentence()
    {
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
        ///
        //혹시 NPC가 선택지에 대답중이라면? 
        //바로 통화 연결 됩니다!  바로 여기서요!~! 
        ///
        if (isNPCresponding)
        {
            NPCResponseToTheChoiceResult(npcResponseNum);
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

        BatchService(curDialogSet);
    }

    //배치서비스 시작합니다.   스플릿서비스는 안녕!  일괄 처리.
    void BatchService(Dialogue.DialogueSet dialogueSet) {
        //여기서 dialogueSet에 입력된 정보에 따라 새 다이얼로그창의 정보를 설정.

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
            //얘네들...있다 true 변수 
            bool goreturn = false;

            //발동할 애니메이션이 있는지? 
            if (dialogueSet.detail.animationSettings.activateObjAnimate)
            {
                //발동할 애니메이션이 존재하면, 해당 애니메이션이 끝나길 기다렸다가  return.                
                foreach (Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData objAnimData in dialogueSet.detail.animationSettings.objectAnimationData)
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
                duringAnimation_AnimateAlone = true;
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

            //발동할 사운드 이펙트가 있는지?
            if (dialogueSet.detail.sFXSettings.enableSFX && dialogueSet.detail.sFXSettings.dialogueLifeTime != 0)
            {
                //사운드 재생 후 
                audioSystem.DialogSFXHelper(dialogueSet);
                //설정한 LifeTime만큼 기다렸다가 다음 다이얼로그 호출. 
                StartCoroutine(WaitLifeTime(dialogueSet.detail.sFXSettings.dialogueLifeTime));
                goreturn = true;
            }


            if(goreturn)
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
            case Dialogue.DialogueSet.Names.음식:
                dialogObjName.text = "음식";
                break;
            case Dialogue.DialogueSet.Names.침대:
                dialogObjName.text = "침대";
                break;
            case Dialogue.DialogueSet.Names.창문:
                dialogObjName.text = "창문";
                break;
            case Dialogue.DialogueSet.Names.캡슐:
                dialogObjName.text = "캡슐";
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


        //선택팝업
        if (dialogueSet.detail.selectionPopupSettings.activateSelectionPopup)
        {
            //선택지 발동 조건 미리 만족시켜둠.  실행은 타이핑이 끝난 시점에 됨. 
            activeChoiceBox = true;
        }

        //Object 이동 애니메이션
        if (dialogueSet.detail.animationSettings.activateObjAnimate)
        {
            foreach (Dialogue.DialogueSet.Details.AnimationSettings.ObjectAnimData objAnimData in dialogueSet.detail.animationSettings.objectAnimationData)
            {
                //objAnimData.objToMakeMove.MoveAnimStart(objAnimData);

                //NPC.cs가 있는 경우.(NPC인 경우) or KeyInput_Controller가 있는 경우.(Player인 경우) 알아서 처리. 

                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 2) return; //지금 미로(씬빌드인덱스2)씬 테스트 목적으로, 잠시 막아두기 

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
        }

        //효과음 실행
        audioSystem.DialogSFXHelper(dialogueSet);

        //컷씬 출력 
        if (dialogueSet.detail.cutSceneSettings.activateCutScene)
        {
            cutSceneImage.color = new Color(1, 1, 1, 1);
            cutSceneImage.sprite = dialogueSet.detail.cutSceneSettings.image;
            cutSceneImage.SetNativeSize();
        }
        else cutSceneImage.color = new Color(1, 1, 1, 0);

        #endregion


        //문장 도도도 출력하는 부분
        StopAllCoroutines();
        StartCoroutine(TypeSentence(dialogueSet));
    }

    //3.결과문 출력
    public void PrintTheChoiceResult(int valueToReturn)
    {
        //선택된 답 저장. 5. 정해진 루트 진행을 위한. 
        //선택 결과들은 딕셔너리 형태로 저장될것임. 
        //Key값은 string으로 dialogueSetName을 넣어주고(스토리아이디값_몇번째다이얼로그인지카운트) , Value값은 int로 어떤 선택을 했는지 번호를 저장(0번부터 시작).
                                                //잡고나서 선택 31_물어보기_행동결정
        GameManager.Instance.AddChoiceResults(curDialogSet.dialogueSetName, valueToReturn);

        activeChoiceBox = false;
        npcResponseNum = valueToReturn;//npc 응답연결을 위한 저장.  
        isNPCresponding = true;
        //SplitStringServiceSir(dialogue.choice_results[valueToReturn]);
        BatchService(curDialogSet.detail.selectionPopupSettings.selectionPopupData.choice_results[valueToReturn]);
    }

    //4.결과문에 엔피씨가 응답한다.(분기점 스탯도 적용도 여기서 구현하면 될듯)
    public void NPCResponseToTheChoiceResult(int valueToResponse)
    {
        ///
        //분기점 관련 기록 부분 추가 필요
        //
        //그리고 분기가 나뉘면,  id값을 다르게 해서 다른 이야기팩으로 새로 시작하게끔 하자. 
        ///
        isNPCresponding = false;

        if (curDialogSet.sentence == "")
        {
            //여기서도 sentece 공백이면 그냥 패스하는 부분 추가.
            DisplayNextSentence();
            return;
        }

        BatchService(curDialogSet.detail.selectionPopupSettings.selectionPopupData.responses[valueToResponse]);
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

    //한글자씩 도도도 찍기
    IEnumerator TypeSentence(Dialogue.DialogueSet dialogueSet)
    {
        ///출력중       
        isDuringTyping = true;

        //선택상자 팝업에서 빨리넘기기 할 때 쓰이는 변수. 
        printingSentence = dialogueSet.sentence;

        dialogSentence.text = "";
        foreach (char letter in dialogueSet.sentence.ToCharArray())
        {
            dialogSentence.text += letter;
 
            //타이핑 효과음 출력 부분.  스페이스는 거른다.  turnOffTypingSound 체크해제 되어있는지도 확인
            if (letter != System.Convert.ToChar(32) && dialogueSet.detail.sFXSettings.turnOffTypingSound == false)
            {
                audioSource.PlayOneShot(typingSounds[dialogueSet.soundNumber]);
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

    //글자 출력 한방에 뙇 
    void PrintAtOnce(Dialogue.DialogueSet dialogueSet)
    {
        StopAllCoroutines();
        isSpaceKeyDowned = false;

        dialogSentence.text = printingSentence;

        isDuringTyping = false;
        //선택상자 실행
        if (activeChoiceBox)
        {
            choiceBox.isChoiceBox = true;
            choiceBox.InitChioceBox(dialogueSet.detail.selectionPopupSettings.selectionPopupData.question, dialogueSet.detail.selectionPopupSettings.selectionPopupData.choices);
        }
            
    }

    //다이얼로그 종료 
    void EndDialogue()
    {
        isDialogueActive = false;
        dialog_animator.SetBool("isOpen", false);

        //다이얼로그 끝나면 플레이어 컨트롤 권한 돌려줌.       
        foreach (KeyInput_Controller item in GameObject.FindObjectsOfType<KeyInput_Controller>())
        {
            item.isControllable = true;
        }

        //특정 이벤트 연계 위한 부분.
        switch (curStoryId)
        {
            case 1:
                //씬1 다이얼로그 모두 끝난 시점 
                //씬2로 이동.
                GameManager.Instance.storyNumber = 2;
                GameManager.Instance.StartStoryEvent();
                break;
            case 31:
                //멧돼지게임
                GameManager.Instance.storyNumber = 31;
                GameManager.Instance.StartStoryEvent();                
                break;
            default:
                break;
        }
    }


}
