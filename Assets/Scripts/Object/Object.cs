using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public int id;
    public Dictionary<int, bool> isInteracted;    

    //주워지거나 하는 상호작용기능이 있는 오브젝트 ex동전. 
    //같은 경우에는 추가적으로 스크립트 만들어서 기능 붙여주자. 
    //마우스오버시 마우스 커서 이미지 변경. 돋보기로 

    public void TriggerDialogue()
    {
        if (!DialogueManager.Instance.isDialogueActive && IsThePlayerNear())
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

    private bool IsThePlayerNear()
    {
        Vector3 playerPos = FindObjectOfType<KeyInput_Controller>().gameObject.transform.position;
        if (Mathf.Abs(playerPos.x-transform.position.x)<=2.5f && Mathf.Abs(playerPos.y-transform.position.y)<=2.5f)
        {
            Debug.Log("Player is near");
            return true;
        }
        else
        {
            Debug.Log("Player isn't near. x:" + Mathf.Abs(playerPos.x - transform.position.x) + ", y:" + Mathf.Abs(playerPos.y - transform.position.y));
            return false;
        }
    }

    private void OnMouseDown()  //터치 감지
    {
        TriggerDialogue();
    }

    private void OnMouseOver()  //마우스오버시 커서 이미지 돋보기 이미지로 변경. 
    {
        if(IsThePlayerNear()) GameManager.Instance.SetGameCursor("MagGlass");

    }

    private void OnMouseExit()  //마우스 벗어나면 다시 원래 커서 이미지로 변경. 
    {
        GameManager.Instance.SetGameCursor("Default");
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
