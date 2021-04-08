using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkHole : MonoBehaviour
{
    public AudioClip sfxClip;

    private void OnTriggerExit2D(Collider2D collision)
    {
        //싱크홀 발동!!
        if (gameObject.GetComponent<Animator>().GetBool("sinkHoleOn") == false)
        {
            gameObject.GetComponent<AudioSource>().PlayOneShot(sfxClip);
            gameObject.GetComponent<Animator>().SetBool("sinkHoleOn", true);
        }
        else Debug.Log("방해물_싱크홀이 이미 발동되었습니다");
    }
}
