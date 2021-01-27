using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtonController : MonoBehaviour
{
    public void OnButtonClick_Continue()
    {
        //이어하기
        Debug.Log("이어하기버튼 클릭");
    }

    public void OnButtonClick_StartNewGame()
    {
        //처음부터 시작.
        SceneManager.LoadScene("SampleRoom");
    }

    public void OnButtonClick_Gallary()
    {
        //갤러리씬 이동
        Debug.Log("갤러리씬 오픈");        
        SceneManager.LoadScene("SampleGallary");
    }
    public void OnButtonClick_Option()
    {
        //옵션창 팝업
        FindObjectOfType<OptionPopUp>().OpenThisPopup();
    }
    public void OnButtonClick_EndGame()
    {
        //게임 종료
        Application.Quit();
    }

}
