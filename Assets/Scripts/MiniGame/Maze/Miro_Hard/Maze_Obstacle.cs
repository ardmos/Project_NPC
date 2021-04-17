using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze_Obstacle : MonoBehaviour
{
    //상호작용 게이지 발동 처리.

    //각각 장애물별 문구 입력
    public string[] 작업문구;

    SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    //작업 시작 처리
    public void StartTask()
    {
        FindObjectOfType<InterActiveBar>().StartInterActiveBar(작업문구[Random.Range(0,작업문구.Length)], gameObject);
    }

    //작업 완료 처리 
    public void CompleteTask()
    {
        //gameObject.SetActive(false)
        //스르륵 사라지게
        StartCoroutine(StartDisappear());


    }

    IEnumerator StartDisappear()
    {
        if(spriteRenderer.color.a != 0f)
        {
            yield return new WaitForSeconds(0.01f);
            spriteRenderer.color = new Color(1f,1f,1f,spriteRenderer.color.a-0.01f);
        }
        else
        {
            //완전 완료!

            //만약, 오브젝트가 문1이나 문2라면!  가시나 힐팩을 활성화시켜줘야한다.
            if (name == "문1")
            {
                FindObjectOfType<Miro_Hard_Manager>().Active가시();
            }
            else if (name == "문2")
            {
                FindObjectOfType<Miro_Hard_Manager>().Active힐팩();
            }
            gameObject.SetActive(false);
        }
    }
}
