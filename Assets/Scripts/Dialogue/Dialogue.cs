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
    

    [Header("- Sentence 입력 방법 -> \"이름:문장:스타일(0~1):초상화번호(-1 없음):선택팝업(1열기)\" ")]
    [TextArea(8, 10)]
    public string[] sentences;      //이름, 문장, 스타일, 초상화번호, 선택지발동팝업

    [Header("- 초상화")]
    public bool isPortrait;
    public Sprite[] portraits;

    [Header("- 선택지  Choices와 Choice_results와 responses는 반드시 짝을 이루어야 한다.")]
    public string ask;
    public string[] choices;
    [Header("- Sentence 입력 방법과 마찬가지로 입력.")]
    public string[] choice_results;
    [Header("- Choice_results에 대한 엔피씨의 답변내용 입니다. 입력방법은 Sentence와 동일합니다.")]
    public string[] responses;

}
