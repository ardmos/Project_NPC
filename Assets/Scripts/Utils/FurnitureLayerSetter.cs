using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureLayerSetter : MonoBehaviour
{
    // 현 obj랑 KeyInput_Controller.cs를 갖고있는 플레이어의 y값 기준으로 자동 감지해서
    // '공통으로 현 오브젝트의 레이어오더는 잠시 50으로 변경한다.
    // 플레이어가 위에 있으면 플레이어의 레이어를 현 오브젝트 레이어보다 하나 내리고,
    // 플레이어가 아래에 있으면 플레이어의 레이어를 현 오브젝트 레이어보다 하나 올린다.

    //isTrigger가 체크된 레이어 세팅용 박스콜라이더가 달린 오브제트를 자식으 하나 더 추가해서, 
    //isTrigger로 체크 받아서 계산.

    //부모 obj, 플레이어 obj
    public GameObject parentsObj, playerObj;
    //부모 obj pos
    Vector2 parentsPos;
    //부모 SpriteRenderer 
    SpriteRenderer parentsRenderer;
    //플레이어 SpriteRenderer.   플레이어의 그림자까지 처리를 위한 List.
    List<SpriteRenderer> playerSpriteRenderers;

    //기존 레이어오더 값
    List<int> defaultLayerOrders;
    //올라간 레이어오더 값
    List<int> newLayerOrders;

    //자식 가구들 SpriteRenderer
    public SpriteRenderer[] childRenderersArr;

    //FurnitureLayerSetterManager 로부터 받은 번호표! 저장하는 변수
    public int numberTicket=0;
    FurnitureLayerSetterManager furnitureLayerSetterManager;


    private void Start()
    {
        furnitureLayerSetterManager = FindObjectOfType<FurnitureLayerSetterManager>();
        newLayerOrders = new List<int>();
        defaultLayerOrders = new List<int>();
        parentsObj = gameObject.transform.parent.gameObject;
        parentsPos = parentsObj.transform.position;
        parentsRenderer = parentsObj.GetComponent<SpriteRenderer>();
        playerSpriteRenderers = new List<SpriteRenderer>();
        playerObj = GameObject.FindGameObjectWithTag("Player");
        foreach (var item in playerObj.GetComponentsInChildren<SpriteRenderer>())
        {
            playerSpriteRenderers.Add(item);
        }
        for (int i = 0; i < playerSpriteRenderers.Count; i++)
        {
            if (playerSpriteRenderers[i].gameObject.name == "EmotionSprite") playerSpriteRenderers.RemoveAt(i); //Emotion까지 가구랑 레이어 변환 해버리면 안되니까~! 빼주기
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        //새로운 레이어 오더값
        //리스트 초기화 해주고, 
        newLayerOrders.Clear();
        defaultLayerOrders.Clear();
        foreach (SpriteRenderer spriteRenderer in parentsObj.GetComponentsInChildren<SpriteRenderer>())
        {
            newLayerOrders.Add(spriteRenderer.sortingOrder + 50);
            defaultLayerOrders.Add(spriteRenderer.sortingOrder);
        }

        childRenderersArr = parentsObj.GetComponentsInChildren<SpriteRenderer>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        //위거나 같으면. 순간 가구 레이어 오더 50만큼 올리고 자식 가구 있으면 같이 올려준다.,  가구 아래로 플레이어가 들어가고
        if (parentsPos.y <= playerObj.transform.position.y)
        {
            for (int count = 0; count < childRenderersArr.Length; count++)
            {
                childRenderersArr[count].sortingOrder = newLayerOrders[count];
            }

            for (int i = 0; i < playerSpriteRenderers.Count; i++)
            {
                playerSpriteRenderers[i].sortingLayerName = "Furniture";
                playerSpriteRenderers[i].sortingOrder = parentsRenderer.sortingOrder - (i+1);
            }

            if (numberTicket == 0) numberTicket = furnitureLayerSetterManager.AddNoteOnBoard();
            else furnitureLayerSetterManager.MakeTrueOnBoard(numberTicket);
        }
        //아래면 그냥 냅두고. 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (numberTicket == 0) return;

        furnitureLayerSetterManager.MakeFalseOnBoard(numberTicket);
        if (furnitureLayerSetterManager.IsThereTrueValueOnBoard())
        {
            //있으면 내 레이어만 원래대로
            Debug.Log("thre is true value");
        }
        else
        {
            Debug.Log("thre is no true value");
            //없으면 다 원래대로
            for (int i = 0; i < playerSpriteRenderers.Count; i++)
            {
                playerSpriteRenderers[i].sortingLayerName = "Character";
                playerSpriteRenderers[i].sortingOrder = parentsRenderer.sortingOrder + (playerSpriteRenderers.Count - 1 - i);
            }
        }

        //범위 벗어나면 원래 레이어대로 해주기
        //그 전에, 
        for (int count = 0; count < childRenderersArr.Length; count++)
        {   
            childRenderersArr[count].sortingOrder = defaultLayerOrders[count];
        }


    }
}
