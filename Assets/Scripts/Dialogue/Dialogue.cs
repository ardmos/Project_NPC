using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue 
{
    public string situationName;
    public int storyId;             //
    [Range(0f, 1f)]
    public float letterSpeed;
    

    [Header("- Sentence 입력 방법 -> \"이름:문장:스타일(0~1):초상화번호(-1 없음)\" ")]
    [TextArea(8, 10)]
    public string[] sentences;      //이름, 문장, 스타일, 초상화번호

    [Header("- 초상화")]
    public bool isPortrait;
    public Sprite[] portraits;

    [Header("- 선택지")]
    public string ask;
    public string[] choices;
    public string[] choice_results;

}
