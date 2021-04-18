using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShooterSensor : MonoBehaviour
{

    public FireShooter_TrapVer fireShooter_TrapVer;

    //함정카드 발동!!
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("함정카드 발동!!");
        fireShooter_TrapVer.ActivateFireShooterTrap();
    }
}
