using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueObject : MonoBehaviour
{
    public Text dialogObjName, sentence;
    public Image portrait;

    public void SetData(Text name, Text sentence, Sprite portrait, bool isPortrait)
    {
        this.dialogObjName = name;
        this.sentence = sentence;
        this.portrait.sprite = portrait;

        if (isPortrait)
            this.portrait.color = new Color(1, 1, 1, 1);
        else
            this.portrait.color = new Color(1, 1, 1, 0);
    }   

}
