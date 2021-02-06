using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : DontDestroy<GameManager>
{
    #region For Singleton <---- DontDestroy 를 통해 싱글턴. 
    //public static GameManager instance;

    //private void Awake()
    //{
    //    instance = this;    
    //}
    #endregion

    [Header("현재 진행중 스토리. 자동진행 스토리 관리")]
    public int storyNumber;

    [Header("선택상자의 선택 결과들.")]
    public Dictionary<string, int> choiceResults;

    protected override void OnAwake()
    {
        base.OnAwake();
        choiceResults = new Dictionary<string, int>();
    }


    private void Update()
    {
        if (choiceResults.Count>0)
        {
            foreach (KeyValuePair<string, int> item in choiceResults)
            {
                print(item.Key + ", 에서의 선택은 " + item.Value + " 입니다.");
            }
        }
    }

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
                DialogueManager.Instance.StartDialogue(storyNumber);
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
