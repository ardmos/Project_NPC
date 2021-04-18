using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShooter_TrapVer : MonoBehaviour
{

    [SerializeField]
    GameObject fireBall1Pref, fireBall2Pref;
    [SerializeField]
    bool isFireBall1;
    [SerializeField]
    float cooldownTime, tmpCoolDownTime;

    [SerializeField]
    bool shootTrigger;

    private void Start()
    {
        tmpCoolDownTime = cooldownTime;
        shootTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (shootTrigger)
        {
            if (Mathf.Floor(cooldownTime) <= 0)
            {
                //0일때마다 불 발사.
                FireFireBall();
                cooldownTime = tmpCoolDownTime;
            }
            else
            {
                cooldownTime -= Time.deltaTime;
                //Debug.Log(Mathf.Floor(cooldownTime));
            }
        }
    }

    void FireFireBall()
    {
        GameObject prefObj;
        if (isFireBall1)
        {
            prefObj = Instantiate(fireBall1Pref);
            prefObj.transform.SetParent(transform);
            prefObj.transform.position = Vector2.zero;
        }
        else
        {
            prefObj = Instantiate(fireBall2Pref);
            prefObj.transform.SetParent(transform);
            prefObj.transform.position = Vector2.zero;
        }
    }

    public void ActivateFireShooterTrap()
    {
        if(shootTrigger != true)
        {
            shootTrigger = true;
            FireFireBall();
        }
    }

    public void DeActivateFireShooterTrap()
    {
        if (shootTrigger == true)
        {
            shootTrigger = false;
        }
    }
}
