using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TalkBalloonDataContainer 
{
    //스몰타이틀
    //id
    public string smallTitle_;
    public int storyId;

    //말풍선에 있어야할것
    //문장
    //오른쪽인지 왼쪽인지
    //글자색깔
    //글자크기
    //글자폰트

    [Serializable]
    public class TalkBalloonData
    {
        public string sentence;

        public enum OffSet
        {
            Left,
            Right
        }
        public OffSet offSet;

        public Color fontColor;
        public int fontSize;
        public Font font;
    }

    public TalkBalloonData[] talkBalloonDatas;
}
