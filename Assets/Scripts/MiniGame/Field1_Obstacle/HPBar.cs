using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{       
    // Update is called once per frame
    void Update()
    {
        if(PlayerStat.instance.hP <= 0)
        {
            //죽음~!  실패 ! 
        }
        //매 프레임마다, PlayerStat에서 HP 양 읽어와서 적용. 
        gameObject.GetComponentInChildren<Slider>().value = PlayerStat.instance.hP;
    }
}
