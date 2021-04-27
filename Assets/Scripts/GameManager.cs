using System.Collections;
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
    public GameObject 천형사, 경찰2;

    //커서 이미지 설정
    public Texture2D cursorDefaultTexture, cursorMagGlassTexture, cursorMagGlassTexture_HighLighted;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    //ESC메뉴팝업 오브젝트
    public GameObject escPopupObj;
    public bool isEscOpen;

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
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Scene1_Incident")
        {
            천형사.SetActive(false);
            //경찰2.SetActive(false);  <<- 이제는 숨어있지 않음!
        }


        //커서 이미지 설정
        SetGameCursor("Default");
    }


    private void Update()
    {
        //ESC 눌렸을 시.  메뉴 팝업 띄우기
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (isEscOpen == false)
            {
                Debug.Log("ESC");
                escPopupObj.SetActive(true);
                isEscOpen = true;
            }
            else
            {
                escPopupObj.SetActive(false);
                isEscOpen = false;
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
                ///씬1_사건현장 시작.
                //첫 뚜벅뚜벅 다이얼로그 발동.
                DialogueManager.Instance.StartDialogue(storyNumber);
                break;
            case 1:
                //천형사 다이얼로그 발동.
                print("Here is GameManager.StartStoryEvent() 1 천형사 등장");
                //천형사, 경찰2 sprite 보이게 한 후
                //foreach (SpriteRenderer spriteRenderer in 천형사.GetComponentsInChildren<SpriteRenderer>())
                //{
                //    spriteRenderer.color = new Color(1, 1, 1, 1);
                //}
                //foreach (SpriteRenderer spriteRenderer in 경찰2.GetComponentsInChildren<SpriteRenderer>())
                //{
                //    spriteRenderer.color = new Color(1, 1, 1, 1);
                //}
                천형사.SetActive(true);
                //경찰2.SetActive(true); <<-- 이제는 숨어있지 않음

                //천형사 애니메이션 포함된 다이얼로그 시작시키자.
                DialogueManager.Instance.StartDialogue(storyNumber);
                break;
            case 2:
                //페이드 아웃, 씬 이동 
                LevelChanger_ForScene1.instance.FadeToNextLevel();
                //다음날, 경찰서 취조실
                break;
            case 3:
                ///씬2_취조실 시작.   (LevelChanger_ForScene2.cs에서 페이드인 애니메이션 끝났을 때  호출.)
                DialogueManager.Instance.StartDialogue(storyNumber);
                break;
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
    public void AddChoiceResults(string questionSentence, int linkedStoryDialogueIdNumber)
    {
        if (choiceResults.ContainsKey(questionSentence))
        {
            //이미 해당 키값의 딕셔너리가 존재한다면, 벨류값을 바꿔라
            choiceResults[questionSentence] = linkedStoryDialogueIdNumber;
        }
        else
            //해당 키값의 데이터가 이미 존재하는게 아니라면, 그냥 새로 추가.
            choiceResults.Add(questionSentence, linkedStoryDialogueIdNumber);
    }

    //choiceResults 가져오는 곳.
    public int GetChoiceResults(string questionSentence)
    {
        return choiceResults[questionSentence];
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
            case "MagGlass_HL":
                Cursor.SetCursor(cursorMagGlassTexture_HighLighted, hotSpot, cursorMode);
                break;
            default:
                break;
        }
    }
    #endregion
}
