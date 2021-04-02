using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public PlayerStat playerStat;
    // Update is called once per frame
    void Update()
    {
        //매 프레임마다, PlayerStat에서 HP 양 읽어와서 적용. 
        gameObject.GetComponentInChildren<Slider>().value = playerStat.hP*0.1f;
    }
}
