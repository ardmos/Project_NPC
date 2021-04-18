using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{    
    public Text text;
    public float remainTime = 90;
    public Miro_Hard_Manager miro_Hard_Manager;
    bool timerStopper;
    
    // Update is called once per frame
    void Update()
    {
        if(timerStopper== false)
        {
            if (Mathf.Floor(remainTime) <= 0f)
            {
                //시간 모두 소모! 실패!
                //Debug.Log("시간 모두 소모! 실패!!!");
                remainTime = 0;
                text.text = remainTime.ToString();

                //실패!
                miro_Hard_Manager.GameOver();
            }
            else
            {
                if (Mathf.Floor(remainTime) <= 10f)
                {
                    //두근두근애니메이션 추가하기
                    GetComponent<Animator>().SetTrigger("TimerPopMinus");
                }

                remainTime -= Time.deltaTime;
                text.text = Mathf.Floor(remainTime).ToString();
            }
        }
    }

    //시간 가감
    public void PlusTime(int time)
    {
        //remainTime을 늘려야함
        remainTime += time;
        //글씨색 잠시동안 초록색, 크기 키우기        
        GetComponent<Animator>().SetTrigger("TimerPopPlus");
    }
    public void MinusTime(int time)
    {
        //remainTime을 줄여야함
        remainTime -= time;
        //글씨색 잠시동안 빨간색, 크기 키우기
        GetComponent<Animator>().SetTrigger("TimerPopMinus");
    }
    public void SetRemainTime(int time)
    {
        remainTime = time;
    }

    //타이머 일시정지
    public void PauseTimer()
    {
        timerStopper = true;
    }

    //타이머 재시작
    public void StartTimer()
    {
        timerStopper = false;
    }



}
