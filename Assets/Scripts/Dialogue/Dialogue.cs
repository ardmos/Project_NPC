using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public int id;
    public string name;
    [TextArea(8, 10)]
    public string sentence;
    public Sprite portrait;
    public bool isPortrait;
}
