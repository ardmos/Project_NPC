using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace CatchingWildBoar
{
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// 이곳은 미니게임<멧돼지>의 게임 매니저
        /// 
        /// 추가할것들
        ///  1.성공시, 실패시 다이얼로그 출력되는부분 추가.
        ///  2.씬4_베릴성과의 알맞은 씬 전환 연결.
        ///  3.LevelChanger_WithSentence 추가 및 활용.
        /// </summary>



        //멧돼지 잡은 숫자 카운트. 
        public int catchCount;
        public bool isClear;

        //성공or실패 팝업
        public GameObject popupObj;

        //잡고나서 어떻게 할지 선택상자 다이얼로그 호출. 
        //멧돼지 어떻게할지 선택상자 id번호 31_0        

        //동물 생성할 부쉬 계속 바꿔주기 위한 부분
        [HideInInspector]
        public List<GameObject> bushes = new List<GameObject>();
        [HideInInspector]
        public int n;
        [HideInInspector]
        public int m;
        [HideInInspector]
        public int j;
        [HideInInspector]
        public int frameCounter, genTiming;

        //상단 정보 표시를 위한 텍스트UI
        public Text timerText, countText;

        /* //커서 변경을 위한 부분. 지금은 안씀.
        public Texture2D cursorTexture;
        public CursorMode cursorMode = CursorMode.Auto;
        public Vector2 hotSpot = Vector2.zero;
        */

        //타이머
        //[HideInInspector]
        public float time;
        public bool isTimeZero;


        //때려서 쫒을것인가, 간지럽혀서 쫒을것인가.
        public bool isStrokeMode;        

        private void Start()
        {
            StartMiniGame();       
        }

        public void StartMiniGame()
        {
            countText.text = "Pase 1  Count : 0";
            foreach (GameObject bush in GameObject.FindGameObjectsWithTag("Bush"))
            {
                bushes.Add(bush);
            }
            
            foreach (GameObject animals in GameObject.FindGameObjectsWithTag("Animal"))
            {
                //재시작할경우를 대비해서. 이미 있는 동물들 다 없애고 시작. 
                Destroy(animals);
            }

            time = 60;
            catchCount = 0;
            isClear = false;
            isTimeZero = false;

            print("StartMiniGame()");
            //GameManager로부터 사용자의 선택 결과를 읽어와서 isStrokeMode를 설정해준다.            
            switch (global::GameManager.Instance.GetChoiceResults("31_0"))
            {
                case 0:
                    //쓰다듬는다
                    isStrokeMode = true;
                    break;
                case 1:
                    //때린다
                    isStrokeMode = false;
                    break;
                default:
                    break;
            }
        }

        private void Update()
        {
            //타이머
            if (time > 0 && !isClear) time -= Time.deltaTime;
            else if (time <= 0 && isTimeZero==false)
            {
                //실패시 과정 처리.
                //1. 실패 결과 팝업을 띄우고
                //2. 다시하기를 누르면 미니게임 다시 시작해주고 (살리기 or 죽이기 선택해서. -  GameManager의 31_0의 값을 바꿔주자. 그럼 시작시에 자동으로 변경 적용 될 것.  )
                //2. 나가기를 누르면 그냥 이 전 씬으로 이동한다. 여기서는 인덱스 -1 해주면 됨. 지금 이곳은 그렇다.
                isTimeZero = true;
                //실패 팝업 0
                FindObjectOfType<ResultPopup>().MakePopup(0);
                print("으악!실패!");

            }   //실패!!! 팝업!!띄우기.! 재도전? 나가기? 

            timerText.text = Mathf.Ceil(time).ToString();

            //50마리 넘게 잡으면 
            if (catchCount >= 50 && !isClear)
            {
                //커서 원래대로 변경
                //Cursor.SetCursor(null, Vector2.zero, cursorMode);

                countText.text = "Clear!  Count : " + catchCount.ToString();
                isClear = true;
                //성공 팝업 시작 
                StartPopup();

                print("와! 성공!");
            }   //시간 내 잡기 성공           
       
            if (!isClear && catchCount<50)
            {
                //커서 변경
                //Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);

                if (frameCounter == genTiming)
                {
                    frameCounter = 0;

                    if (catchCount<=20)
                    {
                        // 잡은게 20마리까진 한 부쉬에서 1마리 생성하면, 바꿔줌.
                        ChooseRandomBushToGenerate();
                        countText.text = "Pase 1  Count : " + catchCount.ToString();
                    }
                    else if (catchCount<=35)
                    {
                        // 잡은게 21마리 부터는 한 번에 두 곳에서 생성됨. 
                        ChooseRandom_Two_BushesToGenerate();
                        countText.text = "Pase 2  Count : " + catchCount.ToString();
                    }
                    else
                    {
                        // 잡은게 46마리 부터는 한 번에 세 곳에서 생성됨.
                        ChooseRandom_Three_BushesToGenerate();
                        countText.text = "Pase 3  Count : " + catchCount.ToString();
                    }
                }
                frameCounter++;
            }   //멧돼지 페이스 조절
            
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

        void StartPopup()
        {
            //성공 팝업 1
            FindObjectOfType<ResultPopup>().MakePopup(1);
        }
    }
}

