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
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, target, 0.05f);

            if ((Vector2)gameObject.transform.position == target)
            {
                go = false;
                Destroy(gameObject);
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
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }
    private void OnMouseUp()
    {
        //만약, 쓰다듬기라면??   클릭 세 번이어야한다! 
        if (isStrokeMode)
        {
            //쓰다듬기 상황일 때.   애니메이션도 여기다 넣으면 된다. 
            strokeCount++;
            if(strokeCount >= 3)
            {
                FindObjectOfType<CatchingWildBoar.GameManager>().catchCount++;
                Destroy(gameObject);
            }
        }
        else
        {
            FindObjectOfType<CatchingWildBoar.GameManager>().catchCount++;
            Destroy(gameObject);
        }
    }
}
