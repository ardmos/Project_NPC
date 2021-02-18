using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjLayerSetter : MonoBehaviour
{
    // 현 obj랑 KeyInput_Controller.cs를 갖고있는 플레이어의 y값 기준으로 자동 감지해서
    // 플레이어가 위에 있으면 플레이어의 레이어를 현 오브젝트 레이어보다 하나 내리고,
    // 플레이어가 아래에 있으면 플레이어의 레이어를 현 오브젝트 레이어보다 하나 올린다.

    //isTrigger가 체크된 레이어 세팅용 박스콜라이더가 달린 오브제트를 자식으 하나 더 추가해서, 
    //isTrigger로 체크 받아서 계산.

    //부모 obj
    GameObject parentsObj;

    private void Start()
    {
        parentsObj = gameObject.transform.parent.gameObject;
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {

        
    }
}
