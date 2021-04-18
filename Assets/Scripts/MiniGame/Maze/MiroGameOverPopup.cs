using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiroGameOverPopup : MonoBehaviour
{
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
