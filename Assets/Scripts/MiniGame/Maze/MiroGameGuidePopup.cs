using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiroGameGuidePopup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<Timer>().PauseTimer();
        FindObjectOfType<KeyInput_Controller>().isControllable = false;
    }

    public void OnBtnClick_GameStart()
    {
        FindObjectOfType<Timer>().StartTimer();
        FindObjectOfType<KeyInput_Controller>().isControllable = true;
        Destroy(gameObject);
    }
}
