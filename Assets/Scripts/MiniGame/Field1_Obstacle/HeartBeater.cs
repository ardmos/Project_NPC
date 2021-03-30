using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeater : MonoBehaviour
{
    //비트arr들. 비트 타입별로
    [SerializeField]
    float[] beatType0, beatType1, beatType2, beatType3, beatType4, beatType5;
    //UI Line Renderer의 List<Vector2> points를 담은
    [SerializeField]
    List<Vector2> points;

    //비트 타입 (외부 설정용)
    public Dialogue.DialogueSet.Details.BeatBeat.BeatType beatType;

    void Start()
    {
        //UI Line Renderer에서 포인트들 배열 받아오자. Points []
        points = gameObject.GetComponent<UILineRenderer>().points;

        MakePointsFlow(beatType1);
    }

    //배치 부드럽게 돌리는 부분
    private void MakePointsFlow(float[] 비트arr)
    {
        StartCoroutine(Flow(비트arr));
        
    }

    IEnumerator Flow(float[] 비트arr)
    {
        //21(끝)부터 ~ 0-비트arr.Length  까지. 
        print("시작!");
        for (int idx = 21; idx >= -비트arr.Length; idx--)
        {
            SetPoints(idx, 비트arr);
            gameObject.GetComponent<UILineRenderer>().SetAllDirty();
            yield return new WaitForSeconds(0.2f);
        }
        print("끝!");

        //시작하는걸 바꿔줄 수 있는 기회. 여기서 처리 필요
        switch (beatType)
        {
            case Dialogue.DialogueSet.Details.BeatBeat.BeatType.beatType0:
                MakePointsFlow(beatType0);
                break;
            case Dialogue.DialogueSet.Details.BeatBeat.BeatType.beatType1:
                MakePointsFlow(beatType1);
                break;
            case Dialogue.DialogueSet.Details.BeatBeat.BeatType.beatType2:
                MakePointsFlow(beatType2);
                break;
            case Dialogue.DialogueSet.Details.BeatBeat.BeatType.beatType3:
                MakePointsFlow(beatType3);
                break;
            case Dialogue.DialogueSet.Details.BeatBeat.BeatType.beatType4:
                MakePointsFlow(beatType4);
                break;
            case Dialogue.DialogueSet.Details.BeatBeat.BeatType.beatType5:
                MakePointsFlow(beatType5);
                break;
            default:
                print("deafault_at Flow()");
                MakePointsFlow(beatType0);
                break;
        }

    }

    //실제 Point 배치 처리 부분 
    private void SetPoints(int startPointIndex, float[] 비트arr)
    {
        // startPointIndex < 0
        if (startPointIndex<0)
        {
            for (int j=-startPointIndex; j<비트arr.Length; j++)
            {
                points[j + startPointIndex]= new Vector2(points[j + startPointIndex].x, 비트arr[j]);
            }
        }

        // 21-startPointIndex < 비트arr.Length
        else if ((points.Count - startPointIndex) < 비트arr.Length)
        {
            for (int j = 0; j < points.Count - startPointIndex; j++)
            {
                //print("startPointIndex + j :" + (startPointIndex + j).ToString() + ", points[startPointIndex + j].x: " + points[startPointIndex + j].x + "비트arr[j]: " + 비트arr[j]);
                points[startPointIndex + j] = new Vector2(points[startPointIndex + j].x, 비트arr[j]);
            }
        }

        // 그 외 경우 0 <= startPointIndex ~ 
        else
        {
            for (int j = 0; j < 비트arr.Length; j++)
            {
                points[startPointIndex+j] = new Vector2(points[startPointIndex + j].x, 비트arr[j]);
            }
        }

    }

}
