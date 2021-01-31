﻿using System.Collections;
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

    int length;

    public void InitChioceBox(string mAsk, string[] mChoices) 
    {
        isChoiceBox = true;
        question = mAsk;
        choices = mChoices;

        //각 텍스트를 오브젝트텍스트에 넣어주는 과정.
        questionText.text = question;

        length = choices.Length;

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
            if(Input.GetKeyDown(KeyCode.Keypad1))
            {
                GetChoice(1);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                GetChoice(2);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                GetChoice(3);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                GetChoice(4);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                GetChoice(5);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                GetChoice(6);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                GetChoice(7);

            }
            else if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                GetChoice(8);
            }
        }
    }

    //입력 처리하는 부분.
    public void GetChoice(int n)
    {
        print(n);
        if (buttons[n-1].activeSelf)
        {
            ReturnTheAnswer(n-1);
        }
    }

}
