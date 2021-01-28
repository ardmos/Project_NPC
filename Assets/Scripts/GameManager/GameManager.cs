using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text Dialogue_Slide_NameText, Dialogue_Slide_SentenceText;
    public Text Dialogue_Stable_NameText, Dialogue_Stable_SentenceText;
    public GameObject scanObject;
    public bool isScanAction;

    public void ScanAction(GameObject scanObj, string dialogueStyle)
    {
        if (isScanAction)   
        {
            //End ScanAction
            isScanAction = false;
            DialogueManager.instance.EndDialogue(dialogueStyle);
        }
        else
        {
            //Start ScanAction
            isScanAction = true;
            //DialogueManager.instance.StartDialogue(dialogueStyle);
            scanObject = scanObj;
            Dialogue_Stable_NameText.text = scanObject.name;
            Dialogue_Stable_SentenceText.text = "이것의 이름은 " + scanObject + "이라고 한다.";
        }

    }
}
