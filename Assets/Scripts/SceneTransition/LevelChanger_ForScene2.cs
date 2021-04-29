using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelChanger_ForScene2 : MonoBehaviour
{

    public Animator animator;
    public Text text;
    public AudioSource audioSource;
    public AudioClip audioClip;

    int levelToLoad;

    bool forScene2_BalckScreen;

    #region 싱글턴
    public static LevelChanger_ForScene2 instance;

    private void Awake()
    {
        instance = this;        
    }
    #endregion

    private void Start()
    {
        //처음 날짜 도도도도 찍어주기.
        StartCoroutine(TypeSentence("다음날, 00경찰서 취조실"));
        //까만배경 페이드인 애니메이션 실행
        animator.SetBool("DoFadeIn", true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F8))
        {
            FadeToNextLevel();
        }
    }


    //애니메이션 이벤트로 호출되는 함
    public void StartDialogue()
    {
        //Scene2_취조실 다이얼로그 최초 시작
        Debug.Log(GameManager.Instance.GetStoryNumber());
        GameManager.Instance.SetStoryNumber(3);
        Debug.Log(GameManager.Instance.GetStoryNumber());
        GameManager.Instance.StartStoryEvent();
    }



    //한글자씩 도도도 찍기
    IEnumerator TypeSentence(string str)
    {
        text.text = "";
        //foreach (char letter in str.ToCharArray()) ToCharArray()가 중복이라고 한다.
        foreach (char letter in str)
        {
            text.text += letter;

            //타이핑 효과음 출력 부분.  스페이스는 거른다. 
            if (letter != System.Convert.ToChar(32))
            {
                //audioSource.PlayOneShot(audioClip);
            }

            yield return new WaitForSeconds(0.08f);
        }

    }


    #region 씬 넘기기용
    public void FadeToNextLevel()   //이걸 호출하면 빌드세팅 씬 넘버 순서상 다음 씬으로 넘어감! 
    {
        print("FadeToNextLevel");
        FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void FadeToLevel(int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()    //애니메이션이벤트를 사용했음.
    {
        print("OnFadeComplete");

        if (forScene2_BalckScreen)
        {
            //까만화면인채로 다이얼로그 시작. 해야함.  이걸 OnFadeComplete에서 한 번에 처리.  다른 페이드아웃이지만. 페이드아웃 컴플릿 메소드는 공유중!        
            //씬2_까만화면 실행할 차례면 아래 내용
            //씬2_까만화면 다이얼로그 시작할 차례.
            GameManager.Instance.SetStoryNumber(5);
            GameManager.Instance.StartStoryEvent();
        }
        else
        {
            //씬3_다시취조실이면 다음씬으로.
            SceneManager.LoadScene(levelToLoad);
        }               
    }
    #endregion


    #region 씬2_검은화면용
    public void StartScene2_Black()
    {
        forScene2_BalckScreen = true;
        //페이드아웃 시키고,
        
        animator.SetTrigger("FadeOut_Scene2_BlackScreen");                
    }        
    #endregion
}

