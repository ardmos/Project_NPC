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


    public void StartDialogue()
    {
        Debug.Log(GameManager.Instance.storyNumber);
        GameManager.Instance.storyNumber = 3;
        Debug.Log(GameManager.Instance.storyNumber);
        GameManager.Instance.StartStoryEvent();
    }



    //한글자씩 도도도 찍기
    IEnumerator TypeSentence(string str)
    {
        text.text = "";
        foreach (char letter in str.ToCharArray())
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
        SceneManager.LoadScene(levelToLoad);
    }
    #endregion

}

