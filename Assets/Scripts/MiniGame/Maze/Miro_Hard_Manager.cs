using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Miro_Hard_Manager : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineVirtual;
    //상호작용 오브젝트들
    //문1, 문2, 가시, 수상한 꽃, 힐팩, 바위, 나무판1, 나무판2, 대왕거미줄
    public GameObject 문1, 문2, 가시, 수상한꽃, 힐팩, 바위, 나무판1, 나무판2, 대왕거미줄, 작업바, 타이머, 말풍선, 게임오버팝업, 게임승리팝업, 게임가이드팝업;

    //처음엔 가시, 힐팩 비활성화시켜야함
    //문1을 열면 가시 활성화시켜야함
    //문2를 열면 힐팩 활성화시켜야함


    //플레이어 오브젝트 
    KeyInput_Controller player;
    //플레이어 스타팅 포지션
    Vector2 playerStartingPos = new Vector2(1.6f,-12f);
    //동료 스타팅 포지션
    Vector2 fellowStartingPos = new Vector2(0f, -12f);


    //실패시 카메라 효과 위한 부분
    bool width = false;
    bool height = false;
    bool zoom = false;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<KeyInput_Controller>();
        InitMapObstacles();
    }

    void InitMapObstacles()
    {
        말풍선.SetActive(false);
        가시.SetActive(false);
        힐팩.SetActive(false);
        게임오버팝업.SetActive(false);

        문1.SetActive(true);
        문2.SetActive(true);
        수상한꽃.SetActive(true);
        바위.SetActive(true);
        나무판1.SetActive(true);
        나무판2.SetActive(true);
        대왕거미줄.SetActive(true);
    }

    //가시 활성화시켜주기
    public void Activate가시()
    {
        가시.SetActive(true);
    }
    //힐팩 활성화시켜주기
    public void Activate힐팩()
    {
        힐팩.SetActive(true);
    }
    //말풍선 활성화시켜주기
    public void Activate말풍선(Vector2 screenPos, string scanObjName)
    {
        if(말풍선.activeSelf != true && GameObject.Find("InterActiveBar") ==null)
        {
            말풍선.SetActive(true);
            Vector2 localPos = Vector2.zero;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(말풍선.GetComponent<RectTransform>(), screenPos, Camera.main, out localPos);
            //Debug.Log(localPos);

            //문이면 그대로, 다른애들이면 y를 +30 해주기 

            if (scanObjName.Contains("문"))
            {
                말풍선.GetComponent<RectTransform>().localPosition = localPos;
            }
            else
            {
                말풍선.GetComponent<RectTransform>().localPosition = new Vector2(localPos.x, localPos.y+30f);
            }


            //말풍선.GetComponent<RectTransform>().anchoredPosition = localPos;
        }       
    }
    //말풍선 비활성화 시켜주기
    public void DeActivate말풍선()
    {
        if (말풍선.activeSelf == true)
        {
            말풍선.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            말풍선.SetActive(false);
        }
    }


    //게임 승리 팝업 시작 
    public void Win()
    {
        //이동 정지 하고 
        player.movement = Vector2.zero;
        player.isControllable = false;
        //게임승리 팝업 띄워줘야함. 
        게임승리팝업.SetActive(true);

        //혹시 모르니 게임오버팝업은 꺼주자
        게임오버팝업.SetActive(false);
    }


    //게임 패배 팝업 시작
    public void GameOver()
    {
        Debug.Log("게임오버");
        //이동 정지 하고 
        player.movement = Vector2.zero;
        player.isControllable = false;

        //화면확대 스르륵, 음악재생 ㅋㅋㅋ 한 다음 
        //게임종료 팝업 띄워줘야함. 
        StartCoroutine(GameOverDirectingStart());
    }

    //게임 재도전
    public void RestartGame()
    {
        //다시 이동 가능하게하고
        player.isControllable = true;
        //장애물들 다 원상복귀 시키고
        InitMapObstacles();
        //파이어볼슈터 슈팅 트리거 꺼주고
        FindObjectOfType<FireShooter_TrapVer>().DeActivateFireShooterTrap();

        //타이머 원상봉귀 시키고
        타이머.GetComponent<Timer>().SetRemainTime(90);
        타이머.GetComponent<Timer>().isgameover = false;
        //플레이어캐릭터 출발위치로.
        player.gameObject.transform.position = playerStartingPos;
        //동료캐릭터 출발위치로. 
        //FindObjectOfType<NPC>().gameObject.transform.position = fellowStartingPos;

        //카메라 원위치 
        cinemachineVirtual.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneWidth = 0.8f;
        cinemachineVirtual.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = 0.7f;
        cinemachineVirtual.m_Lens.OrthographicSize = 8f;
        //게임종료 팝업 끄고.
        게임오버팝업.SetActive(false);
    }

    //게임 나가기
    public void LeaveThisMiniGame()
    {
        //이전 씬으로 이동
        Debug.Log("LeaveThisMiniGame()");
    }


    //게임오버시 화면확대스르륵, 음악재생, 게임오버팝업까지. 
    IEnumerator GameOverDirectingStart()
    {
        //일단 음악 재생 시작
        //지금은 없음


        //다음 화면 이동 스르륵 시작
        width = false;
        height = false;
        zoom = false;


        while (!width && !height)
        {
            //화면 중앙으로! 
            yield return new WaitForSeconds(0.001f);

            if (cinemachineVirtual.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneWidth >= 0.1f)
            {      
                cinemachineVirtual.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneWidth -= 0.001f;
            }
            else
            {
                width = true;
            }

            if (cinemachineVirtual.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight >= 0.1f)
            {
                cinemachineVirtual.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight -= 0.001f;
            }
            else
            {
                height = true;
            }
        }

        //다음 화면 확대 시작

        while (!zoom)
        {
            yield return new WaitForSeconds(0.001f);
            if(cinemachineVirtual.m_Lens.OrthographicSize >= 2f)
            {
                cinemachineVirtual.m_Lens.OrthographicSize -= 0.01f;
            }
            else
            {
                zoom = true;
            }
        }


        //마지막으로 2초 기다렸다가 게임오버 팝업 띄우기까지
        yield return new WaitForSeconds(1f);
        if (게임오버팝업.activeSelf == false)
            게임오버팝업.SetActive(true);

    }
}
