using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Dialogue : MonoBehaviour
{
    int talkCount = 0;
    private int storyPackCount = 1;  //스토리팩의 갯수. 내가 일일히 바꿔줘야함. 기본은 1
    public Dialogue[] storyPack;    //storyPack1, storyPack2, storyPack3,

    public void SetPackCount(int n)
    {
        //각 오브젝트의 스크립트에서 이걸 설정해주는 부분 추가. 
        storyPackCount = n;
    }

    public void TriggerDialogue()
    {
        //하나인 경우. 
        DialogueManager.instance.StartDialogue(storyPack, "Stable");
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
