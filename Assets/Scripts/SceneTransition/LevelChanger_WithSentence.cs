using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelChanger_WithSentence : MonoBehaviour
{

	public Animator animator;
	public Text text;	
	int levelToLoad;

	//현재 씬 최초 다이얼로그의 ID넘버
	public int storyNumber;
	//현재 씬 최초 시작 문장
	public string sentence;
	#region 싱글턴
	public static LevelChanger_WithSentence instance;

	private void Awake()
	{
		instance = this;
	}
	#endregion

	private void Start()
	{
		//처음 날짜 도도도도 찍어주기.
		StartCoroutine(TypeSentence(sentence));
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
		//해당 씬 다이얼로그 최초 시작
		Debug.Log(GameManager.Instance.GetStoryNumber());
		GameManager.Instance.SetStoryNumber(storyNumber);
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

		//다음 씬으로 씬 전환
		SceneManager.LoadScene(levelToLoad);
	}
	#endregion

}

