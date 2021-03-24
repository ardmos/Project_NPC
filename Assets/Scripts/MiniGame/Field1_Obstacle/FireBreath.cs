using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBreath : MonoBehaviour
{
    public bool startDelay;
    public float startDelayTime, cooldownTime, tmpCoolDownTime;
    public AudioClip sfxClip;

    private void Start()
    {
        tmpCoolDownTime = cooldownTime;
        cooldownTime += startDelayTime;
    }

    private void Update()
    {
        //딜레이 주고싶은 경우 맨 처음 한 번 만!
        if (startDelay)
        {
            if (Mathf.Floor(cooldownTime) <= 0)
            {
                //0일때마다 불 발사.
                FireFireBreath();
                cooldownTime = tmpCoolDownTime;
                startDelay = false;
            }
            else
            {
                cooldownTime -= Time.deltaTime;
                //Debug.Log(Mathf.Floor(cooldownTime));
            }
        }
        else
        {
            if (Mathf.Floor(cooldownTime) <= 0)
            {
                //0일때마다 불 발사.
                FireFireBreath();
                cooldownTime = tmpCoolDownTime;
            }
            else
            {
                cooldownTime -= Time.deltaTime;
                //Debug.Log(Mathf.Floor(cooldownTime));
            }
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
