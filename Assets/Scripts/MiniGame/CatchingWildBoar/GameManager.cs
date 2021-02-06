using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace CatchingWildBoar
{
    public class GameManager : MonoBehaviour
    {
        //멧돼지 잡은 숫자 카운트. 
        public int catchCount;
        public bool isClear;

        //잡고나서 어떻게 할지 선택상자 다이얼로그 호출. 
        //멧돼지 어떻게할지 선택상자 id번호 31

        //동물 생성할 부쉬 계속 바꿔주기 위한 부분
        public List<GameObject> bushes;
        public int n;
        public int m;
        public int j;
        public int frameCounter, genTiming;


        //디버깅을 위한 텍스트
        public Text text;


        //커서 변경을 위한 부분
        public Texture2D cursorTexture;
        public CursorMode cursorMode = CursorMode.Auto;
        public Vector2 hotSpot = Vector2.zero;

        private void Start()
        {
            foreach (GameObject bush in GameObject.FindGameObjectsWithTag("Bush"))
            {
                bushes.Add(bush);
            }
        }

        private void Update()
        {
            //50마리 넘게 잡으면 
            if (catchCount >= 50 && !isClear)
            {
                //커서 원래대로 변경
                Cursor.SetCursor(null, Vector2.zero, cursorMode);

                text.text = "Clear!  Count : " + catchCount.ToString();
                isClear = true;
                //다이얼로그 시작 
                StartChoice();
            }

            //text.text = " Count : " + catchCount.ToString();


            
            if (!isClear && catchCount<50)
            {
                //커서 변경
                Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);

                if (frameCounter == genTiming)
                {
                    frameCounter = 0;

                    if (catchCount<=20)
                    {
                        // 잡은게 20마리까진 한 부쉬에서 1마리 생성하면, 바꿔줌.
                        ChooseRandomBushToGenerate();
                        text.text = "Pase 1  Count : " + catchCount.ToString();
                    }
                    else if (catchCount<=45)
                    {
                        // 잡은게 21마리 부터는 한 번에 두 곳에서 생성됨. 
                        ChooseRandom_Two_BushesToGenerate();
                        text.text = "Pase 2  Count : " + catchCount.ToString();
                    }
                    else
                    {
                        // 잡은게 46마리 부터는 한 번에 세 곳에서 생성됨.
                        ChooseRandom_Three_BushesToGenerate();
                        text.text = "Pase 3  Count : " + catchCount.ToString();
                    }
                }
                frameCounter++;
            }
            

        }

        public void ChooseRandomBushToGenerate()
        {
            n = Random.Range(0, bushes.Count - 1);

            bushes[n].GetComponent<Bush>().generateSwitch = true;
        }

        public void ChooseRandom_Two_BushesToGenerate()
        {
            n = Random.Range(0, bushes.Count - 1);
            m = Random.Range(0, bushes.Count - 1);

            bushes[n].GetComponent<Bush>().generateSwitch = true;
            bushes[m].GetComponent<Bush>().generateSwitch = true;
        }

        public void ChooseRandom_Three_BushesToGenerate()
        {
            n = Random.Range(0, bushes.Count - 1);
            m = Random.Range(0, bushes.Count - 1);
            j = Random.Range(0, bushes.Count - 1);

            bushes[n].GetComponent<Bush>().generateSwitch = true;
            bushes[m].GetComponent<Bush>().generateSwitch = true;
            bushes[j].GetComponent<Bush>().generateSwitch = true;
        }

        void StartChoice()
        {
            //다이얼로그 시작
            DialogueManager.Instance.StartDialogue(31);
        }
    }
}

