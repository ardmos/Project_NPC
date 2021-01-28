using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorField : MonoBehaviour
{
    public GameObject police1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //영역 내 접근 감지
        print(collision.gameObject.name + "here");

        //경찰 호출 
        police1.GetComponent<NPC_SamplePolice>().Move_Event();
    }
}
