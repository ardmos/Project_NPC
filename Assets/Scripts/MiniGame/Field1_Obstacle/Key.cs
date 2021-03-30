using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField]
    AudioClip pickupsfx;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //습득처리
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

            //습득 이펙트
            foreach (ParticleSystem particleSystem in gameObject.GetComponentsInChildren<ParticleSystem>())
            {
                //print(particleSystem);
                particleSystem.Play();
            }

            //습득 효과음
            gameObject.GetComponent<AudioSource>().volume = 0.3f;
            gameObject.GetComponent<AudioSource>().PlayOneShot(pickupsfx);
            //클리어!!
            //Destroy(gameObject);
            //콜라이더 무효화
            gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            //디스트로이 대신 열쇠 이미지 투명화. 
            SpriteRenderer[] spriteRenderers =  gameObject.GetComponentsInChildren<SpriteRenderer>();
            spriteRenderers[0].color = new Color(spriteRenderers[0].color.r, spriteRenderers[0].color.g, spriteRenderers[0].color.b, 0f);
            //그림자는 스프라이트 끄기
            spriteRenderers[1].enabled = false;
        }
    }
}
