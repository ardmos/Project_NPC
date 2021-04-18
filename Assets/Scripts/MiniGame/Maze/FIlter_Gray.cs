using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FIlter_Gray : MonoBehaviour
{
    public void MakeGameOverPopup()
    {
        FindObjectOfType<MiroGameOverPopup>().MakePopup();
    }

    public void StartRestartGame()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Contains("Hard"))
        {
            FindObjectOfType<Miro_Hard_Manager>().RestartGame();
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Contains("Easy"))
        {

        }
    }
}
