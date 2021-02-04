using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewWildBoar : MonoBehaviour
{
    public List<GameObject> bushes;
    public bool go;
    public int n;
    public Vector2 target;



    // Update is called once per frame
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
        print("s");
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }
    private void OnMouseUp()
    {
        FindObjectOfType<CatchingWildBoar.GameManager>().catchCount++;
        Destroy(gameObject);
    }
}
