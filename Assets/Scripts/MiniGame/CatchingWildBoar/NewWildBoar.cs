using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewWildBoar : MonoBehaviour
{
    public float speed;
    public List<GameObject> bushes;
    public bool go, hasCaught;
    public int n;
    public Vector2 target;

    //쓰다듬기 상황  <-- 이걸 트루로 해주면 된다. 
    public bool isStrokeMode;
    public int strokeCount;

    //피격 이펙트
    ParticleSystem[] particleSystems;

    void Update()
    {
        if (go)
        {
            //이동
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, target, speed);                        

            if ((Vector2)gameObject.transform.position == target)
            {
                go = false;
                Destroy(gameObject);
            }
            else
            {
                //진행 방향에 따른 이미지 방향 전환
                if (target.x < gameObject.transform.position.x)
                {
                    //그대로
                    gameObject.GetComponent<SpriteRenderer>().flipX = false;
                    particleSystems[0].gameObject.transform.localPosition = new Vector3(-0.45f, 0.23f, 0);
                }
                else
                {
                    //뒤집
                    gameObject.GetComponent<SpriteRenderer>().flipX = true;
                    particleSystems[0].gameObject.transform.localPosition = new Vector3(0.45f, 0.23f, 0);
                }

            }
        }
    }

    //목적지를 가지고 이동. 많은 부쉬들 중 선택.  
    //지금 부쉬가 선택되는 케이스도 그대로 둠 -> 결과적으로 젠 타이밍의 랜덤성 올라감   <--- 은 그냥 해결했음.
    public void DecideWhereToGo()
    {
        particleSystems = gameObject.GetComponentsInChildren<ParticleSystem>();

        foreach (GameObject bushObj in GameObject.FindGameObjectsWithTag("Bush"))
        {
            bushes.Add(bushObj);
        }       
        n = Random.Range(0, bushes.Count);
        if (bushes[n].transform.position == transform.position)
        {            
            DecideWhereToGo();
        }
        target = bushes[n].transform.position;

        go = true;        
    }

    private void OnMouseDown()
    {
        //동물별 피격 효과.지금 여기서부터 해야함.  셋트리거 없는애들떄문에.  일단 다르게 처리를 해줘야겠다. 
        //그리고 동물별 잡았을 때 결과 다르게. 
        //때렸을 때 쑥 팡 살짝 차이나게. 


        if (hasCaught)
        {
            return;
        }

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

        //쓰다듬 피격 이펙트 
        particleSystems[0].Play();

        if (strokeCount >= 3)
        {
            hasCaught = true;
            go = false;
            FindObjectOfType<CatchingWildBoar.GameManager>().catchCount++;
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }
    }
    IEnumerator SaveOneSec_Caught()
    {
        hasCaught = true;
        particleSystems[1].Play();
        particleSystems[2].Play();
        gameObject.GetComponent<Animator>().SetTrigger("isCaught");                
        go = false;
        FindObjectOfType<CatchingWildBoar.GameManager>().catchCount++;
        yield return new WaitForSeconds(1f);
        
        Destroy(gameObject);
    }
}
