using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkHole : MonoBehaviour
{
    public AudioClip sfxClip;

    private void OnTriggerExit2D(Collider2D collision)
    {
        //싱크홀 발동!!
        gameObject.GetComponent<AudioSource>().PlayOneShot(sfxClip);
        gameObject.GetComponent<Animator>().SetBool("sinkHoleOn", true);

    }
}
