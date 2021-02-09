using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        //딕셔너리에 선택상자 선택 결과물이 잘 들어왔는지 확인을 위한 프린트.
        if (choiceResults.Count>0)
        {
            foreach (KeyValuePair<string, int> item in choiceResults)
            {
                print("선택지 " + item.Key + ", 에서의 선택은 " + item.Value + " 입니다.");
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

            case 31:
                //멧돼지게임
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
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



    //choiceResults 추가하는 곳.
    public void AddChoiceResults(string key, int value)
    {
        if (choiceResults.ContainsKey(key))
        {
            //이미 해당 키값의 딕셔너리가 존재한다면, 벨류값을 바꿔라
            choiceResults[key] = value;
        }
        //해당 키값의 데이터가 이미 존재하는게 아니라면, 그냥 새로 추가.
        choiceResults.Add(key, value);
    }

    //choiceResults 가져오는 곳.
    public int GetChoiceResults(string key)
    {
        return choiceResults[key];
    }

}
