using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Dialogue : MonoBehaviour
{
    int id;

    public void TriggerDialogue()
    {
        DialogueManager.instance.StartDialogue(id);
    }

    private void OnMouseDown()  //터치 감지
    {
        TriggerDialogue();
    }
}
