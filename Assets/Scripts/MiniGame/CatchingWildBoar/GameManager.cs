using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CatchingWildBoar
{
    public class GameManager : MonoBehaviour
    {
        //멧돼지 잡은 숫자 카운트. 
        public int catchCount;
        public bool isClear;

        //잡고나서 어떻게 할지 선택상자 다이얼로그 호출. 
        //멧돼지 어떻게할지 선택상자 id번호 31


        private void Update()
        {
            //50마리 넘게 잡으면 
            if (catchCount >= 50)
            {
                isClear = true;
                //다이얼로그 시작 
                StartChoice();
            }
            else
                isClear = false;
        }

        void StartChoice()
        {
            //자동생성 정지
            foreach (Bush bush in FindObjectsOfType<Bush>())
            {
                bush.StopGenerating();
            }
            //다이얼로그 시작
            DialogueManager.instance.StartDialogue(31);
        }
    }
}

