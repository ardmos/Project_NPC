using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewWildBoar : MonoBehaviour
{
    public AudioClip audioClip_PointCounterUp, audioClip_PointCounterSuperUp, audioClip_PointCounterDown;
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

    public bool goldenWildBoar, wildBoar, squirrel;

    private void Start()
    {
        bushes = new List<GameObject>();
    }

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
        //그리고 동물별 잡았을 때 결과 다르게. 

        if (hasCaught)
        {
            return;
        }

        //모드별 피격 효과.
        if (isStrokeMode)
        {
            //쓰다듬기모드
            GameObject prefObj = Instantiate(Resources.Load("Prefabs/MiniGame/CatchingWildBoar/Feather") as GameObject);
            //Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //prefObj.transform.position = new Vector3(pos.x, pos.y, 0f);
            prefObj.transform.SetParent(gameObject.transform);
            prefObj.transform.localPosition = new Vector3(-0.55f, -0.55f);
            StopAllCoroutines();
            StartCoroutine(SaveOneSec_Stroked());
        }
        else
        {
            //잡기모드
            GameObject prefObj = Instantiate(Resources.Load("Prefabs/MiniGame/CatchingWildBoar/Sword") as GameObject);
            //Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //prefObj.transform.position = new Vector3(pos.x, pos.y, 0f);
            prefObj.transform.SetParent(gameObject.transform);
            prefObj.transform.localPosition = new Vector3(0.6f, -0.2f);
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

        if (strokeCount >= 2)
        {
            hasCaught = true;
            go = false;
            if (wildBoar)
            {
                FindObjectOfType<CatchingWildBoar.GameManager>().catchCount++;
                gameObject.GetComponent<AudioSource>().PlayOneShot(audioClip_PointCounterUp);
            }
            else if (goldenWildBoar) 
            {
                FindObjectOfType<CatchingWildBoar.GameManager>().catchCount += 5;
                gameObject.GetComponent<AudioSource>().PlayOneShot(audioClip_PointCounterSuperUp);
            }

            else if (squirrel)
            {
                FindObjectOfType<CatchingWildBoar.GameManager>().catchCount++;
                gameObject.GetComponent<AudioSource>().PlayOneShot(audioClip_PointCounterUp);
            }
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }
    }
    IEnumerator SaveOneSec_Caught()
    { 
        particleSystems[1].Play();
        gameObject.GetComponent<Animator>().SetTrigger("isCaught");

        hasCaught = true;
        go = false;
        if (wildBoar) FindObjectOfType<CatchingWildBoar.GameManager>().catchCount++;
        else if (goldenWildBoar) FindObjectOfType<CatchingWildBoar.GameManager>().catchCount += 5;
        else if (squirrel) FindObjectOfType<CatchingWildBoar.GameManager>().catchCount--;
        yield return new WaitForSeconds(0.3f);
        particleSystems[2].Play();
        if (wildBoar) gameObject.GetComponent<AudioSource>().PlayOneShot(audioClip_PointCounterUp);
        else if (goldenWildBoar) gameObject.GetComponent<AudioSource>().PlayOneShot(audioClip_PointCounterSuperUp);
        else if (squirrel) gameObject.GetComponent<AudioSource>().PlayOneShot(audioClip_PointCounterDown);
        yield return new WaitForSeconds(0.7f);     
        Destroy(gameObject);
    }
}
