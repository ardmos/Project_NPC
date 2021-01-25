using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInput_Controller : MonoBehaviour
{
    //키인풋 컨트롤 만들기! 아무 오브젝트에나 이거 붙이면 방향키 입력 받아서 움직임! 
    //타일맵의 셀 사이즈를 1로 했기 때문에,  이동도 1만큼 시키면 딱 딱 맞음.

    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {        
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //print("LL");
            gameObject.transform.Translate(Vector2.left);
        }else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //print("RR");
            gameObject.transform.Translate(Vector2.right);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //print("UU");
            gameObject.transform.Translate(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //print("DD");
            gameObject.transform.Translate(Vector2.down);
        };
    }



    /// <summary>
    /// 아, 지금 이동을 쑥 쑥 해버려서, 충돌체크 하기도 전에 쑥 통과해버리는거지!!!  
    /// 이동방식 변경 ㄱㄱ !
    /// </summary>
 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print(collision.gameObject.name);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.gameObject.name);
    }
}



