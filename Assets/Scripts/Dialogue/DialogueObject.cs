using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueObject : MonoBehaviour
{
    public Text dialogObjName, sentence;
    public Image portrait;
    public string[] sentences;
    public Sprite[] portraits;

    public void SetData(Dialogue dialogueData)
    {
        this.dialogObjName.text = dialogueData.situationName;
        sentences = dialogueData.sentences;
        portraits = dialogueData.portraits;

        if (dialogueData.isPortrait)
            this.portrait.color = new Color(1, 1, 1, 1);
        else
            this.portrait.color = new Color(1, 1, 1, 0);
    }   

}
