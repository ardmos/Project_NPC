using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MiniGameManager minigameManager = FindObjectOfType<MiniGameManager>();
            if(gameObject.name.Contains("1"))
            {
                minigameManager.IGotKey1();
            }
            else
            {
                minigameManager.IGotKey2();
            }
            //클리어!!
            Destroy(gameObject);
        }
    }
}
