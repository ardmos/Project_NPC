using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze_Obstacle : MonoBehaviour
{
    //상호작용 게이지 발동 처리.

    //각각 장애물별 문구 입력
    public string[] 작업문구;

    SpriteRenderer spriteRenderer;
    public bool isTaskStarted;
    KeyInput_Controller player;
    Miro_Hard_Manager miro_Hard_Manager;
    InterActiveBar 작업바;
    Vector2 tmpMovement;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        miro_Hard_Manager = FindObjectOfType<Miro_Hard_Manager>();
        작업바 = miro_Hard_Manager.작업바.GetComponent<InterActiveBar>();
        player = FindObjectOfType<KeyInput_Controller>();
    }

    private void Update()
    {
        //작업 시작 이후 플레이어가 이동하면 작업 중지.
        //작업시작했는지
        if (isTaskStarted)
        {            
            tmpMovement = player.movement;
            //플레이어가 이동했는지
            if (Vector2.zero != tmpMovement)
            {
                //Debug.Log("이동했습니다! 진행중이던 작업을 종료합니다! tmpMovement: " + tmpMovement);
                StopTask();
            }
            else
            {
                //Debug.Log("이동하지 않았습니다! tmpMovement: " + tmpMovement);
            }
        }
    }

    //작업 시작 처리
    public void StartTask()
    {
        if (miro_Hard_Manager != null)
        {
            if (miro_Hard_Manager.작업바.activeSelf)
            {
                Debug.Log("상호작용바가 이미 실행중입니다. :" + miro_Hard_Manager.작업바 + "~");
            }
            else
            {
                Debug.Log("상호작용바를 실행합니다. :" + miro_Hard_Manager.작업바 + "~");

                miro_Hard_Manager.작업바.SetActive(true);
                작업바.StartInterActiveBar(작업문구[Random.Range(0, 작업문구.Length)], gameObject);
                
                isTaskStarted = true;

                //Timer 마이너스 5초
                FindObjectOfType<Timer>().MinusTime(5);
            }
        }
        else Debug.Log("상호작용바를 찾을 수 없습니다.");
    }

    public void StopTask()
    {
        if (FindObjectOfType<InterActiveBar>() != null)
        {
            FindObjectOfType<InterActiveBar>().StopInterActiveBar();
        }
        else Debug.Log("종료시킬 상호작용바가 없습니다.");
    }

    //작업 완료 처리 
    public void CompleteTask()
    {
        //gameObject.SetActive(false)
        //스르륵 사라지게
        Debug.Log("작업 완료! 이제 오브젝트 사라지기 시작!");

        foreach (SpriteRenderer spriteRenderer in gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            StartCoroutine(StartDisappear(spriteRenderer));
        }
        
    }

    IEnumerator StartDisappear(SpriteRenderer spriteRenderer)
    {
        Debug.Log("스르륵 사라지기 진행 : " + spriteRenderer.gameObject.name);
        while (spriteRenderer.color.a >= 0f)
        {
            yield return new WaitForSeconds(0.01f);
            
            spriteRenderer.color = new Color(1f, 1f, 1f, spriteRenderer.color.a - 0.03f);
        }       

        //완전 완료!
        Debug.Log("스르륵 사라지기 완전 완료! : " + spriteRenderer.gameObject.name);

        //만약, 오브젝트가 문1이나 문2라면!  가시나 힐팩을 활성화시켜줘야한다.
        if (name == "문1")
        {
            miro_Hard_Manager.Active가시();
        }
        else if (name == "문2")
        {
            miro_Hard_Manager.Active힐팩();
        }

        isTaskStarted = false;
        gameObject.SetActive(false);
    }
}
