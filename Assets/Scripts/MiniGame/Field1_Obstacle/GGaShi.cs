﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GGaShi : MonoBehaviour
{
    bool isPerfactThorn;
    [SerializeField]
    AudioClip[] sfxClip;

    public void SetPerfactThorn()
    {
        isPerfactThorn = true;
        //효과음
        gameObject.GetComponent<AudioSource>().volume = 0.1f;
        gameObject.GetComponent<AudioSource>().PlayOneShot(sfxClip[Random.Range(0, 2)]);
    }
    public void SetNotPerfactThorn()
    {
        isPerfactThorn = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //충돌처리
        if (isPerfactThorn)
        {
            if (collision.CompareTag("Player"))
            {
                Debug.Log("hit");
                if (collision.gameObject.transform.position.x <= gameObject.transform.position.x)
                {
                    //브레스보다 좌측에서 맞았을 떄
                    //collision.gameObject.GetComponent<Rigidbody2D>().MovePosition(Vector2.left*10f);
                    //print(gameObject.transform.position);
                    Vector3 desPos = new Vector3(collision.gameObject.transform.position.x - 2f, collision.gameObject.transform.position.y, collision.gameObject.transform.position.z);
                    collision.gameObject.GetComponent<KeyInput_Controller>().GetHit(desPos);
                }
                else
                {
                    //우측에서 맞았을 때
                    //collision.gameObject.GetComponent<Rigidbody2D>().MovePosition(Vector2.right*10f);
                    //print(gameObject.transform.position + ".!");
                    Vector3 desPos = new Vector3(collision.gameObject.transform.position.x + 2f, collision.gameObject.transform.position.y, collision.gameObject.transform.position.z);
                    collision.gameObject.GetComponent<KeyInput_Controller>().GetHit(desPos);
                }

            }
        }
    }
}
