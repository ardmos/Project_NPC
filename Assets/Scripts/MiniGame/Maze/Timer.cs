using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{    
    public Text text;
    float remainTime = 90;
    
    // Update is called once per frame
    void Update()
    {
        if (Mathf.Floor(remainTime) <= 0f)
        {
            //시간 모두 소모! 실패!
            //Debug.Log("시간 모두 소모! 실패!!!");            
        }
        else
        {
            remainTime -= Time.deltaTime;
            text.text = Mathf.Floor(remainTime).ToString();
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



}
