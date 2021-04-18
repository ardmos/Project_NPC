using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiroGameOverPopup : MonoBehaviour
{
    public GameObject popupBG;

    private void Start()
    {
        popupBG.SetActive(false);
    }

    public void MakePopup()
    {
        //얘도 애니메이션으로 하자. 
        popupBG.SetActive(true);
    }

    public void OnClickBtnRestartGame()
    {
        GetComponentInChildren<Animator>().SetTrigger("StartFadeIn");        
    }

    public void OnClickBtnLeaveGame()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Contains("Hard"))
        {
            FindObjectOfType<Miro_Hard_Manager>().LeaveThisMiniGame();
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Contains("Easy"))
        {

        }
    }
}
