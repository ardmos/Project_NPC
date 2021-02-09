using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultPopup : MonoBehaviour
{
    public int selectedModeNumber;
    public Text mainText;


    private void Start()
    {
        gameObject.transform.localScale = Vector3.zero;
    }

    public void SetModeNumber()
    {
        //드롭다운의 모드넘버값이 바뀌면 그 값을 저장해둔다. 
        selectedModeNumber = gameObject.GetComponent<Dropdown>().value;
        print("selectedModeNumber: " + selectedModeNumber);        
    }

    public void RestartMiniGame()
    {
        //바뀐 값을 새로 넣어준다
        GameManager.Instance.choiceResults["31_0"] = selectedModeNumber;
        //게임을 새로 시작해준다.
        FindObjectOfType<CatchingWildBoar.GameManager>().StartMiniGame();
        //팝업을 다시 닫아준다.
        gameObject.transform.localScale = Vector3.zero;
    }

    public void EndMiniGame()
    {
        //이전 씬으로 이동. 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
    }

    public void MakePopup(int n)
    {
        switch (n)
        {
            case 0:
                //실패
                mainText.text = "실패";
                break;

            case 1:
                //성공
                mainText.text = "성공";
                break;

            default:
                break;
        }

        gameObject.transform.localScale = new Vector3(1f, 1f);
    }
}
