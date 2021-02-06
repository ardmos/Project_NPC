using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewWildBoar : MonoBehaviour
{
    public List<GameObject> bushes;
    public bool go;
    public int n;
    public Vector2 target;

    //쓰다듬기 상황  <-- 이걸 트루로 해주면 된다. 
    public bool isStrokeMode;
    public int strokeCount;

    void Update()
    {
        if (go)
        {
            //이동
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, target, 0.05f);                        

            if ((Vector2)gameObject.transform.position == target)
            {
                go = false;
                Destroy(gameObject);
            }
            else
            {
                //진행 방향에 따른 이미지 방향 전환
                if (target.x < gameObject.transform.position.x)
                    //그대로
                    gameObject.GetComponent<SpriteRenderer>().flipX = false;
                else
                    //뒤집
                    gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }

    //목적지를 가지고 이동. 많은 부쉬들 중 선택.  
    //지금 부쉬가 선택되는 케이스도 그대로 둠 -> 결과적으로 젠 타이밍의 랜덤성 올라감
    public void DecideWhereToGo()
    {
        foreach (GameObject bushObj in GameObject.FindGameObjectsWithTag("Bush"))
        {
            bushes.Add(bushObj);
        }
        n = Random.Range(0, bushes.Count - 1);
        target = bushes[n].transform.position;

        go = true;        
    }

    private void OnMouseDown()
    {
        //모드별 피격 이미지.
        if (isStrokeMode)
        {
            //쓰다듬기모드
            StopAllCoroutines();
            StartCoroutine(SaveOneSec_Stroked());
        }
        else
        {
            //잡기모드
            StartCoroutine(SaveOneSec_Caught());
        }
    }


    //피격애니메이션 유지용
    IEnumerator SaveOneSec_Stroked()
    {
        gameObject.GetComponent<Animator>().SetTrigger("isStroked");
        strokeCount++;
        
        if (strokeCount >= 3)
        {
            go = false;
            FindObjectOfType<CatchingWildBoar.GameManager>().catchCount++;
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }
    }
    IEnumerator SaveOneSec_Caught()
    {
        gameObject.GetComponent<Animator>().SetTrigger("isCaught");                
        go = false;
        FindObjectOfType<CatchingWildBoar.GameManager>().catchCount++;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
