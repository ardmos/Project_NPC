﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    //오브젝트들 상호작용 여부 관리.
    public Dictionary<int, bool> isInteracted;
    public GameObject 스토리정리;

    //Scene1 오브젝트들
    //천형사, 경찰2
    public SpriteRenderer 천형사, 경찰2;

    //커서 이미지 설정
    public Texture2D cursorDefaultTexture, cursorMagGlassTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    protected override void OnAwake()
    {
        base.OnAwake();
        choiceResults = new Dictionary<string, int>();
        isInteracted = new Dictionary<int, bool>();
    }
    protected override void OnStart()
    {
        base.OnStart();

        //스토리정보 읽어오기.
        //foreach는 해보니까 없으면 알아서 안하게끔 처리되어있는듯. 오류없게끔. 사이즈0일때 예외처리 안해줘도 됨
        foreach (AttachThis attachThis in 스토리정리.GetComponentsInChildren<AttachThis>())
        {
            foreach (Dialogue item in attachThis.dialogues)
            {
                isInteracted.Add(item.storyId, false);
            }
        }

        //스토리1 등장인물들 클로킹
        //씬 1인지 씬을 확인하고,  
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            천형사.color = new Color(1, 1, 1, 0);
            경찰2.color = new Color(1, 1, 1, 0);
        }


        //커서 이미지 설정
        SetGameCursor("Default");
    }

    #region 커서 이미지 설정
    public void SetGameCursor(string type)
    {
        switch (type)
        {
            case "Default":
                Cursor.SetCursor(cursorDefaultTexture, hotSpot, cursorMode);
                break;
            case "MagGlass":
                Cursor.SetCursor(cursorMagGlassTexture, hotSpot, cursorMode);
                break;
            default:
                break;
        }
    }
    #endregion

    private void Update()
    {
        //딕셔너리에 선택상자 선택 결과물이 잘 들어왔는지 확인을 위한 프린트.
        if (choiceResults.Count > 0)
        {
            foreach (KeyValuePair<string, int> item in choiceResults)
            {
                //print("선택지 " + item.Key + ", 에서의 선택은 " + item.Value + " 입니다.");
            }
        }        
    }

    #region 메인 스토리 흐름 제어
    //스토리 이벤트 
    public void StartStoryEvent()
    {
        switch (storyNumber)
        {
            case 0:
                //첫 뚜벅뚜벅 다이얼로그 발동.
                DialogueManager.Instance.StartDialogue(storyNumber);
                break;
            case 1:
                //천형사 다이얼로그 발동.
                print("Here is GameManager.StartStoryEvent() 1 천형사 등장");
                //천형사, 경찰2 sprite 보이게 한 후
                천형사.color = new Color(1, 1, 1, 1);
                경찰2.color = new Color(1, 1, 1, 1);
                //천형사 애니메이션 포함된 다이얼로그 시작시키자.
                DialogueManager.Instance.StartDialogue(storyNumber);
                break;
            case 2:

            case 3:

            case 4:

            case 5:                
                break;

            case 31:
                print("Here is GameManager.StartStoryEvent() 31");
                //멧돼지게임
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
    #endregion


    #region 선택상자 관리
    //choiceResults 추가하는 곳.
    public void AddChoiceResults(string key, int value)
    {
        if (choiceResults.ContainsKey(key))
        {
            //이미 해당 키값의 딕셔너리가 존재한다면, 벨류값을 바꿔라
            choiceResults[key] = value;
        }
        else
            //해당 키값의 데이터가 이미 존재하는게 아니라면, 그냥 새로 추가.
            choiceResults.Add(key, value);
    }

    //choiceResults 가져오는 곳.
    public int GetChoiceResults(string key)
    {
        return choiceResults[key];
    }
    #endregion


    #region 오브젝트들 상호작용여부 관리
    //상호작용 여부 관리.
    //상호작용 흐름 기반 이벤트 발동 관리는 각자 오브젝트에서. 
    public void DidInteracted(int objId)
    {
        isInteracted[objId] = true;
    }        
    #endregion

}
