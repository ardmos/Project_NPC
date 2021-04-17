using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPack : MonoBehaviour
{
    //힐팩 획득
    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindObjectOfType<Timer>().PlusTime(20);
        Destroy(gameObject);
    }
}
