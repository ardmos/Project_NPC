using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_LayerSetter : MonoBehaviour
{
    // 현 obj랑 KeyInput_Controller.cs를 갖고있는 플레이어의 y값 기준으로 자동 감지해서
    // '공통으로 현 오브젝트의 레이어오더는 잠시 50으로 변경한다.
    // 플레이어가 위에 있으면 플레이어의 레이어를 현 오브젝트 레이어보다 하나 내리고,
    // 플레이어가 아래에 있으면 플레이어의 레이어를 현 오브젝트 레이어보다 하나 올린다.

    //isTrigger가 체크된 레이어 세팅용 박스콜라이더가 달린 오브제트를 자식으 하나 더 추가해서, 
    //isTrigger로 체크 받아서 계산.

    //NPC obj, 플레이어 obj
    public GameObject thisNPCObj, playerObj;
    //NPC SpriteRenderer 
    SpriteRenderer thisNPCSpriteRenderer;
    //플레이어 SpriteRenderer.   플레이어의 그림자까지 처리를 위한 List.
    List<SpriteRenderer> playerSpriteRenderers;

    //기존 레이어오더 값
    List<int> defaultLayerOrders;
    //올라간 레이어오더 값
    List<int> newLayerOrders;

    //자식 가구들 SpriteRenderer
    public SpriteRenderer[] childRenderersArr;

    private void Start()
    {
        newLayerOrders = new List<int>();
        defaultLayerOrders = new List<int>();
        thisNPCObj = gameObject;
        thisNPCSpriteRenderer = thisNPCObj.GetComponent<SpriteRenderer>();
        playerSpriteRenderers = new List<SpriteRenderer>();
        playerObj = GameObject.FindGameObjectWithTag("Player");
        foreach (var item in playerObj.GetComponentsInChildren<SpriteRenderer>())
        {
            playerSpriteRenderers.Add(item);
            Debug.Log(item);
        }
        for (int i = 0; i < playerSpriteRenderers.Count; i++)
        {
            if (playerSpriteRenderers[i].gameObject.name == "EmotionSprite") playerSpriteRenderers.RemoveAt(i); //Emotion까지 가구랑 레이어 변환 해버리면 안되니까~! 빼주기
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //새로운 레이어 오더값
        //리스트 초기화 해주고, 
        newLayerOrders.Clear();
        defaultLayerOrders.Clear();
        foreach (SpriteRenderer spriteRenderer in thisNPCObj.GetComponentsInChildren<SpriteRenderer>())
        {
            newLayerOrders.Add(spriteRenderer.sortingOrder + 50);
            defaultLayerOrders.Add(spriteRenderer.sortingOrder);
        }

        childRenderersArr = thisNPCObj.GetComponentsInChildren<SpriteRenderer>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //꼭 플레이어한테만 이걸 적용시킬 필욘 없어서 주석 처리
        //if (!collision.CompareTag("Player")) return;

        //위거나 같으면. 순간 가구 레이어 오더 50만큼 올리고 자식 가구 있으면 같이 올려준다.,  가구 아래로 플레이어가 들어가고
        if (gameObject.transform.position.y <= playerObj.transform.position.y)
        {
            for (int count = 0; count < childRenderersArr.Length; count++)
            {
                childRenderersArr[count].sortingOrder = newLayerOrders[count];
            }

            for (int i = 0; i < playerSpriteRenderers.Count; i++)
            {
                //Debug.Log(playerSpriteRenderers[i] + "Here!");
                playerSpriteRenderers[i].sortingLayerName = "NPC";
                playerSpriteRenderers[i].sortingOrder = thisNPCSpriteRenderer.sortingOrder - (i + 1);
            }
        }
        //아래면 그냥 냅두고. 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //범위 벗어나면 원래 레이어대로 해주기
        for (int count = 0; count < childRenderersArr.Length; count++)
        {
            childRenderersArr[count].sortingOrder = defaultLayerOrders[count];
        }

        for (int i = 0; i < playerSpriteRenderers.Count; i++)
        {
            playerSpriteRenderers[i].sortingLayerName = "Character";
            playerSpriteRenderers[i].sortingOrder = thisNPCSpriteRenderer.sortingOrder + (playerSpriteRenderers.Count - 1 - i);
        }
    }
}
