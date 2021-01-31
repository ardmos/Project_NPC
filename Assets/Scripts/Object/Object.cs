using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public int id;

    //주워지거나 하는 기능이 있는 오브젝트 ex동전. 
    //같은 경우에는 추가적으로 스크립트 만들어서 기능 붙여주자. 

    public void TriggerDialogue()
    {
        if (!DialogueManager.instance.isDialogueActive)
            DialogueManager.instance.StartDialogue(id);
    }

    private void OnMouseDown()  //터치 감지
    {
        TriggerDialogue();
    }
}
