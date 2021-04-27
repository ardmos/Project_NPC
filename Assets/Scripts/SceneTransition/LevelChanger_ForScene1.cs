using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelChanger_ForScene1 : MonoBehaviour
{

    public Animator animator;
    public Text text;
    public AudioSource audioSource;
    public AudioClip audioClip;

    int levelToLoad;


    #region 싱글턴
    public static LevelChanger_ForScene1 instance;

    private void Awake()
    {
        instance = this;        
    }
    #endregion

    private void Start()
    {
        //처음 날짜 도도도도 찍어주기.
        StartCoroutine(TypeSentence("20XX년 1월 3일 오후 2시, 경기도 A빌라 306호"));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F8))
        {
            FadeToNextLevel();
        }
    }


    //한글자씩 도도도 찍기
    IEnumerator TypeSentence(string str)
    {
        text.text = "";
        foreach (char letter in str.ToCharArray())
        {
            
            text.text += letter;

            //Debug.Log(text.text);

            //타이핑 효과음 출력 부분.  스페이스는 거른다. 
            if (letter != System.Convert.ToChar(32))
            {
                audioSource.PlayOneShot(audioClip);
            }

            yield return new WaitForSeconds(0.08f);
        }

        //출력 끝나면,  스토리 이벤트 시작 (낡은 빌라에 도착...)
        GameManager.Instance.StartStoryEvent();
    }


    public void FadeToNextLevel()   //이걸 호출하면 빌드세팅 씬 넘버 순서상 다음 씬으로 넘어감.! 
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




    //씬2 등 다른 씬에서 사용하는 녀석.  씬1에서는 필요 없지만, 애니메이션을 공유하는 이상 함수 자체는 존재해야 한다. 아무런 기능을 넣지 않는 함수
    public void StartDialogue()
    {        
    }
}

