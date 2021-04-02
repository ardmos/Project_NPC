using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public PlayerStat playerStat;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //플레이어면, 아이템 획득 여부 확인 후 처리 
        if (collision.CompareTag("Player"))
        {
            if(playerStat.isGotkey1 && playerStat.isGotkey2)
            {
                print("클리어!");
            }
            else
            {
                print("모든 키를 모으세요! ");
            }
        }
    }
}
