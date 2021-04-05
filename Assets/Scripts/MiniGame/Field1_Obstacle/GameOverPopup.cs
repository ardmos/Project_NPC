using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPopup : MonoBehaviour
{
    [SerializeField]
    MiniGameManager miniGameManager;
   
    //미니게임 재시작
    public void RestartButtonClicked()
    {
        //재시작!
        Debug.Log("재시작!!");                     
        miniGameManager.RestartGame();
    }

    //미니게임 포기
    public void GiveUpButtonClicked()
    {
        //포기했다!!!  이전 씬으로 돌아가자.
        Debug.Log("포기!");
    }
}
