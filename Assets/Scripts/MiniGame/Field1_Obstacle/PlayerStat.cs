using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    //HP
    public float hP;
    //하트비트 타입 BeatType
    public Dialogue.DialogueSet.Details.BeatBeat.BeatType beatType;
    //하트비트 스피드
    public float beatSpeed;
    //하트비터
    public HeartBeater heartBeater;
    //획득 열쇠 
    public bool isGotkey1, isGotkey2;

    private void Awake()
    {
        beatType = Dialogue.DialogueSet.Details.BeatBeat.BeatType.beatType2;
        beatSpeed = 0.2f;
    }

    private void Update()
    {
        //HP 10 이상
        if (hP >= 10)
        {
            beatType = Dialogue.DialogueSet.Details.BeatBeat.BeatType.beatType2;
        }
        //HP 8 이상
        else if(hP>=8)
        {
            beatType = Dialogue.DialogueSet.Details.BeatBeat.BeatType.beatType3;
        }
        //HP 6 이상
        else if (hP>=6)
        {

            beatType = Dialogue.DialogueSet.Details.BeatBeat.BeatType.beatType4;
            beatSpeed = 0.2f;
        }
        //HP 4 이상
        else if (hP>=4)
        {
            beatType = Dialogue.DialogueSet.Details.BeatBeat.BeatType.beatType5;
            beatSpeed = 0.2f;
        }
        //HP 2 이상
        else if(hP>=2)
        {
            beatType = Dialogue.DialogueSet.Details.BeatBeat.BeatType.beatType1;
            beatSpeed = 0.2f;
        }
        //HP 바닥.
        else
        {
            //삐이이이
            beatSpeed = 0.2f;
            beatType = Dialogue.DialogueSet.Details.BeatBeat.BeatType.beatType0;
        }

        heartBeater.beatSpeed = beatSpeed;
        heartBeater.beatType = beatType;
    }
}
