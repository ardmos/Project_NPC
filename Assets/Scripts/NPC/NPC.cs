using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public float movespeed = 5f;
    public Rigidbody2D rb;
    public bool move;
    public Vector2 pos;

    private void Start()
    {
        pos = rb.position+Vector2.left;
    }

    private void FixedUpdate()
    {
        if (rb.position == pos)
        {
            move = false;
        }else
        {
            if (move)
            {
                print("tri");
                Vector2 newPos = Vector2.MoveTowards(rb.position, pos, movespeed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);
            }
        }


    }

    public void Move_Event()
    {
        //엔피씨 이동 이벤트
        move = true;
    }
}
