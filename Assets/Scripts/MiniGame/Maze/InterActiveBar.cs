using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterActiveBar : MonoBehaviour
{
    //상호작용바.
    //시작시 5초 차감.
    //슬라이더 게이지 자동 진행
    //가운데 텍스트
    //배경 이미지
    //중간에 이동하면 끊김

    public Slider slider;
    float delayTime;
    GameObject obstacleObj;    

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);        
    }

    // Update is called once per frame
    void Update()
    {
        //게이지 자동으로 차게끔.
        //if (Mathf.Floor(delayTime) <= 0)
        if (delayTime <= 0)
        {
            //Debug.Log("delayTime: " + delayTime);
            //0일때마다 게이지 증가.
            FillingGage();
            delayTime = 0.02f;
        }
        else
        {
            delayTime -= Time.deltaTime;
            //Debug.Log(Mathf.Floor(cooldownTime));
        }

        //슬라이더 게이지가 꽉 차면.  작업 완료 처리
        if (slider.value == 1f)
        {
            Debug.Log("게이지 꽉 찼다! 작업종료 시작 : " + obstacleObj);
            obstacleObj.GetComponent<Maze_Obstacle>().CompleteTask();
            gameObject.SetActive(false);
        }
    }

    //private void FillingGage

    //게이지 자동충전중...
    private void FillingGage()
    {
        slider.value += 0.02f;
    }

    //상호작용바 시작
    public void StartInterActiveBar(string prompt, GameObject obstacle)
    {
        slider = gameObject.GetComponent<Slider>();
        slider.value = 0f;
        delayTime = 0.02f;
        obstacleObj = obstacle;

        //문구 설정
        gameObject.GetComponentInChildren<Text>().text = prompt;

    }

    //상호작용바 강제종료
    public void StopInterActiveBar()
    {
        obstacleObj.GetComponent<Maze_Obstacle>().isTaskStarted = false;
        gameObject.SetActive(false);        
    }

}
