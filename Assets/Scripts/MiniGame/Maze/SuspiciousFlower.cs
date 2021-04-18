using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspiciousFlower : MonoBehaviour
{
    //수상한 꽃 획득
    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindObjectOfType<Timer>().MinusTime(10);
        Destroy(gameObject);
    }
}
