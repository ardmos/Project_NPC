using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Dialogue : MonoBehaviour
{
    public int talkCount = 0;
    public int storyPackCount = 1;
    public Dialogue[] storyPack;    //storyPack1, storyPack2, storyPack3,

    public void TriggerDialogue()
    {
        //하나인 경우. 
        DialogueManager.instance.StartDialogue(storyPack);
        //여럿인 경우는 talkCount에 따라서 다른 스토리팩을 보내줘야 함. 
    } 

    private void OnMouseDown()  //터치 감지
    {
        TriggerDialogue();
        if ( storyPackCount > talkCount )
        {
            talkCount++;
        }
    }

}
