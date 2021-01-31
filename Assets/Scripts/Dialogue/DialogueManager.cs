using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Animator dialog_animator;
    public RuntimeAnimatorController stable, slide;
    public Text dialogObjName, dialogSentence;
    public Image dialogPortrait_Left, dialogPortrait_Right;
    public ChoiceBox choiceBox;

    //동작 체크 
    public bool isDialogueActive;

    [Header("- 대화 보따리 저장소. 만들고자 하는 Dialog 보따리의 갯수를 입력해주세요 ^^"), Space(20)]
    public Dialogue[] dialogues;
    [Header("- 초상화 저장소. 사용될 초상화들을 모두 이곳에 저장해주세요.~")]
    public Sprite[] portraits;

    Dictionary<int, Dialogue> dialogueData_Dic;
    Dialogue dialogue;
    Queue<Dialogue.DialogueSet> dialogueSetsQue;
    public Dialogue.DialogueSet curDialogSet;

    bool activeChoiceBox, isNPCresponding;
    int npcResponseNum;

    #region For Signleton
    //싱글턴
    public static DialogueManager instance;

    private void Awake()
    {
        //싱글턴
        instance = this;
        dialogueData_Dic = new Dictionary<int, Dialogue>();
    }
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        dialogueSetsQue = new Queue<Dialogue.DialogueSet>();
        //딕셔너리에 넣는 과정 
        if (dialogues.Length != 0)
        {
            foreach (Dialogue item in dialogues)
            {
                dialogueData_Dic.Add(item.storyId, item);
            }
        }

    }

    public void StartDialogue(int objid)    //다이얼로그의 다양한 부분을 초기화. 
    {
        isDialogueActive = true;

        //해당 아이디값 개체 검색       

        if (dialogueData_Dic.ContainsKey(objid))    //해당 id값을 가진 개체가 존자한다면
        {
            //그 개체의 Dialogue 클래스를 꺼내와서
            dialogue = dialogueData_Dic[objid];
            //그 중 sentences 스트링 배열에 접근한다.
            Dialogue.DialogueSet[] sentences_ = dialogue.dialogueSet;
            //얻어낸 string[]를 Queue에 순차적으로 Enqueue 한다.
            dialogueSetsQue.Clear();
            foreach (Dialogue.DialogueSet item in sentences_)
            {
                dialogueSetsQue.Enqueue(item);
            }
        }
        else
        {
            Debug.Log("정보가 없는 녀석입니다.");
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
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

        curDialogSet = dialogueSetsQue.Dequeue();       
        BatchService(curDialogSet);
    }



    //배치서비스 시작합니다.   스플릿서비스는 안녕!  일괄 처리.
    void BatchService(Dialogue.DialogueSet dialogueSet) {

        //기본 글자 속도.
        if (dialogueSet.details.letterSpeed == 0f)
            dialogueSet.details.letterSpeed = 0.9f;

        //Sentence 비어있을경우 그냥 패스! 
        if (dialogueSet.sentence == "")
        {
            DisplayNextSentence();
            return;
        }


        //여기서 dialogueSet에 입력된 정보에 따라 처리.

        //이름
        dialogObjName.text = dialogueSet.name;

        ///Details
        ///
        //스타일
        StartAnimByStyle((int)dialogueSet.details.styles);
        //초상화 세팅
        //초상화(좌)
        if (dialogueSet.details.portraitSettings.showLeftPortrait)
        {
            dialogPortrait_Left.color = new Color(1, 1, 1, 1);
            dialogPortrait_Left.sprite = portraits[dialogueSet.details.portraitSettings.leftPortraitNumber]; //초상화(좌) 표정
        }
        else
            dialogPortrait_Left.color = new Color(1, 1, 1, 0);

        //초상화(우)
        if (dialogueSet.details.portraitSettings.showRightPortrait)
        {
            dialogPortrait_Right.color = new Color(1, 1, 1, 1);
            dialogPortrait_Right.sprite = portraits[dialogueSet.details.portraitSettings.rightPortraitNumber]; //초상화(우) 표정
        }
        else
            dialogPortrait_Right.color = new Color(1, 1, 1, 0);


        //선택팝업
        if (dialogueSet.details.activateSelectionPopup)
        {
            //선택지 발동 
            activeChoiceBox = true;
            choiceBox.isChoiceBox = true;
        }

        //npc 애니메이션
        if (dialogueSet.details.activateNpcAnimate)
        {
            foreach (Dialogue.DialogueSet.Details.NpcAnimData npcAnimData in dialogueSet.details.npcAnimationData)
            {
                npcAnimData.npc.Play(npcAnimData.animationName);                
            }
        }
        ///
        ///Details
        ///


        //문장
        StopAllCoroutines();
        StartCoroutine(TypeSentence(dialogueSet));
    }

    //3.결과문 출력
    public void PrintTheChoiceResult(int valueToReturn)
    {
        activeChoiceBox = false;
        npcResponseNum = valueToReturn;//npc 응답연결을 위한 저장.  
        isNPCresponding = true;
        //SplitStringServiceSir(dialogue.choice_results[valueToReturn]);
        BatchService(curDialogSet.details.selectionPopupData.choice_results[valueToReturn]);
    }
    //4.결과문에 엔피씨가 응답한다.(분기점 스탯도 적용되는 부분)
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

        BatchService(curDialogSet.details.selectionPopupData.responses[valueToResponse]);
    }


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
        dialogSentence.text = "";
        foreach (char letter in dialogueSet.sentence.ToCharArray())
        {
            dialogSentence.text += letter;
            yield return new WaitForSeconds(1f - dialogueSet.details.letterSpeed); //letterSpeed 받아서 처리하게끔 하자
        }

        //선택상자 실행
        if (activeChoiceBox)
            choiceBox.InitChioceBox(dialogueSet.details.selectionPopupData.question, dialogueSet.details.selectionPopupData.choices);
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        dialog_animator.SetBool("isOpen", false);
    }


}
