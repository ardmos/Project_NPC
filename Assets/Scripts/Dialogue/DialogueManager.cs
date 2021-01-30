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



    [Header("- 스토리 문장 저장소"), Space(20)]
    public Dialogue[] dialogueData;
    Dictionary<int, Dialogue> dialogueData_Dic;
    Dialogue dialogue;
    Queue<string> sentences;

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
        sentences = new Queue<string>();
        //딕셔너리에 넣는 과정 
        foreach (Dialogue item in dialogueData)
        {
            dialogueData_Dic.Add(item.storyId, item);
        }
    }

    public void StartDialogue(int objid)    //다이얼로그의 다양한 부분을 초기화. 
    {
        //해당 아이디값 개체 검색
        print(dialogueData_Dic[objid]);

        if (dialogueData_Dic.ContainsKey(objid))    //해당 id값을 가진 개체가 존자한다면
        {
            //그 개체의 Dialogue 클래스를 꺼내와서
            dialogue = dialogueData_Dic[objid];
            //그 중 sentences 스트링 배열에 접근한다.
            string[] sentences_ = dialogue.sentences;
            //얻어낸 string[]를 Queue에 순차적으로 Enqueue 한다.
            sentences.Clear();
            foreach (string item in sentences_)
            {
                sentences.Enqueue(item);
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
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        SplitStringServiceSir(sentences.Dequeue());
    }

    //스플릿!
    void SplitStringServiceSir(string str)
    {
        //스플릿!
        ///
        //split 해서 길이 0 나오면 바로 패스.
        //바로 넥스트 다이얼로그 
        //split 해서 길이 2 나오면 기본 stable스타일
        //0 이름, 1문장
        //split 해서 길이 3 나오면 스타일 선택 가능
        //0 이름, 1 문장, 2 스타일
        //split 해서 길이 4 나오면 초상화 선택 가능
        //0 이름, 1 문장, 2 스타일, 3 초상화 번호
        //split 해서 길이 5 나오면 선택지 팝업 가능
        //0 이름, 1 문장, 2 스타일, 3 초상화 번호, 4 선택지발동팝업 (다음 페이지 시작 직전에 스톱해두고 발동. )
        //split 해서 길이 6 나오면 선택지 팝업 가능
        //0 이름, 1 문장, 2 스타일, 3 초상화 좌 번호, 4 선택지발동팝업 (다음 페이지 시작 직전에 스톱해두고 발동. ), 5 초상화 우 번호
        ///
        string[] strRules = str.Split(':');
        string sentence = null;
        switch (strRules.Length)
        {
            case 1:                    
                if (strRules[0] == "")
                {
                    print("It's empty! Pass!");
                    //비어있으면 그냥 패스.
                    DisplayNextSentence();
                    return;
                }
                else
                {
                    //비어있는게 아닐 경우. 환경음 등의 표현일 때 ex (뚜벅뚜벅뚜벅)
                    dialogObjName.text = "";   //이름  
                    sentence = strRules[0];
                    StartAnimByStyle(0);    //기본 스타일                                  
                    dialogPortrait_Right.color = new Color(1, 1, 1, 0); //초상화(우) 없음
                    dialogPortrait_Left.color = new Color(1, 1, 1, 0); //초상화(좌) 없음
                    break;
                }
            case 2:
                dialogObjName.text = strRules[0];   //이름                
                sentence = strRules[1]; //문장
                StartAnimByStyle(0);    //기본 스타일                                  
                dialogPortrait_Right.color = new Color(1, 1, 1, 0); //초상화(우) 없음
                dialogPortrait_Left.color = new Color(1, 1, 1, 0); //초상화(좌) 없음
                break;
            case 3:
                dialogObjName.text = strRules[0];   //이름                
                sentence = strRules[1]; //문장
                StartAnimByStyle(int.Parse(strRules[2]));   //스타일
                dialogPortrait_Right.color = new Color(1, 1, 1, 0); //초상화(우) 없음
                dialogPortrait_Left.color = new Color(1, 1, 1, 0); //초상화(좌) 없음
                break;
            case 4:
                dialogObjName.text = strRules[0];   //이름               
                sentence = strRules[1]; //문장
                StartAnimByStyle(int.Parse(strRules[2]));   //스타일
                if (int.Parse(strRules[3]) == -1)
                    dialogPortrait_Right.color = new Color(1, 1, 1, 0);
                else
                {
                    dialogPortrait_Right.color = new Color(1, 1, 1, 1);
                    dialogPortrait_Right.sprite = dialogue.portraits[int.Parse(strRules[3])]; //초상화(우) 표정
                }
                dialogPortrait_Left.color = new Color(1, 1, 1, 0);//초상화(좌) 없음
                break;
            case 5:
                dialogObjName.text = strRules[0];   //이름               
                sentence = strRules[1]; //문장
                StartAnimByStyle(int.Parse(strRules[2]));   //스타일
                if (int.Parse(strRules[3]) == -1)
                    dialogPortrait_Right.color = new Color(1, 1, 1, 0);
                else
                {
                    dialogPortrait_Right.color = new Color(1, 1, 1, 1);
                    dialogPortrait_Right.sprite = dialogue.portraits[int.Parse(strRules[3])]; //초상화(우) 표정
                }
                dialogPortrait_Left.color = new Color(1, 1, 1, 0);//초상화(좌) 없음
                if (int.Parse(strRules[4]) == -1)
                    print("noting");
                else if(int.Parse(strRules[4])<dialogue.choices.Length)
                {
                    //선택지 발동 
                    activeChoiceBox = true;
                    choiceBox.isChoiceBox = true;
                }
                else
                    Debug.Log("선택지 발동 부분에 입력된 숫자를 확인해주세요 : " + int.Parse(strRules[4]));
                break;
            case 6:
                dialogObjName.text = strRules[0];   //이름               
                sentence = strRules[1]; //문장
                StartAnimByStyle(int.Parse(strRules[2]));   //스타일
                if (int.Parse(strRules[3]) == -1)
                    dialogPortrait_Right.color = new Color(1, 1, 1, 0);
                else
                {
                    dialogPortrait_Right.color = new Color(1, 1, 1, 1);
                    dialogPortrait_Right.sprite = dialogue.portraits[int.Parse(strRules[3])]; //초상화(우) 표정
                }
                if (int.Parse(strRules[4]) == -1)
                    print("noting");
                else if(int.Parse(strRules[4]) < dialogue.choices.Length)
                {
                    //선택지 발동 
                    activeChoiceBox = true;
                    choiceBox.isChoiceBox = true;
                }
                else
                    Debug.Log("선택지 발동 부분에 입력된 숫자를 확인해주세요 : " + int.Parse(strRules[4]));
                //초상화(좌)
                if (int.Parse(strRules[5]) == -1)
                    dialogPortrait_Left.color = new Color(1, 1, 1, 0);
                else
                    dialogPortrait_Left.color = new Color(1, 1, 1, 1);
                dialogPortrait_Left.sprite = dialogue.portraits[int.Parse(strRules[5])]; //초상화(좌) 표정
                break;
            case 7:
                dialogObjName.text = strRules[0];   //이름               
                sentence = strRules[1]; //문장
                StartAnimByStyle(int.Parse(strRules[2]));   //스타일
                if (int.Parse(strRules[3]) == -1)
                    dialogPortrait_Right.color = new Color(1, 1, 1, 0);
                else
                {
                    dialogPortrait_Right.color = new Color(1, 1, 1, 1);
                    dialogPortrait_Right.sprite = dialogue.portraits[int.Parse(strRules[3])]; //초상화(우) 표정
                }
                if (int.Parse(strRules[4]) == -1)
                    print("noting");
                else if (int.Parse(strRules[4]) < dialogue.choices.Length)
                {
                    //선택지 발동 
                    activeChoiceBox = true;
                    choiceBox.isChoiceBox = true;
                }
                else
                    Debug.Log("선택지 발동 부분에 입력된 숫자를 확인해주세요 : " + int.Parse(strRules[4]));
                //초상화(좌)
                if (int.Parse(strRules[5]) == -1)
                    dialogPortrait_Left.color = new Color(1, 1, 1, 0);
                else
                {
                    dialogPortrait_Left.color = new Color(1, 1, 1, 1);
                    dialogPortrait_Left.sprite = dialogue.portraits[int.Parse(strRules[5])]; //초상화(좌) 표정
                }
                    
                

                //애니메이션 발동.
                int npcNum = int.Parse(strRules[6].Split('=')[0]);
                string animationclip = strRules[6].Split('=')[1];

                dialogue.npc[npcNum].Play(animationclip);//해당 애니메이션 실행.        

                break;
            default:
                sentence = "error at DialogueManager.cs // split : here. please make sure the split rules.  case " + strRules.Length + ":";
                break;
        }

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }


    //3.결과문 출력
    public void PrintTheChoiceResult(int valueToReturn)
    {
        activeChoiceBox = false;
        npcResponseNum = valueToReturn;//npc 응답연결을 위한 저장.  
        isNPCresponding = true;
        SplitStringServiceSir(dialogue.choice_results[valueToReturn]);
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
        SplitStringServiceSir(dialogue.responses[valueToResponse]);
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
    IEnumerator TypeSentence(string sentence)
    {
        dialogSentence.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogSentence.text += letter;
            yield return new WaitForSeconds(1f - dialogue.letterSpeed);
        }

        if (activeChoiceBox)
            choiceBox.InitChioceBox(dialogue.ask, dialogue.choices);
    }

    void EndDialogue()
    {
        dialog_animator.SetBool("isOpen", false);
    }


}
