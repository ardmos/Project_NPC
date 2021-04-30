using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkBalloonManager : MonoBehaviour
{
    //각지에 흩어져있는 말풍선들을 등록시켜두고
    //필요한 경우 연결시켜주는 매니저

    //각지에 흩어져있는 말풍선들(컨트롤러) 모아뒀다 사용
    public TalkBalloonController desk;

    #region 말풍선들 ID 테이블. 이것을 기준으로 말풍선들 여기서 연결시켜준다.
    public void ConnectTB(int id, Queue<Dialogue.DialogueSet> dialogueSetsQue)
    {
        switch (id)
        {
            case 800000:
                //씬1 데스크
                desk.StartTalkBalloon(id, dialogueSetsQue);
                break;
            default:
                Debug.Log("해당 id넘버로 준비된 말풍선 오브젝트가 없습니다. id넘버를 확인해주십시오. ");
                break;
        }
    }
    #endregion

}
