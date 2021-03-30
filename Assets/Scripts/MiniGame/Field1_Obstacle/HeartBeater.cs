using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeater : MonoBehaviour
{

    //Points Y 좌표만 바꿔주면 됩니다.  Y좌표를 담을 float배열. 
    [SerializeField]
    float[] pointY;
    //비트arr들. 비트 타입별로
    [SerializeField]
    float[] beatType0, beatType1, beatType2, beatType3, beatType4, beatType5;




    void Start()
    {
        //UI Line Renderer에서 포인트들 배열 받아오자. Points []
        
    }


    void Update()
    {
        
    }
    

    //실제 Point 배치 처리 부분. 
    private void SetPoints(int startPointIndex, float[] 비트arr)
    {
        // startPointIndex < 0

        // 21-startPointIndex < 비트arr.Length

        // 그 외 경우 0 <= startPointIndex ~ 


    }

}
