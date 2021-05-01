using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scene0_Title의 씬 매니저.
/// 하는 일.
/// 1. BGM의 관ㄹ
/// </summary>


public class TitleSceneManager : MonoBehaviour
{
    [SerializeField]
    AudioClip bgmClip;

    // Start is called before the first frame update
    void Start()
    {
        AudioSystem.Instance.ActivateBGM(bgmClip);   
    }
}
