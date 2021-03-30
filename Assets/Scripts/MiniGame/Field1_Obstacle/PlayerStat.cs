using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    //HP
    public float hP;
    //하트비트 타입 BeatType
    public Dialogue.DialogueSet.Details.BeatBeat.BeatType beatType;
    //획득 열쇠 
    public bool[] isGotkey;


    public static PlayerStat instance;

    private void Awake()
    {
        if(instance == null) instance = new PlayerStat();
    }

}
