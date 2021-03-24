using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBreath : MonoBehaviour
{
    public float cooldownTime = 3f;
    public AudioClip sfxClip;

    private void Update()
    {
        if(Mathf.Floor(cooldownTime) <= 0)
        {
            //0일때마다 불 발사.
            FireFireBreath();
            cooldownTime = 5f;
        }
        else
        {
            cooldownTime -= Time.deltaTime;
            //Debug.Log(Mathf.Floor(cooldownTime));
        }
    }

    public void FireFireBreath()
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(sfxClip);
        gameObject.GetComponent<Animator>().SetTrigger("StartFire");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("hit");
            if (collision.gameObject.transform.position.x<=gameObject.transform.position.x)
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
