using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusicAudio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartMusic());
    }
    
    IEnumerator StartMusic()
    {
        yield return new WaitForSeconds(1.2f);
        gameObject.GetComponent<AudioSource>().Play();
    }
}
