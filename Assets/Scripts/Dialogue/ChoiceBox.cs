using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChoiceBox : MonoBehaviour
{
    ///여기서 하는 일
    //1.맞는 질문텍스트와, 받은 선택지 갯수만큼의 버튼(최대 4글자, 8개) 생성. 
    //2.유저 키 입력 (1,2,3), 마우스 입력 받아서 처리.   
    //  입력받을 시 정답범위인지 체크하는 과정 필요
    //3.결과 return
    ///

    public bool isChoiceBox;
    public string question;
    public Dialogue.DialogueSet.Details.Choices[] choices;

    public GridLayoutGroup gridLeft, gridRight;
    public Animator animator;
    public Text questionText;
    public GameObject[] buttons;

    int curNum, tmp, choicesLength;


    public void InitChioceBox(string mAsk, Dialogue.DialogueSet.Details.Choices[] mChoices) 
    {        
        isChoiceBox = true;
        question = mAsk;
        choices = mChoices;
        curNum = 0;
        tmp = 0;
        choicesLength = DialogueManager.Instance.curDialogSet.detail.selectionPopupSettings.selectionPopupData.choices.Length;

        //각 텍스트를 오브젝트텍스트에 넣어주는 과정.
        questionText.text = question;

        int length = choices.Length;

        //기본 너비 세팅 
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(300f, 234f);
        if (length<=4)        
            gridLeft.cellSize = new Vector2(280f, 45f);    
        else
            gridLeft.cellSize = new Vector2(140f, 45f);

        for (int i = 0; i < length; i++)
        {
            buttons[i].GetComponentInChildren<Text>().text = choices[i].sentence;
            buttons[i].SetActive(true);
        }

        for (int i = length; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }

        #region 보기 문장의 길이에 맞춰서 전체 width 조정
        bool needBiggerCase = false;
        float sizeToNeed = 0f;
        //보기 문장의 길이에 맞춰서 전체 width 조정.
        ///// 1~4번까지 보기만 존재할 경우.
        ///1. 만약, Text.preferredWidth가 280보다 크다면, Text.preferredWidth-280 한 만큼 바탕의 width를 늘린다. 
        ///2. Text.preferredWidth-280한 만큼 그리드1, 버튼의 너비도 각 각 늘린다.
        if (length <= 4)
        {
            foreach (GameObject buttonObj in buttons)
            {
                if (buttonObj.GetComponentInChildren<Text>().preferredWidth > 280f)
                {
                    needBiggerCase = true;
                    sizeToNeed = buttonObj.GetComponentInChildren<Text>().preferredWidth - 280f;
                }
            }

            if (needBiggerCase)
            {
                //바탕 크기
                RectTransform caseRT = gameObject.GetComponent<RectTransform>();
                caseRT.sizeDelta = new Vector2(caseRT.sizeDelta.x + sizeToNeed, caseRT.sizeDelta.y);
                //gridLeft
                RectTransform gridLeftRT = gridLeft.gameObject.GetComponent<RectTransform>();
                gridLeftRT.sizeDelta = new Vector2(gridLeftRT.sizeDelta.x + sizeToNeed, gridLeftRT.sizeDelta.y);
                //버튼들 크기
                gridLeft.cellSize = new Vector2(gridLeft.cellSize.x + sizeToNeed, gridLeft.cellSize.y);
            }
        }
        ///// 1~8번까지의 보기가 존재할 경우.
        ///1. 만약, Text.preferredWidth가 140보다 크다면, Text.preferredWidth-140 한 만큼 바탕의 width를 늘린다. 
        ///2. Text.preferredWidth-140한 만큼 그리드1, 그리드2, 버튼의 너비도 각 각 늘린다.
        else
        {
            foreach (GameObject buttonObj in buttons)
            {
                if (buttonObj.GetComponentInChildren<Text>().preferredWidth > 140f)
                {
                    needBiggerCase = true;
                    sizeToNeed = buttonObj.GetComponentInChildren<Text>().preferredWidth - 140f;
                }
            }

            if (needBiggerCase)
            {
                //바탕 크기
                RectTransform caseRT = gameObject.GetComponent<RectTransform>();
                caseRT.sizeDelta = new Vector2(caseRT.sizeDelta.x + sizeToNeed, caseRT.sizeDelta.y);
                //gridLeft
                RectTransform gridLeftRT = gridLeft.gameObject.GetComponent<RectTransform>();
                gridLeftRT.sizeDelta = new Vector2(gridLeftRT.sizeDelta.x + sizeToNeed, gridLeftRT.sizeDelta.y);
                //버튼들 크기
                gridLeft.cellSize = new Vector2(gridLeft.cellSize.x + sizeToNeed, gridLeft.cellSize.y);
                //gridRight
                RectTransform gridRightRT = gridRight.gameObject.GetComponent<RectTransform>();
                gridRightRT.sizeDelta = new Vector2(gridRightRT.sizeDelta.x + sizeToNeed, gridRightRT.sizeDelta.y);
                //버튼들 크기
                gridRight.cellSize = new Vector2(gridRight.cellSize.x + sizeToNeed, gridRight.cellSize.y);
            }
        }
        #endregion

        //ChoiceBox 열기. 애니메이션.
        animator.SetBool("isOpen", isChoiceBox);
    }


    private void Update()
    {
        if (isChoiceBox)
        {
            ///여기서 할 일!
            //박스가 열리면, 1번 버튼 배경 이미지가 보임.(버튼은 항상 존재하니 신경 안써도 됨.)  
            //방향키 이동에 따라 int값 변함. 
            //변한 인트값에 맞는 번호의 버튼 배경 이미지가 보임. 
            //스페이스키 입력시 현재 int값. 즉 활성화된 버튼의 번호가 GetChoice()함수로 넘어감. 

            foreach (GameObject item in buttons)
            {
                item.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }
            buttons[curNum].GetComponent<Image>().color = new Color(1, 1, 1, 1);


            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                //Left
                switch (curNum)
                {
                    case 0:                        
                    case 1:
                    case 2:
                    case 3:
                        tmp = 0;
                        break;
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                        tmp = -4;
                        break;
                    default:
                        break;
                }
                SetCurNum(tmp);
            }
            else if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                //RIght
                switch (curNum)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                        tmp = 4;
                        break;
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                        tmp = 0;
                        break;
                    default:
                        break;
                }
                SetCurNum(tmp);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                //Up
                switch (curNum)
                {
                    case 0:
                    case 4:
                        tmp = 0;
                        break;
                    case 1:
                    case 2:
                    case 3:                    
                    case 5:
                    case 6:
                    case 7:
                        tmp = -1;
                        break;
                    default:
                        break;
                }
                SetCurNum(tmp);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                //Down
                switch (curNum)
                {
                    case 0:
                    case 1:
                    case 2:                    
                    case 4:
                    case 5:
                    case 6:
                        tmp = 1;
                        break;
                    case 3:
                    case 7:
                        tmp = 0;
                        break;
                    default:
                        break;
                }
                SetCurNum(tmp);
            }

            if (Input.GetKeyDown(KeyCode.Return) && !DialogueManager.Instance.isDuringTyping)
                GetChoice(curNum);
        }
    }

    //생성된 버튼 갯수와 비교해서 curNum 숫자 제한
    void SetCurNum(int n)
    {        
        if (curNum + n >= choicesLength)
            curNum = choicesLength - 1;
        else if (curNum + n < 0)
            curNum = 0;
        else
            curNum += n;
    }

    //입력 처리하는 부분.
    public void GetChoice(int n)
    {
        //print(n);
        if (buttons[n].activeSelf)
        {
            foreach (GameObject item in buttons)
            {
                item.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }
            buttons[n].GetComponent<Image>().color = new Color(1, 1, 1, 1);
            ReturnTheAnswer(choices[n]);
        }
    }

    public void ReturnTheAnswer(Dialogue.DialogueSet.Details.Choices choice)  //이 함수 호출은 버튼들에서. or 여기 Update 리턴키 처리 부분에서.
    {
        //ChoiceBox 닫기. 애니메이션. 
        //isChoiceBox = false;
        animator.SetBool("isOpen", isChoiceBox = false);
        //DialogueManager에 보내기. 
        DialogueManager.Instance.PrintTheChoiceResult(choice);
    }


}
