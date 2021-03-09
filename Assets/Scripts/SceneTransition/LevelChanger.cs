using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{

    public Animator animator;

    int levelToLoad;


    #region 싱글턴
    public static LevelChanger instance;

    private void Awake()
    {
        instance = this;        
    }
    #endregion


    private void Update()
    {
        
        if (Input.GetMouseButton(0))
        {
            FadeToNextLevel();
        }
        
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

}

