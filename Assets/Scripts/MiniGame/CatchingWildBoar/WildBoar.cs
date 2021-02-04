using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildBoar : MonoBehaviour
{
    public Vector2 movement;
    public float movespeed;
    public Rigidbody2D rb;
    public int distance;

    public Vector2 vec2;
    public int moveCount;
    public bool isMoving;
    public int dirNum;

    public int age;

    private void Start()
    {
        movespeed = 5f;
        rb = gameObject.GetComponent<Rigidbody2D>();
        //이동거리
        distance = 150;
    }

    // Update is called once per frame
    void Update()
    {
        //디스턴스만큼씩 이동시키고 생각하고 반복 
        if (isMoving)
        {
            Wandering(vec2.x, vec2.y);
            //INHERE();
            moveCount++;
            if (moveCount >= distance || !INHERE())
            //if (moveCount >= distance)
                isMoving = false;
        }
        else
        {
            //ThinkWhereToMove();
            vec2 = new Vector2(Random.Range(-1f,1f), Random.Range(-1f, 1f));
            isMoving = true;
            moveCount = 0;
        }

        age++;

        //if (age >= 50)
            //gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    private void FixedUpdate()
    {
        //실질적 이동
        rb.MovePosition(rb.position + movement * movespeed * Time.fixedDeltaTime);
    }

    private bool INHERE()   //화면 밖으로 못나가게.
    {
        bool b = true;
        Vector3 pos = Camera.main.WorldToViewportPoint(this.transform.position);
        if (pos.x < 0f)
        {
            pos.x = 0f;
            b = false;
        }
        if (pos.y < 0f)
        {
            pos.y = 0f;
            b = false;
        }
        if (pos.x > 1f)
        {
            pos.x = 1f;
            b = false;
        }
        if (pos.y > 1f)
        {
            pos.y = 1f;
            b = false;
        }

        this.transform.position = Camera.main.ViewportToWorldPoint(pos);
        return b;
    }

    public void Wandering(float x, float y)
    {       
        //방향을 기준으로 벡터 적용.
        movement.x = FindObjectOfType<CatchingWildBoar.GameManager>().isClear ? 0 : x;
        movement.y = FindObjectOfType<CatchingWildBoar.GameManager>().isClear ? 0 : y;

        ///애니메이션 보류
        //animator.SetFloat("Horizontal", movement.x);
        //animator.SetFloat("Vertical", movement.y);
        //animator.SetFloat("Speed", movement.sqrMagnitude);

    }

    private void OnMouseDown()
    {
        FindObjectOfType<CatchingWildBoar.GameManager>().catchCount++;
        Destroy(gameObject);
    }

    private void OnBecameInvisible()    //화면 밖으로 나가면 사라지게.
    {
       // Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        //부쉬에 닿으면
        if (collision.CompareTag("Bush") && age>=50)
        {
            //부쉬 흔들 애니메이션

            //멧돼지 사라짐
            Destroy(gameObject);
        }
    }

}
