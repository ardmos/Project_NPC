using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorField : MonoBehaviour
{
    //접근 감지 스크립트


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //영역 내 접근 감지
        print(collision.gameObject.name + "here");
    }
}
