using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorField : MonoBehaviour
{
    //접근 감지 스크립트

    //해당 접근 감지 스크립트가 작동하길 원하는 storyNumber를 입력해줘야한다.
    [Header("- 해당 접근 감지 스크립트 작동하길 원하는 storyNumber 입력")]
    public int storyNum;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //영역 내 접근 감지
        //print(collision.gameObject.name + "here");


        //플레이어일 경우
        if(collision.CompareTag("Player") && GameManager.Instance.storyNumber == storyNum)
        {
            //이벤트 시작! *(GameManager 내부에서 자동으로 현재 스토리 진행상황에 맞는 다이얼로그 이벤트가 발동됨. )
            GameManager.Instance.StartStoryEvent();
            gameObject.SetActive(false);
        }
    }
}
