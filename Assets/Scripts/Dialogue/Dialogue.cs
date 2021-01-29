using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string situationName;
    public int storyId;    
    [TextArea(8, 10)]
    public string[] sentences;
    public Sprite[] portraits;
    public bool isPortrait;
    [Range(0f,1f)]
    public float letterSpeed;
    public string[] choices;
    public string[] choice_results;
}
