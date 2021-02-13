using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public int id;
    public Dictionary<int, bool> isInteracted;    

    //주워지거나 하는 기능이 있는 오브젝트 ex동전. 
    //같은 경우에는 추가적으로 스크립트 만들어서 기능 붙여주자. 

    public void TriggerDialogue()
    {
        if (!DialogueManager.Instance.isDialogueActive)
        {
            //대화시 작동되는 부분.

            //혹시 상호작용 기반 이벤트가 발동될 조건인가 확인해보고
            if (InteractedEventController())
            {
                return;
            }            

            //다이얼로그를 열고
            DialogueManager.Instance.StartDialogue(id);
            //게임매니져에 보고를 한다.
            GameManager.Instance.DidInteracted(id);
        }
            
    }

    private void OnMouseDown()  //터치 감지
    {
        TriggerDialogue();
    }

    public bool InteractedEventController()
    {
        //씬1 기타 오브젝트들 상호작용 완료 후 캡슐 선택시.
        if (id == 15300)
        {
            //캡슐일 때, 나머지 오브젝트들 상호작용 여부 체크.
            isInteracted = GameManager.Instance.isInteracted;
            if (isInteracted[15000] && isInteracted[15100] && isInteracted[15200])
            {
                //나머지와 모두 대화를 마쳤다.                
                //씬1 천형사 등장 이벤트 발동. 스토리 이벤트 넘버 = 1
                GameManager.Instance.storyNumber = 1;
                GameManager.Instance.StartStoryEvent();
                return true;
            }
        }

        return false;
    }
}
