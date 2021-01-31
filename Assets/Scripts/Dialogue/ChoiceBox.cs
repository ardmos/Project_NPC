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
    public string[] choices;

    public GridLayoutGroup gridleft;
    public Animator animator;
    public Text questionText;
    public GameObject[] buttons;

    int curNum, tmp, choicesLength;

    public void InitChioceBox(string mAsk, string[] mChoices) 
    {
        isChoiceBox = true;
        question = mAsk;
        choices = mChoices;
        curNum = 0;
        tmp = 0;
        choicesLength = DialogueManager.instance.curDialogSet.detail.selectionPopupData.choices.Length;

        //각 텍스트를 오브젝트텍스트에 넣어주는 과정.
        questionText.text = question;

        int length = choices.Length;

        if (length<=4)        
            gridleft.cellSize = new Vector2(280f, 45f);    
        else
            gridleft.cellSize = new Vector2(140f, 45f);

        for (int i = 0; i < length; i++)
        {
            buttons[i].GetComponentInChildren<Text>().text = choices[i];
        }

        for (int i = length; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }

        //ChoiceBox 열기. 애니메이션.
        animator.SetBool("isOpen", isChoiceBox);
    }

    public void ReturnTheAnswer(int valueToReturn)  //이 함수 호출은 버튼들에서. 
    {
        //ChoiceBox 닫기. 애니메이션. 
        isChoiceBox = false;
        animator.SetBool("isOpen", isChoiceBox);
        //DialogueManager에 보내기. 
        DialogueManager.instance.PrintTheChoiceResult(valueToReturn);
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

            if (Input.GetKeyDown(KeyCode.Space))
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
        print(n);
        if (buttons[n].activeSelf)
        {
            ReturnTheAnswer(n);
        }
    }

}
