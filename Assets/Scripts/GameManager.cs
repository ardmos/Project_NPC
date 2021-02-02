using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region For Singleton
    public static GameManager instance;

    private void Awake()
    {
        instance = this;    
    }
    #endregion

    [Header("현재 진행중 스토리")]
    public int storyNumber;

    //스토리 이벤트 
    public void StartStoryEvent()
    {
        switch (storyNumber)
        {
            case 0:
                //첫 뚜벅뚜벅 다이얼로그 발동.                  
            case 1:
                
            case 2:
                
            case 3:
                
            case 4:
                
            case 5:
                DialogueManager.instance.StartDialogue(storyNumber);
                break;
            default:
                break;
        }
    }



    //스토리 클리어
    public void StoryClear()
    {
        storyNumber++;
    }



}
